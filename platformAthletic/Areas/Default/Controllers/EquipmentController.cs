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
        [Authorize(Roles = "individual,coach,assistant")]
        public ActionResult Index()
        {
            var equipmentList = Repository.Equipments.OrderBy(p => p.Name).ToList();
            var user = CurrentUser;
            if (CurrentUser.InRoles("assistant"))
            {
                user = CurrentUser.TeamOfAssistance.User;
            }
            var selectedEquipmentList = new SelectedEquipmentList(equipmentList, user);
            return View(selectedEquipmentList);
        }

        [HttpPost]
        [Authorize(Roles = "individual,coach,assistant")]
        public ActionResult Index(SelectedEquipmentList selectedEquipmentList)
        {
            if (selectedEquipmentList.List.Any(p => p.Select) && CurrentUser.Mode == (int)Model.User.ModeEnum.Todo)
            {
                Repository.SetTodo(CurrentUser.ID, Model.User.TodoEnum.AddEquipment);
            }
            ViewBag.Message = "Saved";
            var user = CurrentUser;
            if (CurrentUser.InRoles("assistant"))
            {
                user = CurrentUser.TeamOfAssistance.User;
            }

            foreach (var item in selectedEquipmentList.List)
            {
                if (item.Select && !user.UserEquipments.Any(p => p.EquipmentID == item.Equipment.ID))
                {
                    var userEquipment = new UserEquipment
                    {
                        EquipmentID = item.Equipment.ID,
                        UserID = user.ID
                    };
                    Repository.CreateUserEquipment(userEquipment);
                }
                if (!item.Select && user.UserEquipments.Any(p => p.EquipmentID == item.Equipment.ID))
                {
                    var userEquipment = user.UserEquipments.First(p => p.EquipmentID == item.Equipment.ID);
                    Repository.RemoveUserEquipment(userEquipment.ID);
                }
            }
            return View(selectedEquipmentList);
        }
    }
}
