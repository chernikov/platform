using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using platformAthletic.Model;

namespace platformAthletic.Models.Info
{
    public class SelectedEquipmentList
    {
        public class SelectEquipment
        {
            public Equipment  Equipment { get; set; }

            public bool Select { get; set; }
        }

        public List<SelectEquipment> List { get; set; }

        public SelectedEquipmentList()
        {

        }

        public SelectedEquipmentList(IEnumerable<Equipment> list, User user)
        {
            List = new List<SelectEquipment>();
            foreach (var item in list)
            {
                var newSelectEquipment = new SelectEquipment()
                {
                    Equipment = item,
                    Select = user.UserEquipments.Any(p => p.EquipmentID == item.ID)
                };
                List.Add(newSelectEquipment);
            }
        }
    }

}