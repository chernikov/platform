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
            for (int i = 0; i < 5; i++)
            {
                Players.Add(Guid.NewGuid().ToString("N"), new PlayerView());
                break;
            }
        }
        public BatchPlayersView()
        {
            Players = new Dictionary<string, PlayerView>();
        }
    }
}