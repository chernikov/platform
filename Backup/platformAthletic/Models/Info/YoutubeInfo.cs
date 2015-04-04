using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace platformAthletic.Models.Info
{
    public class YoutubeInfo
    {
        [Required(ErrorMessage = "Enter youtube url")]
        public string VideoUrl { get; set; }

        public string VideoCode { get; set; }
    }
}