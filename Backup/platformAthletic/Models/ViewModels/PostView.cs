using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ManageAttribute;
using platformAthletic.Model;


namespace platformAthletic.Models.ViewModels
{ 
	public class PostView
    {
        [PrimaryField]
        [HiddenField]
        public int ID { get; set; }

        [ShowIndex]
        [HiddenField]
		public int UserID {get; set; }

        [TextBoxField]
        [Required]
        [ShowIndex]
		public string Header {get; set; }

        [HtmlTextField]
        [Required]
		public string Text {get; set; }

    }
}