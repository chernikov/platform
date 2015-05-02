using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Model;
using platformAthletic.Helpers;
using platformAthletic.Models.ViewModels;

namespace platformAthletic.Areas.Admin.Controllers
{
    public class PromoActionController : AdminController
    {
        public ActionResult Index()
        {
            var list = Repository.PromoActions.ToList();
            return View(list);
        }

        public ActionResult Create()
        {
            var promoActionView = new PromoActionView()
            {
                CanChangeReusable = true
            };
            return View("Edit", promoActionView);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var promoAction = Repository.PromoActions.FirstOrDefault(p => p.ID == id);

            if (promoAction != null)
            {
                var promoActionView = (PromoActionView)ModelMapper.Map(promoAction, typeof(PromoAction), typeof(PromoActionView));
                return View(promoActionView);
            }
            return RedirectToNotFoundPage;
        }

        [HttpPost]
        public ActionResult Edit(PromoActionView promoActionView)
        {
            if (promoActionView.Type == (int)PromoAction.TypeEnum.Percentage && promoActionView.Amount > 100)
            {
                ModelState.AddModelError("Amount", "Amount can't be greater that 100%");
            }
            if (promoActionView.ID != 0)
            {
                var promoAction = Repository.PromoActions.FirstOrDefault(p => p.ID == promoActionView.ID);
                if (promoAction != null)
                {
                    var usedCodesCount = promoAction.UsedCodesCount;
                    if (usedCodesCount > promoActionView.Quantity)
                    {
                        ModelState.AddModelError("Quantity",
                            string.Format("Quantity can't be less that count of used promo codes ({0})", usedCodesCount));
                    }
                }
            }
            if (ModelState.IsValid)
            {
                var promoAction = (PromoAction)ModelMapper.Map(promoActionView, typeof(PromoActionView), typeof(PromoAction));
                if (promoAction.ID == 0)
                {
                    Repository.CreatePromoAction(promoAction);
                    Repository.GeneratePromoCodes(promoAction.ID, promoActionView.Quantity, promoActionView.ReferralCode);
                }
                else
                {
                    Repository.UpdatePromoAction(promoAction);
                    Repository.GeneratePromoCodes(promoAction.ID, promoActionView.Quantity, string.Empty);
                }
                return RedirectToAction("Index");
            }
            return View(promoActionView);
        }

        public ActionResult Delete(int id)
        {
            var promoAction = Repository.PromoActions.FirstOrDefault(p => p.ID == id);
            if (promoAction != null)
            {
                Repository.RemovePromoAction(promoAction.ID);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Reopen(int id)
        {
            var promoAction = Repository.PromoActions.FirstOrDefault(p => p.ID == id);
            if (promoAction != null)
            {
                Repository.ChangeStatePromoAction(promoAction.ID, true);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Close(int id)
        {
            var promoAction = Repository.PromoActions.FirstOrDefault(p => p.ID == id);
            if (promoAction != null)
            {
                Repository.ChangeStatePromoAction(promoAction.ID, false);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult GeneratePromoCode(int id)
        {
            var promoCodeView = new PromoCodeView()
            {
                PromoActionID = id
            };
            return View(promoCodeView);
        }

        [HttpPost]
        public ActionResult GeneratePromoCode(PromoCodeView promoCodeView)
        {
            if (ModelState.IsValid)
            {
                for (int i = 0; i < promoCodeView.Quantity; i++)
                {
                    var promoCode = (PromoCode)ModelMapper.Map(promoCodeView, typeof(PromoCodeView), typeof(PromoCode));
                    promoCode.ID = 0;
                    promoCode.AddedDate = DateTime.Now.Current();
                    Repository.CreatePromoCode(promoCode);
                }
                return View("_OK");
            }
            return View(promoCodeView);
        }
    }
}
