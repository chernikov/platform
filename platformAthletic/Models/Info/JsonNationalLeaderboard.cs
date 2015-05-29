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
            var users = Repository.PlayersTeamPlayersUsers;
            if (!string.IsNullOrWhiteSpace(Search.SearchString))
            {
                var lowerString = Search.SearchString.ToLower();
                users = users.Where(p => p.FirstName.ToLower().Contains(lowerString) || p.LastName.ToLower().Contains(lowerString));
            }
            Filter(ref users);
            var orderUsers = Order(users);
            FillRecords(orderUsers);
        }

        protected override void GetPage()
        {
        }
    }
}