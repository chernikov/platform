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

            List.Add(new TodoItem()
            {
                Solved = (todoEnum & Model.User.TodoEnum.AddEquipment) == Model.User.TodoEnum.AddEquipment,
                Name = "Add your Equipment",
                Url = "/equipment#todo-1"
            });
            List.Add(new TodoItem()
            {
                Solved = (todoEnum & Model.User.TodoEnum.ViewWorkOut) == Model.User.TodoEnum.ViewWorkOut,
                Name = "View a Workout",
                Url = "/my-page#todo-2"
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
                Url = "/my-page#todo-16"
            });
            List.Add(new TodoItem()
            {
                Solved = (todoEnum & Model.User.TodoEnum.UploadVideo) == Model.User.TodoEnum.UploadVideo,
                Name = "Upload a video",
                Url = "/my-page#todo-32"
            });
        }
    }
}