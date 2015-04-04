using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Model;
using platformAthletic.Models.ViewModels;
using platformAthletic.Tools;
using platformAthletic.Attributes;

namespace platformAthletic.Areas.Default.Controllers
{
    public class PlayerController : DefaultController
    {
        [SeasonCheck]
        public ActionResult Index()
        {
            if (CurrentUser.InRoles("player,individual"))
            {
                return View(CurrentUser);
            }
            return RedirectToLoginPage;
        }

        [SeasonCheck]
        public ActionResult Item(int id)
        {
            var user = Repository.Users.FirstOrDefault(p => p.ID == id);
            if (user != null)
            {
                if (CurrentUser.InRoles("admin"))
                {
                    return View("Index", user);
                }
                if (CurrentUser.OwnTeam != null)
                {
                    if (CurrentUser.OwnTeam.ID == user.Team.ID)
                    {
                        return View("Index", user);
                    }
                    else
                    {
                        return RedirectToLoginPage;
                    }
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            return RedirectToNotFoundPage;
        }

        public ActionResult PillarsData(int id)
        {
            var list = new List<UserPillar>();

            var user = Repository.Users.FirstOrDefault(p => p.ID == id);
            if (user != null)
            {
                foreach (var pillarType in Repository.PillarTypes.ToList())
                {
                    UserPillar userPillar = null;
                    userPillar = user.UserPillars.Where(p => p.PillarTypeID == pillarType.ID).OrderByDescending(p => p.ID).FirstOrDefault();
                    if (userPillar == null)
                    {
                        userPillar = new UserPillar
                        {
                            PillarType = pillarType
                        };
                    }
                    list.Add(userPillar);
                }
                return View(list);
            }
            return null;
        }

        [HttpGet]
        public ActionResult Pillar()
        {
            if (CurrentUser.InRoles("player,individual"))
            {
                var userPillarView = new UserPillarView()
                {
                    UserID = CurrentUser.ID
                };
                return View(userPillarView);
            }
            return null;
        }

        [HttpPost]
        public ActionResult Pillar(UserPillarView userPillarView)
        {
            var pillarType = Repository.PillarTypes.FirstOrDefault(p => p.ID == userPillarView.PillarTypeID);

            if (pillarType != null)
            {
                switch (pillarType.Type)
                {
                    case (int)PillarTypeView.TypeEnum.Reps:
                        int number;
                        if (Int32.TryParse(userPillarView.TextValue, out number))
                        {
                            userPillarView.Value = number;
                        }
                        else
                        {
                            /* ModelState.AddModelError("TextValue", "Can't parse value to numbers");*/
                        }
                        break;
                    case (int)PillarTypeView.TypeEnum.Time:
                        TimeSpan timeSpan;
                        if (TimeSpan.TryParse(userPillarView.TextValue, out timeSpan))
                        {
                            userPillarView.Value = (int)timeSpan.TotalSeconds;
                        }
                        else
                        {
                            /* ModelState.AddModelError("TextValue", "Can't parse value to time");*/
                        }
                        break;
                }
                if (ModelState.IsValid)
                {
                    var userPillar = (UserPillar)ModelMapper.Map(userPillarView, typeof(UserPillarView), typeof(UserPillar));
                    userPillar.UserID = CurrentUser.ID;
                    Repository.CreateUserPillar(userPillar);
                    return View("_OK");
                }
                return View(userPillarView);
            }
            return null;
        }

        public ActionResult GetTextAbove(int id)
        {
            var pillarType = Repository.PillarTypes.FirstOrDefault(p => p.ID == id);
            if (pillarType != null)
            {
                return Json(new { pillarType.TextAbove, pillarType.Placeholder }, JsonRequestBehavior.AllowGet);
            }
            return Content("");
        }

        public ActionResult SelectPosition(int id)
        {
            var selectList = new List<SelectListItem>();
            var user = Repository.Users.FirstOrDefault(p => p.ID == id);
            if (user != null)
            {
                foreach (var item in Repository.FieldPositions.ToList())
                {
                    selectList.Add(new SelectListItem()
                    {
                        Value = item.ID.ToString(),
                        Text = item.Name,
                        Selected = user.FieldPositions.Any(p => p.ID == item.ID)
                    });
                }
                return View(selectList);
            }
            return null;
        }

        public ActionResult SetField(int id, User.FieldType field, string value)
        {
            var user = Repository.Users.FirstOrDefault(p => p.ID == id);
            if (user != null)
            {
                Repository.SetUserField(id, field, value);
                return Json(new { result = "ok" });
            }
            return Json(new { result = "error" });
        }

        public ActionResult SelectFieldPosition(int id, int fieldPosition)
        {
            var user = Repository.Users.FirstOrDefault(p => p.ID == id);
            if (user != null)
            {
                Repository.SetFieldPosition(id, fieldPosition);
                return Json(new { result = "ok" });
            }
            return Json(new { result = "error" });
        }


    }
}
