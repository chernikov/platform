using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Model;
using platformAthletic.Models.ViewModels;
using platformAthletic.Models.ViewModels.User;
using platformAthletic.Tools.Mail;
using platformAthletic.Helpers;
using platformAthletic.Models.Info;
using System.IO;
using platformAthletic.Tools;
using System.Drawing;

namespace platformAthletic.Areas.Default.Controllers
{
    public class AccountController : DefaultController
    {
        private static string LogoFolder = "/Media/files/logo/";
        private static string AvatarFolder = "/Media/files/avatars/";

        private static string TeamLogoSize = "TeamLogoSize";
        private static string AvatarSize = "AvatarSize";

        public bool EnableHttps 
        {
            get
            {
                return Config.EnableHttps;
            }
        }

        [HttpGet]
        [Authorize(Roles = "individual,coach,player")]
        public ActionResult Index()
        {
            if (CurrentUser.InRoles("individual"))
            {
                var individualUserView = (IndividualUserView)ModelMapper.Map(CurrentUser, typeof(User), typeof(IndividualUserView));

                return View("IndividualAccount", individualUserView);
            }
            if (CurrentUser.InRoles("coach"))
            {
                var teamUserView = (TeamUserView)ModelMapper.Map(CurrentUser, typeof(User), typeof(TeamUserView));
                return View("TeamAccount", teamUserView);
            }
            if (CurrentUser.InRoles("player"))
            {
                var teamUserView = (PlayerUserView)ModelMapper.Map(CurrentUser, typeof(User), typeof(PlayerUserView));
                return View("PlayerAccount", teamUserView);
            }
            return RedirectToLoginPage;
        }

        [HttpPost]
        [Authorize(Roles = "individual,coach,player")]
        public ActionResult Index(BaseUserView userView)
        {
            if (userView is IndividualUserView)
            {
                var individualUserView = (IndividualUserView)userView;

                if (ModelState.IsValid)
                {
                    ViewBag.Message = "Saved";
                    var user = (User)ModelMapper.Map(individualUserView, typeof(IndividualUserView), typeof(User));
                    user.ID = CurrentUser.ID;
                    Repository.UpdateUser(user);
                    Repository.SetUserColors(user);
                }
                return View("IndividualAccount", individualUserView);
            }

            if (userView is TeamUserView)
            {
                var teamUserView = (TeamUserView)userView;

                if (ModelState.IsValid)
                {
                    ViewBag.Message = "Saved";
                    var user = (User)ModelMapper.Map(teamUserView, typeof(TeamUserView), typeof(User));
                    var team = (Team)ModelMapper.Map(teamUserView.Team, typeof(TeamView), typeof(Team));
                    user.ID = CurrentUser.ID;
                    Repository.UpdateUser(user);
                    team.ID = CurrentUser.OwnTeam.ID;
                    Repository.UpdateTeam(team);
                }
                return View("TeamAccount", teamUserView);
            }
            if (userView is PlayerUserView)
            {
                var playerUserView = (PlayerUserView)userView;

                if (ModelState.IsValid)
                {
                    ViewBag.Message = "Saved";
                    var user = (User)ModelMapper.Map(playerUserView, typeof(PlayerUserView), typeof(User));
                    user.ID = CurrentUser.ID;
                    Repository.UpdateUser(user);
                }
                return View("PlayerAccount", playerUserView);
            }
            return RedirectToLoginPage;
        }

        #region Register & payment

        [HttpGet]
        public ActionResult RegisterIndividual()
        {
            if (Request.Url.Scheme == "http" && !Request.IsLocal && EnableHttps)
            {
                return Redirect("https://" + HostName + "/individual-registration");
            }
            var registerUserView = new RegisterIndividualView
            {
                RegisterType = RegisterUserView.RegisterTypeEnum.Individual,
            };
            return View(registerUserView);
        }

