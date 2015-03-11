using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ManageAttribute;
using platformAthletic.Model;
using System.ComponentModel.DataAnnotations;


namespace platformAthletic.Models.ViewModels
{
    public class AphorismView
    {
        public int ID { get; set; }

        public string Author { get; set; }



        public string Text { get; set; }
    }
}