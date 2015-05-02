using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace platformAthletic.Model
{

    public partial class SqlRepository
    {
        public IQueryable<SBCValue> SBCValues
        {
            get
            {
                return Db.SBCValues;
            }
        }

        private bool SaveSBCValue(int idUser, double squat, double bench, double clean, DateTime? addedDate = null)
        {

            var sbcValue = Db.SBCValues.FirstOrDefault(p => p.UserID == idUser && p.AddedDate == (addedDate ?? CurrentDateTime).Date);
            if (sbcValue == null)
            {
                var user = Db.Users.FirstOrDefault(p => p.ID == idUser);
   
                var previous = Db.SBCValues.OrderByDescending(p => p.ID).FirstOrDefault(p => p.UserID == idUser);

                if (previous != null)
                {
                    sbcValue = new SBCValue
                    {
                        UserID = idUser,
                        Squat = previous.Squat,
                        Bench = previous.Bench,
                        Clean = previous.Clean,
                        TeamID = user.PlayerOfTeamID,
                        AddedDate = (addedDate ?? CurrentDateTime).Date
                    };
                }
                else
                {
                    sbcValue = new SBCValue
                    {
                        UserID = idUser,
                        TeamID = user.PlayerOfTeamID,
                        AddedDate = (addedDate ?? CurrentDateTime).Date
                    };
                }
                Db.SBCValues.InsertOnSubmit(sbcValue);
                Db.Users.Context.SubmitChanges();
            }
            sbcValue.Squat = squat;
            sbcValue.Bench = bench;
            sbcValue.Clean = clean;
            
            Db.Users.Context.SubmitChanges();
            return true;
        }


        public bool UpdateSbcValue(SBCValue instance)
        {
            var cache = Db.SBCValues.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
                cache.Squat = instance.Squat;
                cache.Bench = instance.Bench;
                cache.Clean = instance.Clean;
                cache.TeamID = instance.TeamID;
                Db.SBCValues.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveSbcValue(int idSbcValue)
        {
            var instance = Db.SBCValues.FirstOrDefault(p => p.ID == idSbcValue);
            if (instance != null)
            {
                Db.SBCValues.DeleteOnSubmit(instance);
                Db.SBCValues.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}