        [HttpPost]
        public ActionResult RegisterIndividual(RegisterIndividualView registerIndividualView)
        {
            if (!registerIndividualView.AgreementPrivacyPolicy || !registerIndividualView.AgreementTermCondition)
            {
                ModelState.AddModelError("Agreement", "You need to accept Privacy Policy and Terms and Conditions to continue.");
            }
            ValidateCreditCardExpirationDate(registerIndividualView.BillingInfo);
            int? promoCode = null;
            ValidatePromoCode(registerIndividualView.Target, registerIndividualView.ReferralCode, out promoCode);
            if (promoCode.HasValue)
            {
                registerIndividualView.PromoCodeID = promoCode.Value;
            }
            if (ModelState.IsValid)
            {
                var user = (User)ModelMapper.Map(registerIndividualView, typeof(RegisterIndividualView), typeof(User));
                user.ActivatedDate = DateTime.Now.Current();
                Repository.CreateUser(user);

                /* process payment */
                var billingInfoView = ((RegisterIndividualView)registerIndividualView).BillingInfo;
                if (billingInfoView != null)
                {
                    var billingInfo = (BillingInfo)ModelMapper.Map(billingInfoView, typeof(BillingInfoView), typeof(BillingInfo));
                    billingInfo.UserID = user.ID;
                    Repository.CreateBillingInfo(billingInfo);
                    if (!ProcessPayment(billingInfo, registerIndividualView.TotalSum, registerIndividualView.ReferralCode, registerIndividualView.Target))
                    {
                        Repository.RemoveUser(user.ID);
                        if (ModelState.IsValid)
                        {
                            ModelState.AddModelError("Payment", "The credit card information you entered is invalid. Please re-enter payment information.");
                        }
                        return View(registerIndividualView);
                    }
                }
                var userRole = new UserRole()
                {
                    UserID = user.ID,
                    RoleID = 4 //individual
                };
                Repository.CreateUserRole(userRole);
                NotifyMail.SendNotify(Config, "RegisterIndividual", user.Email,
                          (u, format) => format,
                          (u, format) => string.Format(format, u.Email, u.Password),
                          user);
                Auth.Login(user.Email);
                return Redirect("http://" + HostName + "/register-success");
            }
            return View(registerIndividualView);
        }

        [HttpGet]
        public ActionResult RegisterTeam()
        {
            if (Request.Url.Scheme == "http" && !Request.IsLocal && EnableHttps)
            {
                return Redirect("https://" + HostName + "/team-registration");
            }
            var registerUserView = new RegisterTeamView
            {
                RegisterType = RegisterUserView.RegisterTypeEnum.Coach,
            };
            return View(registerUserView);
        }


        [HttpPost]
        public ActionResult RegisterTeam(RegisterTeamView registerTeamView)
        {
            if (!registerTeamView.AgreementPrivacyPolicy || !registerTeamView.AgreementTermCondition)
            {
                ModelState.AddModelError("Agreement", "You need to accept Privacy Policy and Terms and Conditions to continue.");
            }
            int? promoCode = null;
            ValidatePromoCode(registerTeamView.Target, registerTeamView.ReferralCode, out promoCode);
            if (promoCode.HasValue)
            {
                registerTeamView.PromoCodeID = promoCode.Value;
            }
            if (ModelState.IsValid)
            {
                var user = (User)ModelMapper.Map(registerTeamView, typeof(RegisterTeamView), typeof(User));
                user.ActivatedDate = DateTime.Now.Current();
                Repository.CreateUser(user);

                var invoice = new Invoice()
                {
                    NameOfOrganization = registerTeamView.Team.Name,
                    City = "None",
                    StateID = registerTeamView.Team.StateID,
                    ZipCode = "0",
                    PhoneNumber = registerTeamView.PhoneNumber,
                    TotalSum = registerTeamView.TotalSum,
                    UserID = user.ID,
                };
                Repository.CreateInvoice(invoice);
                ProcessPayment(invoice, registerTeamView.TotalSum, registerTeamView.ReferralCode, registerTeamView.Target);

                var userRole = new UserRole()
                {
                    UserID = user.ID,
                    RoleID = 2 //coach
                };
                Repository.CreateUserRole(userRole);
                var team = (Team)ModelMapper.Map(registerTeamView.Team, typeof(TeamView), typeof(Team));
                team.UserID = user.ID;
                Repository.CreateTeam(team);
                NotifyMail.SendNotify(Config, "Register", user.Email,
                          (u, format) => format,
                          (u, format) => string.Format(format, u.Email, u.Password),
                          user);

                Auth.Login(user.Email);
                return Redirect("http://" + HostName + "/register-success");
            }
            return View(registerTeamView);
        }

