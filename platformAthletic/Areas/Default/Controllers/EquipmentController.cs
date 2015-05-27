using platformAthletic.Model;
using platformAthletic.Models.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace platformAthletic.Areas.Default.Controllers
{
    public class EquipmentController : DefaultController
    {
        [HttpGet]
        [Authorize(Roles = "individual,coach")]
        public ActionResult Index()
        {
            var equipmentList = Repository.Equipments.OrderBy(p => p.Name).ToList();
            var selectedEquipmentList = new SelectedEquipmentList(equipmentList, CurrentUser);
            return View(selectedEquipmentList);
        }

        [HttpPost]
        [Authorize(Roles = "individual,coach")]
        public ActionResult Index(SelectedEquipmentList selectedEquipmentList)
        {
            ViewBag.Message = "Saved";
            foreach (var item in selectedEquipmentList.List)
            {
                if (item.Select && !CurrentUser.UserEquipments.Any(p => p.EquipmentID == item.Equipment.ID))
                {
                    var userEquipment = new UserEquipment
                    {
                        EquipmentID = item.Equipment.ID,
                        UserID = CurrentUser.ID
                    };
                    Repository.CreateUserEquipment(userEquipment);
                }
                if (!item.Select && CurrentUser.UserEquipments.Any(p => p.EquipmentID == item.Equipment.ID))
                {
                    var userEquipment = CurrentUser.UserEquipments.First(p => p.EquipmentID == item.Equipment.ID);
                    Repository.RemoveUserEquipment(userEquipment.ID);
                }
            }
            return View(selectedEquipmentList);
        }
    }
}
