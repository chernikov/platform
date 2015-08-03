using platformAthletic.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace platformAthletic.Models.Info
{
    public class IndividualTodoListInfo : TodoListInfo
    {


        public IndividualTodoListInfo(int todo)
        {
            var todoEnum = (Model.User.TodoEnum)todo;
            var rand = StringExtension.CreateRandomPassword(4, allowedChars: "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789");
            List.Add(new TodoItem()
            {
                Solved = (todoEnum & Model.User.TodoEnum.AddEquipment) == Model.User.TodoEnum.AddEquipment,
                Name = "Add your Equipment",
                Url = "/equipment?ch=" + rand + "#todo-1"
            });
            List.Add(new TodoItem()
            {
                Solved = (todoEnum & Model.User.TodoEnum.ViewWorkOut) == Model.User.TodoEnum.ViewWorkOut,
                Name = "View a Workout",
                Url = "/my-page?ch=" + rand + "#todo-2"
            });
            List.Add(new TodoItem()
            {
                Solved = (todoEnum & Model.User.TodoEnum.ConfirmTrainingStartDate) == Model.User.TodoEnum.ConfirmTrainingStartDate,
                Name = "Confirm Training Start Date",
                Url = "/scheduling?ch=" + rand + "#todo-4"
            });
            List.Add(new TodoItem()
            {
                Solved = (todoEnum & Model.User.TodoEnum.EnterMaxes) == Model.User.TodoEnum.EnterMaxes,
                Name = "Enter Maxes",
                Url = "/my-page?ch=" + rand + "#todo-16"
            });
            List.Add(new TodoItem()
            {
                Solved = (todoEnum & Model.User.TodoEnum.UploadVideo) == Model.User.TodoEnum.UploadVideo,
                Name = "Upload a video",
                Url = "/my-page?ch=" + rand + "#todo-32"
            });
        }
    }
}