        private void ValidatePromoCode(PromoAction.TargetEnum target, string referralCode, out int? promoCode)
        {
            promoCode = null;
            if (!string.IsNullOrWhiteSpace(referralCode))
            {
                if (!Repository.ValidatePromoCode(referralCode, target))
                {
                    ModelState.AddModelError("ReferralCode", "Referral code is invalid");
                }
                else
                {
                    var code = Repository.PromoCodes.Where(p => !p.PromoAction.Closed && (p.PromoAction.ValidDate == null || p.PromoAction.ValidDate.Value > DateTime.Now.Current())).OrderBy(p => p.UsedDate.HasValue ? 1 : 0).FirstOrDefault(p => string.Compare(p.ReferralCode, referralCode, true) == 0);
                    if (code != null)
                    {
                        promoCode = code.ID;
                    }
                }
            }
        }

        private void ValidateCreditCardExpirationDate(BillingInfoView BillingInfo)
        {
            var expirationDate = new DateTime(BillingInfo.ExpirationYear, BillingInfo.ExpirationMonth, 1);
            if (expirationDate <= DateTime.Now.Current())
            {
                ModelState.AddModelError("BillingInfo.ExpirationMonth", "Expiration Date not Valid");
                ModelState.AddModelError("BillingInfo.ExpirationYear", "Expiration Date not Valid");
            }
        }

        [Authorize]
        public ActionResult RegisterSuccess()
        {
            ViewBag.Coach = CurrentUser.InRoles("coach");
            return View(CurrentUser);
        }

        public ActionResult InvoicePart()
        {
            var invoiceView = new InvoiceView();
            return View(invoiceView);
        }

        public ActionResult CreditCardPart()
        {
            var billingInfoView = new BillingInfoView();
            return View(billingInfoView);
        }

