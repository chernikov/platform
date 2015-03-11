using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace platformAthletic.Model
{ 
    public partial class TrainingDayCell
    {
        public enum SBCTypeEnum
        {
            S = 0x01,
            B = 0x02,
            C = 0x03
        };

        public string Value
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(PrimaryText))
                {
                    return PrimaryText;
                }

                switch (Cell.Type)
                {
                    case (int)Cell.CellType.Info :
                        return PrimaryText ?? string.Empty;
                    case (int)Cell.CellType.Exercise :
                        var TrainingEquipments = TrainingSet.TrainingEquipments.OrderBy(p => p.Priority).FirstOrDefault();
                        if (TrainingEquipments != null) 
                        {
                            return TrainingEquipments.Training.Name;
                        }
                        return string.Empty;
                    case (int)Cell.CellType.Value :
                        return string.Format("{0} x {1}", Coefficient, (SBCTypeEnum)SBCType);
                }

                return string.Empty;
            }
        }
	}
}