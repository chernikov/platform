using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ManageAttribute;
using platformAthletic.Model;


namespace platformAthletic.Models.ViewModels
{ 
	public class PageView
    {
        [PrimaryField]
        [HiddenField]
        public int ID { get; set; }

        [ShowIndex]
        [TextBoxField]
		public string Name {get; set; }

        [HtmlTextField]
		public string Text {get; set; }

    }
}