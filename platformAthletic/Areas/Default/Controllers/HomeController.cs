﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Controllers;
using platformAthletic.Model;
using platformAthletic.Models.Info;
using platformAthletic.Models.ViewModels;

namespace platformAthletic.Areas.Default.Controllers
{
    public class HomeController : DefaultController
    {
        #region New Design
        public ActionResult Index(int page = 1)
        {
            if (CurrentUser != null)
            {
                if (!CurrentUser.InRoles("payed"))
                {
                    return Redirect("~/billing");
                }
                if (CurrentUser.InRoles("coach,individual") && CurrentUser.CurrentSeason == null)
                {
                    return Redirect("/register-success");
                }
                if (CurrentUser.InRoles("coach,player"))
                {
                    var team = CurrentUser.Team ?? CurrentUser.OwnTeam;
                    if (team != null) 
                    {
                        var coach = team.User;
                        var admins = Repository.Users.Where(p => p.UserRoles.Any(r => string.Compare(r.Role.Code, "admin", true) == 0)).Select(p => p.ID);

                        var list = Repository.Posts.Where(p => p.UserID == coach.ID || admins.Contains(p.UserID)).OrderByDescending(p => p.ID);
                        var data = new PageableData<Post>();
                        data.Init(list, page, "Index");
                        return View("Team", data);
                    }
                }
                if (CurrentUser.InRoles("individual"))
                {
                    var admins = Repository.Users.Where(p => p.UserRoles.Any(r => string.Compare(r.Role.Code, "admin", true) == 0)).Select(p => p.ID);

                    var list = Repository.Posts.Where(p => admins.Contains(p.UserID)).OrderByDescending(p => p.ID);
                    var data = new PageableData<Post>();
                    data.Init(list, page, "Index");
                    return View("Individual", data);
                }
            }
            return View();
        }

        public ActionResult WhoWeAre()
        {
            return View();
        }

        public ActionResult WhatWeDo()
        {
            var youtubeInfoView = new YoutubeInfo()
            {
                VideoUrl = Repository.Settings.First(p => p.Name == "VideoUrl").Value,
                VideoCode = Repository.Settings.First(p => p.Name == "VideoCode").Value,
            };
            return View(youtubeInfoView);
        }

        public ActionResult Contact()
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

        public ActionResult TeamRegister()
        {
            return View();
        }

        public ActionResult IndividualRegister()
        {
            return View();
        }

        public ActionResult NewUserLogin()
        {
            return View(CurrentUser);
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
        #endregion 

        public ActionResult Online()
        {
            if (CurrentUser != null)
            {
                Repository.OnlineUser(CurrentUser.ID);
            }
            return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Carousel()
        {
            var list = Repository.Galleries.ToList();

            return View(list);
        }

        public ActionResult UserColors()
        {
            return View(CurrentUser);
        }

        public ActionResult UserLogin()
        {
            return View(CurrentUser);
        }

        public ActionResult Join()
        {
            var priceInfoView = new PriceInfoView()
            {
                TeamPrice = Repository.Settings.First(p => p.Name == "TeamPrice").ValueDouble,
                IndividualPrice = Repository.Settings.First(p => p.Name == "IndividualPrice").ValueDouble,
                TeamPriceImagePath = Repository.Settings.First(p => p.Name == "TeamPriceImagePath").Value,
                IndividualPriceImagePath = Repository.Settings.First(p => p.Name == "IndividualPriceImagePath").Value,
            };
            return View(priceInfoView);
        }

        public ActionResult Abouts()
        {
            var list = Repository.Abouts.ToList();
            return View(list);
        }

        public ActionResult Top5User()
        {
            var team = CurrentUser.Team ?? CurrentUser.OwnTeam;

            if (team != null)
            {
                var list = team.Users.ToList().OrderByDescending(p => p.SbcSum).Take(5);
                return View(list);
            }
            return null;
        }

        [HttpGet]
        public ActionResult AddPost()
        {
            var postView = new PostView()
            {
                UserID = CurrentUser.ID
            };
            return View("EditPost", postView);
        }

        [HttpGet]
        public ActionResult EditPost(int id)
        {
            var post = Repository.Posts.FirstOrDefault(p => p.ID == id);

            if (post != null && post.CanEdit(CurrentUser))
            {
                var postView = (PostView)ModelMapper.Map(post, typeof(Post), typeof(PostView));
                return View(postView);
            }
            return RedirectToLoginPage;
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditPost(PostView postView)
        {
            if (ModelState.IsValid)
            {
                var post = (Post)ModelMapper.Map(postView, typeof(PostView), typeof(Post));
                post.UserID = CurrentUser.ID;
                if (post.ID == 0)
                {
                    Repository.CreatePost(post);
                }
                else
                {
                    Repository.UpdatePost(post);
                }
                return RedirectToAction("Index");
            }
            return View(postView);
        }

        public ActionResult DeletePost(int id)
        {
            var post = Repository.Posts.FirstOrDefault(p => p.ID == id);

            if (post != null && post.CanEdit(CurrentUser))
            {
                Repository.RemovePost(post.ID);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Test()
        {
            throw new Exception();
            return null;
        }

    }
}
