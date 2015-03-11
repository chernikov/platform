using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ManageAttribute;
using platformAthletic.Model;


namespace platformAthletic.Models.ViewModels
{ 
	public class TrainingDayCellView
    {
        public int _ID 
        { 
            get {
                return ID;
            }
            set {
                ID = value;
            }
        }
            
        public int ID { get; set; }

		public int CellID {get; set; }

		public string PrimaryText {get; set; }

		public int? SBCType {get; set; }

		public double? Coefficient {get; set; }

        public int TrainingDayID { get; set; }

        public int? TrainingSetID { get; set; }

        private IEnumerable<TrainingSet> TrainingSets
        {
            get
            {
                var repository = DependencyResolver.Current.GetService<IRepository>();
                var trainingDay = repository.TrainingDays.FirstOrDefault(p => p.ID == TrainingDayID);
                if (trainingDay != null)
                {
                    var phaseID = trainingDay.Week.PhaseID;
                    var dayID = trainingDay.DayID;
                    return repository.TrainingSets.Where(p => p.PhaseID == phaseID && p.DayID == dayID).ToList();
                }
                return null;
            }
        }
        
        public IEnumerable<SelectListItem> SelectListTrainingSetID
        {
        	get
        	{
                if (TrainingSets != null)
                {
                    return TrainingSets.Select(p => new SelectListItem
                                                        {
                                                            Value = p.ID.ToString(),
                                                            Text = p.Names,
                                                            Selected = p.ID == TrainingSetID
                                                        });
                }
                return new List<SelectListItem>();
        	}
        }


        public IEnumerable<SelectListItem> SelectListSBCType
        {
            get
            {
                yield return new SelectListItem { Value = ((int)TrainingDayCell.SBCTypeEnum.S).ToString(), Text = "Squat", Selected = SBCType == (int)TrainingDayCell.SBCTypeEnum.S };
                yield return new SelectListItem { Value = ((int)TrainingDayCell.SBCTypeEnum.B).ToString(), Text = "Bench", Selected = SBCType == (int)TrainingDayCell.SBCTypeEnum.B };
                yield return new SelectListItem { Value = ((int)TrainingDayCell.SBCTypeEnum.C).ToString(), Text = "Clean", Selected = SBCType == (int)TrainingDayCell.SBCTypeEnum.C };
            }
        }

    }
}