
using platformAthletic.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace platformAthletic.Models.Info
{
    public class CoachTodoListInfo : TodoListInfo
    {
    
        public CoachTodoListInfo(int todo) : base()
        {
            var todoEnum = (Model.User.TodoEnum)todo;
            var rand = StringExtension.CreateRandomPassword(4, allowedChars : "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789");
            List.Add(new TodoItem()
            {
                Solved = (todoEnum & Model.User.TodoEnum.AddEquipment) == Model.User.TodoEnum.AddEquipment,
                Name = "Add your Equipment",
                Url = "/equipment?ch=" + rand + "#todo-1"
            });
            List.Add(new TodoItem()
            {
                Solved = (todoEnum & Model.User.TodoEnum.AddPlayers) == Model.User.TodoEnum.AddPlayers,
                Name = "Add Player(s)",
                Url = "/dashboard?ch=" + rand + "#todo-8"
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
                Url = "/dashboard?ch=" + rand + "#todo-16"
            });
            List.Add(new TodoItem()
            {
                Solved = (todoEnum & Model.User.TodoEnum.ViewWorkOut) == Model.User.TodoEnum.ViewWorkOut,
                Name = "View a Workout",
                Url = "/dashboard?ch=" + rand + "#todo-2"
            });
        }
    }
}