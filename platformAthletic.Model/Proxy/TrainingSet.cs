using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace platformAthletic.Model
{ 
    public partial class TrainingSet
    {
        public string Names
        {
            get
            {
                return string.Join(",", TrainingEquipments.OrderByDescending(p => p.Priority).Select(p => p.Training.Name + " (" + (p.EquipmentID.HasValue ? p.Equipment.Name : "N/A") + (p.Equipment2ID.HasValue ? ", " + p.Equipment1.Name : "")  + ")"));
            }
        }
	}
}