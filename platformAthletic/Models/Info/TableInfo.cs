using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Model;
using platformAthletic.Tools;

namespace platformAthletic.Models.Info
{
    public class TableInfo
    {
        public List<TrainingDay> Days { get; set; }

        public List<int> Equipments { get; set; }

        public User User { get; set; }

        public TrainingDay CurrentDay { get; set; }

        public MvcHtmlString GetCellInfo(int id)
        {
            var cell = CurrentDay.TrainingDayCells.FirstOrDefault(p => p.CellID == id);
            if (cell != null)
            {
                return new MvcHtmlString(CellValue(cell));
            }
            return new MvcHtmlString("&nbsp;");
        }


        private string CellValue(TrainingDayCell trainingDayCell)
        {
            if (!string.IsNullOrWhiteSpace(trainingDayCell.PrimaryText))
            {
                return trainingDayCell.PrimaryText;
            }

            switch (trainingDayCell.Cell.Type)
            {
                case (int)Cell.CellType.Info:
                    return trainingDayCell.PrimaryText ?? string.Empty;
                case (int)Cell.CellType.Exercise:
                    var training = GetTrainingEquipmentByEqipment(trainingDayCell.TrainingSet);
                    if (training != null)
                    {
                        return training.Name;
                    }
                    return string.Empty;
                case (int)Cell.CellType.Value:
                    switch (trainingDayCell.SBCType)
                    {
                        case (int)TrainingDayCell.SBCTypeEnum.S :
                            return (trainingDayCell.Coefficient.Value * User.Squat).Round5().ToString("0");
                        case (int)TrainingDayCell.SBCTypeEnum.B:
                            return (trainingDayCell.Coefficient.Value * User.Bench).Round5().ToString("0");
                        case (int)TrainingDayCell.SBCTypeEnum.C:
                            return (trainingDayCell.Coefficient.Value * User.Clean).Round5().ToString("0");
                    }
                    return string.Empty;
            }

            return string.Empty;
        }

        private Training GetTrainingEquipmentByEqipment(TrainingSet trainingSet)
        {
            var equipmentsList = trainingSet.TrainingEquipments.OrderByDescending(p => p.Priority);

            foreach (var equipments in equipmentsList)
            {
                
                if ((!equipments.EquipmentID.HasValue 
                    || (equipments.EquipmentID.HasValue && Equipments.Contains(equipments.EquipmentID.Value)))
                    &&
                    (!equipments.Equipment2ID.HasValue
                    || (equipments.Equipment2ID.HasValue && Equipments.Contains(equipments.Equipment2ID.Value)))
                )
                {
                    return equipments.Training;
                }
            }
            return null;
        }
    }
}