        public ActionResult ApplyReferrerCode(PromoAction.TargetEnum target, string code)
        {
            double total = 0.0;
            if (target == PromoAction.TargetEnum.TeamSubscription)
            {
                total = Repository.Settings.First(p => p.Name == "TeamPrice").ValueDouble;
            }
            if (target == PromoAction.TargetEnum.IndividualSubscription)
            {
                total = Repository.Settings.First(p => p.Name == "IndividualPrice").ValueDouble;
            }
            var promoCode = Repository.PromoCodes.Where(p => !p.PromoAction.Closed && (p.PromoAction.ValidDate == null || p.PromoAction.ValidDate.Value > DateTime.Now.Current())).OrderBy(p => p.UsedDate.HasValue ? 1 : 0).FirstOrDefault(p => string.Compare(p.ReferralCode, code, true) == 0);

            if (promoCode != null)
            {
                var discount = Repository.GetDiscountByPromoCode(promoCode.ID, total, target);
                if (discount != total)
                {
                    return Json(new { result = "ok", sum = discount.ToString("C"), promoCode = promoCode.ID }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { result = "error", sum = total.ToString("C") }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region set Color
        [HttpGet]
        [Authorize]
        public ActionResult SetColors()
        {
            return View(CurrentUser);
        }

        [HttpPost]
        [Authorize]
        public ActionResult SetColors(string primaryColor, string secondaryColor)
        {
            var team = CurrentUser.Teams.FirstOrDefault();
            if (team != null)
            {
                team.PrimaryColor = primaryColor;
                team.SecondaryColor = secondaryColor;
                Repository.UpdateTeam(team);
            }
            CurrentUser.PrimaryColor = primaryColor;
            CurrentUser.SecondaryColor = secondaryColor;

            Repository.SetUserColors(CurrentUser);
            return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Select Season

        [HttpGet]
        public ActionResult SelectSeason()
        {
            return View(CurrentUser);
        }

        [HttpPost]
        public ActionResult SetSeason(UserSeasonView userSeasonView)
        {
            if (userSeasonView.StartDay.Date.DayOfWeek != DayOfWeek.Sunday)
            {
                return Json(new { result = "error" }); 
            }
            if (ModelState.IsValid)
            {
                var userSeason = (UserSeason)ModelMapper.Map(userSeasonView, typeof(UserSeasonView), typeof(UserSeason));
                userSeason.UserID = CurrentUser.ID;
                Repository.CreateUserSeason(userSeason);

                return Json(new { result = "ok" });
            }
            return Json(new { result = "error" }); 
        }


        [HttpPost]
        public ActionResult SelectSeason(UserSeasonView userSeasonView)
        {
            
            if (ModelState.IsValid)
            {
                var userSeason = (UserSeason)ModelMapper.Map(userSeasonView, typeof(UserSeasonView), typeof(UserSeason));
                userSeason.UserID = CurrentUser.ID;
                Repository.CreateUserSeason(userSeason);

                return RedirectToAction("Thanks");
            }
            return View(CurrentUser);
        }

        [Authorize(Roles = "individual,coach")]
        public ActionResult SetNextSeason(DateTime startDay)
        {
            var nextUserSeason = CurrentUser.NextSeason;
            if (nextUserSeason == null)
            {
                var currentSeason = CurrentUser.CurrentSeason;
                Season nextSeason = null;
                if (currentSeason != null)
                {
                    nextSeason = Repository.Seasons.FirstOrDefault(p => p.ID != currentSeason.SeasonID);
                }
                else
                {
                    nextSeason = Repository.Seasons.FirstOrDefault();
                }

                var newUserSeason = new UserSeason
                {
                    UserID = CurrentUser.ID,
                    SeasonID = nextSeason.ID,
                    StartDay = startDay
                };
                Repository.CreateUserSeason(newUserSeason);
            }
            else
            {
                nextUserSeason.StartDay = startDay;
                Repository.UpdateUserSeason(nextUserSeason);
            }
            return Json(new { result = "ok" });
        }

        #endregion

        public ActionResult Thanks()
        {
            return View(CurrentUser);
        }

        #region change password

        [HttpGet]
        [Authorize]
        public ActionResult ChangePassword()
        {
            var changePasswordView = new ChangePasswordView
            {
                ID = CurrentUser.ID
            };
            return View(changePasswordView);
        }

        [HttpPost]
        [Authorize]
        public ActionResult ChangePassword(ChangePasswordView changePasswordView)
        {
            if (ModelState.IsValid)
            {
                CurrentUser.Password = changePasswordView.NewPassword;
                Repository.ChangePassword(CurrentUser);
                ViewBag.Message = "Changed";
                changePasswordView = new ChangePasswordView
                {
                    ID = CurrentUser.ID
                };
            }
            return View(changePasswordView);
        }

        #endregion

        #region Billing

        [HttpGet]
        [Authorize(Roles = "individual,coach,player")]
        public ActionResult Billing()
        {
            if (Request.Url.Scheme == "http" && !Request.IsLocal && EnableHttps)
            {
                return Redirect("https://" + HostName + "/billing");
            }
            ViewBag.OpenPayment = false;
            if (CurrentUser.InRoles("individual"))
            {
                var individualBillingUserView = (IndividualBillingUserView)ModelMapper.Map(CurrentUser, typeof(User), typeof(IndividualBillingUserView));
                return View("IndividualBillingInfo", individualBillingUserView);
            }
            if (CurrentUser.InRoles("player"))
            {
                return View("SubscriptionDontPayed");
            }
            if (CurrentUser.InRoles("coach"))
            {
                var teamBillingUserView = (TeamBillingUserView)ModelMapper.Map(CurrentUser, typeof(User), typeof(TeamBillingUserView));
                return View("TeamBillingInfo", teamBillingUserView);
            }
            return RedirectToLoginPage;
        }

        [HttpPost]
        [Authorize(Roles = "individual,coach")]
        public ActionResult Billing(BillingUserView billingUserView)
        {
            ViewBag.OpenPayment = true;
            int? promoCode = null;
            ValidatePromoCode(billingUserView.Target, billingUserView.BillingInfo.ReferralCode, out promoCode);
            if (promoCode != null)
            {
                billingUserView.PromoCodeID = promoCode;
            }
            if (ModelState.IsValid)
            {
                var billingInfo = (BillingInfo)ModelMapper.Map(billingUserView.BillingInfo, typeof(BillingInfoView), typeof(BillingInfo));
                billingInfo.UserID = CurrentUser.ID;
                if (billingInfo.ID == 0)
                {
                    Repository.CreateBillingInfo(billingInfo);
                }
                else
                {
                    billingInfo = Repository.UpdateBillingInfo(billingInfo);
                }
                if (ProcessPayment(billingInfo, billingUserView.TotalSum, billingUserView.BillingInfo.ReferralCode, billingUserView.Target))
                {
                    ViewBag.Message = "Payment accepted";
                    ViewBag.OpenPayment = false;
                    billingUserView.PaidTill = CurrentUser.PaidTill.Value;
                }
                else
                {
                    ModelState.AddModelError("BillingInfo.Payment", "The credit card you entered is invalid. Please re-enter payment information");
                }
            }

            if (billingUserView is TeamBillingUserView)
            {
                return View("TeamBillingInfo", billingUserView);
            }
            if (billingUserView is IndividualBillingUserView)
            {

                return View("IndividualBillingInfo", billingUserView);
            }
            return RedirectToLoginPage;
        }

        [HttpPost]
        [Authorize(Roles = "individual,coach")]
        public ActionResult CancelAutoDebit()
        {
            var billingInfo = CurrentUser.BillingInfo;
            if (billingInfo != null)
            {
                Repository.CancelAutoDebit(billingInfo.ID);
            }
            return Json(new { result = "ok" });
        }

        #endregion

        #region Equipment

        [HttpGet]
        [Authorize(Roles = "individual,coach")]
        public ActionResult Equipment()
        {
            var equipmentList = Repository.Equipments.OrderBy(p => p.Name).ToList();
            var selectedEquipmentList = new SelectedEquipmentList(equipmentList, CurrentUser);
            return View(selectedEquipmentList);
        }

        [HttpPost]
        [Authorize(Roles = "individual,coach")]
        public ActionResult Equipment(SelectedEquipmentList selectedEquipmentList)
        {
            ViewBag.Message = "Saved";
            foreach (var item in selectedEquipmentList.List)
            {
                if (item.Select && !CurrentUser.UserEquipments.Any(p => p.EquipmentID == item.Equipment.ID))
                {
                    var userEquipment = new UserEquipment
                    {
                        EquipmentID = item.Equipment.ID,
                        UserID = CurrentUser.ID
                    };
                    Repository.CreateUserEquipment(userEquipment);
                }
                if (!item.Select && CurrentUser.UserEquipments.Any(p => p.EquipmentID == item.Equipment.ID))
                {
                    var userEquipment = CurrentUser.UserEquipments.First(p => p.EquipmentID == item.Equipment.ID);
                    Repository.RemoveUserEquipment(userEquipment.ID);
                }
            }
            return View(selectedEquipmentList);
        }

        [HttpPost]
        public JsonResult UploadTeamLogo(string qqfile)
        {
            string fileName;
            var inputStream = GetInputStream(qqfile, out fileName);
            if (inputStream != null)
            {
                var extension = Path.GetExtension(fileName);
                if (extension != null)
                {
                    extension = extension.ToLower();
                    var mimeType = Config.MimeTypes.FirstOrDefault(p => p.Extension == extension);

                    if (mimeType != null && mimeType.Name.Contains("image"))
                    {
                        var logoSizes = Config.IconSizes.FirstOrDefault(c => c.Name == TeamLogoSize);
                        if (logoSizes != null)
                        {
                            var logoSize = new Size(logoSizes.Width, logoSizes.Height);
                            if (PreviewCreator.CheckSize(inputStream, logoSize) && (mimeType.Name == "image/png" || mimeType.Name == "image/gif"))
                            {
                                string imagePath = null;
                                if (mimeType.Name == "image/png")
                                {
                                    imagePath = string.Format("{0}{1}.png", LogoFolder, StringExtension.GenerateNewFile());
                                }
                                if (mimeType.Name == "image/gif")
                                {
                                    imagePath = string.Format("{0}{1}.gif", LogoFolder, StringExtension.GenerateNewFile());
                                }

                                using (var file = new FileStream(Server.MapPath(imagePath), FileMode.Create))
                                {
                                    inputStream.Position = 0;
                                    inputStream.CopyTo(file);
                                }
                                return Json(new
                                {
                                    result = "ok",
                                    imagePath = imagePath
                                }, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                var imagePath = string.Format("{0}{1}.jpg", LogoFolder, StringExtension.GenerateNewFile());
                                MakeImage(inputStream, TeamLogoSize, imagePath, Brushes.Black);
                                return Json(new
                                {
                                    result = "ok",
                                    imagePath = imagePath
                                }, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    else
                    {
                        return Json(new
                        {
                            result = "wrong-file type",
                            error = "File type must be .gif or .png"
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return Json(new
            {
                result = "error",
                error = "Unknow error"
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UploadAvatar(string qqfile)
        {
            string fileName;
            var inputStream = GetInputStream(qqfile, out fileName);
            if (inputStream != null)
            {
                var extension = Path.GetExtension(fileName);
                if (extension != null)
                {
                    extension = extension.ToLower();
                    var mimeType = Config.MimeTypes.FirstOrDefault(p => p.Extension == extension);

                    if (mimeType != null && mimeType.Name.Contains("image"))
                    {
                        var imagePath = string.Format("{0}{1}.jpg", AvatarFolder, StringExtension.GenerateNewFile());
                        MakePreview(inputStream, AvatarSize, imagePath, Brushes.White);
                        return Json(new
                        {
                            result = "ok",
                            imagePath = imagePath
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return Json(new
            {
                result = "error"
            }, JsonRequestBehavior.AllowGet);
        }

        private void MakePreview(Stream ms, string avatarSize, string fileName, Brush brush)
        {
            var avatarSizes = Config.IconSizes.FirstOrDefault(c => c.Name == avatarSize);
            if (avatarSizes != null)
            {
                var previewSize = new Size(avatarSizes.Width, avatarSizes.Height);
                PreviewCreator.CreateAndSaveAvatar(ms, previewSize, Server.MapPath(fileName), brush);
            }
        }


        private void MakeImage(Stream ms, string avatarSize, string fileName, Brush brush)
        {
            var avatarSizes = Config.IconSizes.FirstOrDefault(c => c.Name == avatarSize);
            if (avatarSizes != null)
            {
                var previewSize = new Size(avatarSizes.Width, avatarSizes.Height);
                PreviewCreator.CreateAndSaveImage(ms, previewSize, Server.MapPath(fileName), brush);
            }
        }

        #endregion

        [HttpGet]
        [Authorize(Roles = "coach")]
        public ActionResult Settings()
        {
            var team = CurrentUser.OwnTeam;

            return View(team);
        }


        [HttpPost]
        [Authorize(Roles = "coach")]
        public ActionResult AjaxSettings(int control)
        {
            var team = CurrentUser.OwnTeam;
            team.SBCControl = control;
            Repository.UpdateTeam(team);
            return Json(new { result = "ok" });
        }
    }
}
