using System.Linq;
using System.Web.Mvc;
using platformAthletic.Models.ViewModels;
using platformAthletic.Models.ViewModels.User;
using platformAthletic.Tools.Mail;

namespace platformAthletic.Areas.Default.Controllers
{
    public class LoginController : DefaultController
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public ActionResult Index(LoginViewModel loginView)
        {
            if (ModelState.IsValid)
            {
                var user = Auth.Login(loginView.Email, loginView.Password, loginView.IsPersistent);
                
                if (user != null)
                {
                    if (!user.ActivatedDate.HasValue)
                    {
                        Repository.ActivateUser(user);
                    }
                    if (user.InRoles("admin"))
                    {
                        return RedirectToAction("Index", "Home", new { area = "Admin" });
                    }
                    if (user.Mode == (int)platformAthletic.Model.User.ModeEnum.Tutorial) {
                        if (user.InRoles("coach,assistant"))
                        {
                            return RedirectToAction("Index", "Dashboard");
                        }
                        if (user.InRoles("player,individual"))
                        {
                            return RedirectToAction("MyPage", "User");
                        }
                    }
                    
                    return RedirectToAction("Index", "Home");
                }
                ModelState["Password"].Errors.Add(new ModelError("Wrong user id/password"));
            }
            return View(loginView);
        }

        [HttpPost]
        public ActionResult IsActivated(LoginViewModel loginView)
        {
            if (ModelState.IsValid)
            {
                var user = Repository.Login(loginView.Email, loginView.Password);
                if (user != null)
                {
                    if (user.ActivatedDate.HasValue)
                    {
                        return Json(new { result = "ok" });
                    }
                    else
                    {
                        return Json(new { result = "error" });
                    }
                }
            }
            return Json(new { result = "ok" });
        }

        public ActionResult Logout()
        {
            Auth.LogOut();
            if (!Request.IsLocal)
            {
                return Redirect("http://plt4m.com");
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View(new ForgotPasswordView());
        }

        [HttpPost]
        public ActionResult ForgotPassword(ForgotPasswordView forgotPasswordView)
        {
            if (ModelState.IsValid)
            {
                var user =
                    Repository.Users.FirstOrDefault(p => string.Compare(p.Email, forgotPasswordView.Email, true) == 0);
                if (user != null)
                {
                    NotifyMail.SendNotify(Config, "ForgotPassword", user.Email,
                                                (u, format) => string.Format(format, HostName),
                                                (u, format) => string.Format(format, u.Email, u.Password, HostName),
                                                user);
                    return View("ForgotPasswordSuccess");
                }
                ModelState.AddModelError("Email", "User with this email is not found");
            }
            return View(forgotPasswordView);
        }
    }

}
