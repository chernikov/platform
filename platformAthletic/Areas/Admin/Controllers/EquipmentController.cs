using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Models.ViewModels;
using platformAthletic.Model;


namespace platformAthletic.Areas.Admin.Controllers
{ 
    public class EquipmentController : AdminController
    {
		public ActionResult Index()
        {
			var list = Repository.Equipments.ToList();
			return View(list);
		}

		public ActionResult Create() 
		{
			var equipmentView = new EquipmentView();
			return View("Edit", equipmentView);
		}

		[HttpGet]
		public ActionResult Edit(int id) 
		{
			var  equipment = Repository.Equipments.FirstOrDefault(p => p.ID == id); 

			if (equipment != null) {
				var equipmentView = (EquipmentView)ModelMapper.Map(equipment, typeof(Equipment), typeof(EquipmentView));
				return View(equipmentView);
			}
			return RedirectToNotFoundPage;
		}

		[HttpPost]
		public ActionResult Edit(EquipmentView equipmentView)
        {
            if (ModelState.IsValid)
            {
                var equipment = (Equipment)ModelMapper.Map(equipmentView, typeof(EquipmentView), typeof(Equipment));

                equipment.ImagePath = equipment.ImagePath ?? string.Empty;

                if (equipment.ID == 0)
                {
                    Repository.CreateEquipment(equipment);
                }
                else
                {
                    Repository.UpdateEquipment(equipment);
                }
                return RedirectToAction("Index");
            }
            return View(equipmentView);
        }

        public ActionResult Delete(int id)
        {
            var equipment = Repository.Equipments.FirstOrDefault(p => p.ID == id);
            if (equipment != null)
            {
                    Repository.RemoveEquipment(equipment.ID);
            }
			return RedirectToAction("Index");
        }
	}
}