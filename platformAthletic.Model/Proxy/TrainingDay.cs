using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace platformAthletic.Model
{ 
    public partial class TrainingDay
    {
        public int CountExercise
        {
            get
            {
                return TrainingDayCells.Count(p => p.Cell.Type == (int)Cell.CellType.Exercise);
            }
        }
	}
}