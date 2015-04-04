using platformAthletic.Model;
using platformAthletic.Models.Info;
using platformAthletic.Models.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace platformAthletic.Areas.Cms.Controllers
{

    [Authorize(Roles = "admin")]
    public class HomeController : CmsController
    {
        //
        // GET: /Cms/Home/

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult JoinUs()
        {
            var priceInfoView = new PriceInfoView()
            {
                TeamPrice = Repository.Settings.First(p => p.Name == "TeamPrice").ValueDouble,
                IndividualPrice = Repository.Settings.First(p => p.Name == "IndividualPrice").ValueDouble,
            };
            return View(priceInfoView);
        }


        public ActionResult SlideShow()
        {
            var list = Repository.Galleries.ToList();
            return View(list);
        }

        public ActionResult WhatPeopleSaying()
        {
            var list = Repository.PeopleSayings.ToList();
            return View(list);
        }

        public ActionResult Menu()
        {
            return View();
        }

        public ActionResult IndividualRegister()
        {
            var registerUserView = new RegisterIndividualView
            {
                RegisterType = RegisterUserView.RegisterTypeEnum.Individual,
            };
            return View(registerUserView);
        }

        public ActionResult TeamRegister()
        {
            var registerUserView = new RegisterTeamView
            {
                RegisterType = RegisterTeamView.RegisterTypeEnum.Coach,
            };
            return View(registerUserView);
        }

        public ActionResult WhatWeDo()
        {
            return View();
        }

        public ActionResult WhoWeAre()
        {
            return View();
        }

        public ActionResult PostRegister()
        {
            return View(CurrentUser);
        }
        public ActionResult ContactUs()
        {
            return View();
        }

        public ActionResult NewUserLogin()
        {
            return View(CurrentUser);
        }

        [ValidateInput(false)]
        public ActionResult Save(FormCollection form)
        {
            foreach (string key in form.Keys)
            {
                var value = form[key];

                Repository.SavePagePart(new PagePart()
                {
                    Name = key.ToLower(),
                    Text = value
                });
            }

            return RedirectBack("~/");
        }
    }
}
