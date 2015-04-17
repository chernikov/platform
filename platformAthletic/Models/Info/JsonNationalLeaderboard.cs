using platformAthletic.Model;
using StackExchange.Profiling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace platformAthletic.Models.Info
{
    public class JsonNationalLeaderboard : NationalLeaderboard
    {
        public JsonNationalLeaderboard(SearchNationalLeaderboard search) : base(search)
        {
            
        }

        protected override void Process()
        {
            var users = Repository.Users;
            Filter(ref users);
            var orderUsers = Order(users);
            FillRecords(orderUsers);
        }

        protected override void GetPage()
        {
        }
    }
}