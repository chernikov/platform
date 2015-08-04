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
using StackExchange.Profiling;

namespace platformAthletic.Areas.Default.Controllers
{
    public class AccountController : DefaultController
    {
        public bool EnableHttps 
        {
            get
            {
                return Config.EnableHttps;
            }
        }

      
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

                user.Mode = (int)Model.User.ModeEnum.Tutorial;
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
                        Repository.PurgeUser(user.ID);
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

                var date = DateTime.Now.Current();
                var dayOfWeek = date.DayOfWeek;
                var userSeason = new UserSeason()
                {
                    UserID = user.ID,
                    SeasonID = 1, //Off-season
                    StartDay = date.AddDays(-(int)dayOfWeek)
                };
                Repository.CreateUserSeason(userSeason);

                NotifyMail.SendNotify(Config, "RegisterIndividual", user.Email,
                          (u, format) => format,
                          (u, format) => string.Format(format, u.Email, u.Password),
                          user);
                Auth.Login(user.Email);
                Repository.StartTutorial(user.ID);
                return Redirect("/my-page");
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
                user.Mode = (int)Model.User.ModeEnum.Tutorial;
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

                var date = DateTime.Now.Current();
                var dayOfWeek = date.DayOfWeek;
                var userSeason = new UserSeason()
                {
                    UserID = user.ID,
                    SeasonID = 1, //Off-season
                    StartDay = date.AddDays(-(int)dayOfWeek).Date
                };
                Repository.CreateUserSeason(userSeason);

                NotifyMail.SendNotify(Config, "Register", user.Email,
                          (u, format) => format,
                          (u, format) => string.Format(format, u.Email, u.Password),
                          user);
                Auth.Login(user.Email);
                Repository.StartTutorial(user.ID);
                return Redirect("/thanks");
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

        public ActionResult Thanks()
        {
            return View(CurrentUser);
        }

        public ActionResult GenerateTeam()
        {
            var profiler = MiniProfiler.Current; // it's ok if this is null
            using (profiler.Step("Generate Phantoms"))
            {
                GeneratePhantoms();
            }
            return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
        }

        private void GeneratePhantoms()
        {
            if (CurrentUser.OwnTeam.Players.Any(p => p.IsPhantom))
            {
                return;
            }
            var rand = new Random((int)DateTime.Now.Current().Ticks);
            var group = new Group()
            {
                IsPhantom = true,
                Name = "Football",
                TeamID = CurrentUser.OwnTeam.ID
            };
            Repository.CreateGroup(group);
            CreateUser("John", "Alexander", rand, group);
            CreateUser("Tim", "Brown", rand, group);
            CreateUser("Sarah", "Dodd", rand, group);
            CreateUser("Bill", "Bean", rand, group);
            group = new Group()
            {
                IsPhantom = true,
                Name = "Women's Lax",
                TeamID = CurrentUser.OwnTeam.ID
            };
            Repository.CreateGroup(group);
            CreateUser("Annie", "Faust", rand, group);
            CreateUser("Michelle", "Jenkins", rand, group);
            CreateUser("Sarah", "Parker", rand, group);
            CreateUser("Amy", "Shwartz", rand, group);
            CreateUser("Christy", "Hanson", rand, group);

            group = new Group()
            {
                IsPhantom = true,
                Name = "Mens Bball",
                TeamID = CurrentUser.OwnTeam.ID
            };
            Repository.CreateGroup(group);
            CreateUser("Bill", "Gallihugh", rand, group);
            CreateUser("Jason", "Hinton", rand, group);
            CreateUser("Scott", "King", rand, group);
            CreateUser("Jared", "Nepa", rand, group);
            CreateUser("Frank", "Young", rand, group);


            group = new Group()
            {
                IsPhantom = true,
                Name = "Field Hockey",
                TeamID = CurrentUser.OwnTeam.ID
            };
            Repository.CreateGroup(group);
            CreateUser("Amanda", "Gilmore", rand, group);
            CreateUser("Laura", "Stein", rand, group);
            CreateUser("Jess", "Smith", rand, group);
            CreateUser("Carla", "Parker", rand, group);
            CreateUser("Caitlin", "Beber", rand, group);
        }

        private void CreateUser(string firstName, string lastName, Random rand, Group group)
        {
            var profiler = MiniProfiler.Current; // it's ok if this is null
            using (profiler.Step("CreateUser:" + firstName + " " + lastName))
            {
                var user = new User()
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = firstName.ToLower() + "." + lastName.ToLower() + "@mailinator.com",
                    PlayerOfTeamID = CurrentUser.OwnTeam.ID,
                    Password = "123456",
                    GroupID = group.ID,
                    IsPhantom = true
                };

                Repository.CreateUser(user);

                var playerRole = new UserRole()
                {
                    UserID = user.ID,
                    RoleID = 3 //player
                };
                Repository.CreateUserRole(playerRole);
                for (int i = 0; i < 3; i++)
                {
                    var fieldPosition = Repository.FieldPositions.ToList().OrderBy(p => Guid.NewGuid()).FirstOrDefault();
                    var userPosition = new UserFieldPosition()
                    {
                        UserID = user.ID,
                        FieldPositionID = fieldPosition.ID,
                        SportID = fieldPosition.Sport.ID
                    };
                    Repository.CreateUserFieldPosition(userPosition);
                }
                var squat = 200 + rand.Next(20) * 5;
                var bench = 150 + rand.Next(10) * 5;
                var clean = 100 + rand.Next(7) * 5;
                var startDate = DateTime.Now.Current().AddDays(-90);
                for (int i = 0; i < 4; i++)
                {
                    Repository.SetSbcValue(user.ID, SBCValue.SbcType.Squat, squat, startDate);
                    Repository.SetSbcValue(user.ID, SBCValue.SbcType.Bench, bench, startDate);
                    Repository.SetSbcValue(user.ID, SBCValue.SbcType.Clean, clean, startDate);
                    squat += rand.Next(10) * 5;
                    bench += rand.Next(5) * 5;
                    clean += rand.Next(3) * 5;
                    startDate = startDate.AddDays(21);
                }

                startDate = DateTime.Now.Current().AddDays(-90);
                while (startDate < DateTime.Now.Current())
                {
                    startDate = startDate.AddDays(rand.Next(3) + 2);
                    var userAttendance = new UserAttendance()
                    {
                        UserID = user.ID,
                        AddedDate = startDate,
                        UserSeasonID = user.CurrentSeason.ID
                    };
                    Repository.CreateUserAttendance(userAttendance);
                }
            }
        }

        public ActionResult RemoveTeam()
        {
            RemovePhantoms();
            return Content("OK");
        }

        private void RemovePhantoms()
        {
            Repository.RemovePhantoms(CurrentUser.ID);
        }
    }
}
