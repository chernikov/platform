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

        public bool SaveSBCValue(int idUser, SBCValue.SbcType type, double value)
        {

            var sbcValue = Db.SBCValues.FirstOrDefault(p => p.UserID == idUser && p.AddedDate == DateTime.Now.Date);
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
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        TeamID = user.PlayerOfTeamID,
                        FieldPositionID = user.FieldPositionID,
                        AddedDate = DateTime.Now.Date
                    };
                }
                else
                {
                    sbcValue = new SBCValue
                    {
                        UserID = idUser,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        TeamID = user.PlayerOfTeamID,
                        FieldPositionID = user.FieldPositionID,
                        AddedDate = DateTime.Now.Date
                    };
                }
                Db.SBCValues.InsertOnSubmit(sbcValue);
                Db.Users.Context.SubmitChanges();
            }

            switch (type)
            {
                case SBCValue.SbcType.Squat:
                    sbcValue.Squat = value;
                    break;
                case SBCValue.SbcType.Bench:
                    sbcValue.Bench = value;
                    break;
                case SBCValue.SbcType.Clean:
                    sbcValue.Clean = value;
                    break;
            }
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
                cache.FirstName = instance.FirstName;
                cache.LastName = instance.LastName;
                cache.TeamID = instance.TeamID;
                cache.FieldPositionID = instance.FieldPositionID;
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