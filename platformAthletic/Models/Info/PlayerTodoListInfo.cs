using platformAthletic.Tools;
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
            var rand = StringExtension.CreateRandomPassword(4, allowedChars: "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789");
            List.Add(new TodoItem()
            {
                Solved = (todoEnum & Model.User.TodoEnum.ViewWorkOut) == Model.User.TodoEnum.ViewWorkOut,
                Name = "View a Workout",
                Url = "/my-page?ch=" + rand + "#todo-2"
            });
            List.Add(new TodoItem()
            {
                Solved = (todoEnum & Model.User.TodoEnum.Leaderboard) == Model.User.TodoEnum.Leaderboard,
                Name = "Visit another athletes profile",
                Url = "/leaderboard?ch=" + rand + "#todo-20"
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