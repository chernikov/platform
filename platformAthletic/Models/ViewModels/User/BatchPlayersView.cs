using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace platformAthletic.Models.ViewModels.User
{
    public class BatchPlayersView
    {
        public Dictionary<string, PlayerView> Players { get; set; }


        public void Init()
        {
            Players.Add(Guid.NewGuid().ToString("N"), new PlayerView());
        }
        public BatchPlayersView()
        {
            Players = new Dictionary<string, PlayerView>();
        }
    }
}