using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace platformAthletic.Model
{
	
 public partial class SqlRepository
    {
        public IQueryable<Cell> Cells
        {
            get
            {
                return Db.Cells;
            }
        }

        public bool CreateCell(Cell instance)
        {
            if (instance.ID == 0)
            {
                Db.Cells.InsertOnSubmit(instance);
                Db.Cells.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool UpdateCell(Cell instance)
        {
            var cache = Db.Cells.FirstOrDefault(p => p.ID == instance.ID);
            if (cache != null)
            {
                Db.Cells.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool RemoveCell(int idCell)
        {
            Cell instance = Db.Cells.FirstOrDefault(p => p.ID == idCell);
            if (instance != null)
            {
                Db.Cells.DeleteOnSubmit(instance);
                Db.Cells.Context.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}