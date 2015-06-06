using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace platformAthletic.Models.Info
{
    public class TodoListInfo
    {
        public class TodoItem
        {
            public bool Solved { get; set; }

            public string Url { get; set; }

            public string Name { get; set; }
        }

        public List<TodoItem> List { get; set; }
        public TodoListInfo()
        {
            List = new List<TodoItem>();
        }
    }
}