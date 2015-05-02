using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace platformAthletic.Model
{
    public static class SqlSingleton
    {
        public static SqlRepository sqlRepository { get; set; }
    }
}
