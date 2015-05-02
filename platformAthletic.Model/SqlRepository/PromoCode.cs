using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using platformAthletic.Tools;


namespace platformAthletic.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<PromoCode> PromoCodes
        {
            get
            {
                return Db.PromoCodes;
            }
        }

        public bool CreatePromoCode(PromoCode instance)
        {
            if (instance.ID == 0)
            {
                Db.PromoCodes.InsertOnSubmit(instance);
                Db.PromoCodes.Context.SubmitChanges();
                return true;
            }

            return false;
        }

       public bool UsePromoCode(string referralCode)
        {
            var instance = Db.PromoCodes.Where(p => !p.PromoAction.Closed && (p.PromoAction.ValidDate == null || p.PromoAction.ValidDate.Value > CurrentDateTime)).OrderBy(p => p.UsedDate.HasValue ? 1 : 0).FirstOrDefault(p => p.ReferralCode == referralCode);
            if (instance != null)
            {
                if (!instance.UsedDate.HasValue)
                {
                    instance.UsedDate = CurrentDateTime;
                    Db.PromoCodes.Context.SubmitChanges();
                    return true;
                } 
            }
            return false;
        }

        public bool RemovePromoCode(int idPromoCode)
        {
            var instance = Db.PromoCodes.FirstOrDefault(p => p.ID == idPromoCode);
            if (instance != null)
            {
                Db.PromoCodes.DeleteOnSubmit(instance);
                Db.PromoCodes.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool GeneratePromoCodes(int promoActionId, int quantity, string referralCode)
        {
            var promoAction = Db.PromoActions.FirstOrDefault(p => p.ID == promoActionId);

            if (promoAction != null)
            {
                var additional = quantity - promoAction.PromoCodes.Count();
                if (additional > 0)
                {
                    for (int i = 0; i < additional; i++)
                    {
                        var newPromoCode = new PromoCode()
                        {
                            PromoActionID = promoAction.ID,
                            AddedDate = CurrentDateTime,
                        };
                        if (!string.IsNullOrWhiteSpace(referralCode))
                        {
                            newPromoCode.ReferralCode = referralCode;
                        }
                        else
                        {
                            newPromoCode.ReferralCode = StringExtension.CreateRandomPassword(15, "ABCDEFGHJKLMNOPQRSTUVWXYZ0123456789");
                        }
                        CreatePromoCode(newPromoCode);
                    }
                }
                else if (additional < 0)
                {
                    var toRemove = -additional;
                    var nonUsedPromoCodes = promoAction.PromoCodes.Where(p => !p.UsedDate.HasValue).ToList();

                    if (nonUsedPromoCodes.Count() < toRemove)
                    {
                        toRemove = nonUsedPromoCodes.Count();
                    }

                    for (int i = 0; i < toRemove; i++)
                    {
                        RemovePromoCode(nonUsedPromoCodes[i].ID);
                    }
                }
                return true;
            }
            return false;
        }


        public double GetDiscountByPromoCode(int idPromoCode, double totalPrice, PromoAction.TargetEnum target)
        {
            var promoCode = Db.PromoCodes.Where(p => !p.PromoAction.Closed && (p.PromoAction.ValidDate == null || p.PromoAction.ValidDate.Value > CurrentDateTime)).OrderBy(p => p.UsedDate.HasValue ? 1 : 0).FirstOrDefault(p => p.ID == idPromoCode);

            if (promoCode != null)
            {
                var promoAction = promoCode.PromoAction;

                if (promoAction.Closed)
                {
                    return totalPrice;
                }

                if (promoAction.ValidDate.HasValue && promoAction.ValidDate.Value < CurrentDateTime)
                {
                    return totalPrice;
                }

                if (!promoAction.Reusable && promoCode.Used)
                {
                    return totalPrice;
                }
                if (promoAction.Target == (int)PromoAction.TargetEnum.Both || promoAction.Target == (int)target)
                {
                    if (promoAction.Type == (int)PromoAction.TypeEnum.Percentage)
                    {
                        return (100 - promoAction.Amount) * totalPrice / 100;
                    }

                    if (promoAction.Type == (int)PromoAction.TypeEnum.Absolute)
                    {
                        return totalPrice - promoAction.Amount;
                    }
                }
            }
            return totalPrice;
        }

        public bool ValidatePromoCode(string referralCode, PromoAction.TargetEnum target)
        {
            var promoCode = Db.PromoCodes.Where(p => !p.PromoAction.Closed && (p.PromoAction.ValidDate == null || p.PromoAction.ValidDate.Value > CurrentDateTime)).OrderBy(p => p.UsedDate.HasValue ? 1 : 0).FirstOrDefault(p => string.Compare(p.ReferralCode, referralCode, true) == 0);

            if (promoCode != null)
            {
                var promoAction = promoCode.PromoAction;

                if (promoAction.Closed)
                {
                    return false;
                }
                if (promoAction.ValidDate.HasValue && promoAction.ValidDate.Value < CurrentDateTime)
                {
                    return false;
                }
                if (!promoAction.Reusable && promoCode.Used)
                {
                    return false;
                }
                if (promoAction.Target == (int)PromoAction.TargetEnum.Both || promoAction.Target == (int)target)
                {
                    return true;
                }
            }
            return false;
        }
    }
}