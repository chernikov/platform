using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace platformAthletic.Models.Info
{
    public class CoachTodoListInfo
    {
        public class TodoItem
        {
            public bool Solved { get; set; }

            public string Url { get; set; }

            public string Name { get; set; }
        }

        public List<TodoItem> List { get; set; }
        public CoachTodoListInfo(int todo)
        {
            var todoEnum = (Model.User.TodoEnum)todo;

            List = new List<TodoItem>();

            List.Add(new TodoItem()
            {
                Solved = (todoEnum & Model.User.TodoEnum.AddEquipment) == Model.User.TodoEnum.AddEquipment,
                Name = "Add your Equipment",
                Url = "/equipment#todo-1"
            });
            List.Add(new TodoItem()
            {
                Solved = (todoEnum & Model.User.TodoEnum.AddPlayers) == Model.User.TodoEnum.AddPlayers,
                Name = "Add Player(s)",
                Url = "/dashboard#todo-8"
            });
            List.Add(new TodoItem()
            {
                Solved = (todoEnum & Model.User.TodoEnum.ConfirmTrainingStartDate) == Model.User.TodoEnum.ConfirmTrainingStartDate,
                Name = "Confirm Training Start Date",
                Url = "/scheduling#todo-4"
            });
            List.Add(new TodoItem()
            {
                Solved = (todoEnum & Model.User.TodoEnum.EnterMaxes) == Model.User.TodoEnum.EnterMaxes,
                Name = "Enter Maxes",
                Url = "/dashboard#todo-16"
            });
            List.Add(new TodoItem()
            {
                Solved = (todoEnum & Model.User.TodoEnum.ViewWorkOut) == Model.User.TodoEnum.ViewWorkOut,
                Name = "View a Workout",
                Url = "/dashboard#todo-2"
            });
        }
    }
}