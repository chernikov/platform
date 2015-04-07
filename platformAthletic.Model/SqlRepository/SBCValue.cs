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

        //TODO: Спросить про такую коллизию, если я на какой-то позиции будучи установил рекорд, а потом каким-то образом меня перевели на другую позицию, 
        //то все мои рекорды как считаются? Должен ли записываться рекорд на текущую позицию, или переносится вместе с позицией.
        private bool SaveSBCValue(int idUser, double squat, double bench, double clean)
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
                        FieldPositionID = null,
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
                        FieldPositionID = null,
                        AddedDate = DateTime.Now.Date
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