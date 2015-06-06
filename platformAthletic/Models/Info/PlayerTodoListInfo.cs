using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace platformAthletic.Models.Info
{
    public class PlayerTodoListInfo : TodoListInfo
    {


        public PlayerTodoListInfo(int todo)
        {
            var todoEnum = (Model.User.TodoEnum)todo;

          
            List.Add(new TodoItem()
            {
                Solved = (todoEnum & Model.User.TodoEnum.ViewWorkOut) == Model.User.TodoEnum.ViewWorkOut,
                Name = "View a Workout",
                Url = "/my-page#todo-2"
            });
            List.Add(new TodoItem()
            {
                Solved = (todoEnum & Model.User.TodoEnum.Leaderboard) == Model.User.TodoEnum.Leaderboard,
                Name = "Visit another athletes profile",
                Url = "/leaderboard#todo-20"
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