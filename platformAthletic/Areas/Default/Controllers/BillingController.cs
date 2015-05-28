using platformAthletic.Helpers;
using platformAthletic.Model;
using platformAthletic.Models.ViewModels;
using platformAthletic.Models.ViewModels.User;
using System;
using System.Linq;
using System.Web.Mvc;


namespace platformAthletic.Areas.Default.Controllers
{
    public class BillingController : DefaultController
    {
        public bool EnableHttps
        {
            get
            {
                return Config.EnableHttps;
            }
        }

        public ActionResult Index()
        {
            if (Request.Url.Scheme == "http" && !Request.IsLocal && EnableHttps)
            {
                return Redirect("https://" + HostName + "/billing");
            }
            ViewBag.OpenPayment = false;
            if (CurrentUser.InRoles("individual"))
            {
                var individualBillingUserView = (IndividualBillingUserView)ModelMapper.Map(CurrentUser, typeof(User), typeof(IndividualBillingUserView));
                return View("IndividualBilling", individualBillingUserView);
            }
            if (CurrentUser.InRoles("player"))
            {
                return View("SubscriptionDontPayed");
            }
            if (CurrentUser.InRoles("coach"))
            {
                var teamBillingUserView = (TeamBillingUserView)ModelMapper.Map(CurrentUser, typeof(User), typeof(TeamBillingUserView));
                return View("TeamBilling", teamBillingUserView);
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
                return View("TeamBilling", billingUserView);
            }
            if (billingUserView is IndividualBillingUserView)
            {

                return View("IndividualBilling", billingUserView);
            }
            return RedirectToLoginPage;
        }

        [HttpGet]
        public ActionResult CancelAutoDebit()
        {
            return View();
        }


        [HttpPost]
        [Authorize(Roles = "individual,coach")]
        public ActionResult CancelAutoDebit(int id)
        {
            var billingInfo = CurrentUser.BillingInfo;
            if (billingInfo != null)
            {
                Repository.CancelAutoDebit(billingInfo.ID);
            }
            return Json(new { result = "ok" });
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

    }
}
