using System.Web.Mvc;

namespace platformAthletic.Areas.Default
{
    public class DefaultAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Default";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "error",
                "error",
                new { controller = "Error", action = "Index" },
                new[] { "platformAthletic.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "who-we-are",
                new { controller = "Home", action = "WhoWeAre" },
                new[] { "platformAthletic.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "what-we-do",
                new { controller = "Home", action = "WhatWeDo" },
                new[] { "platformAthletic.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "contact-us",
                new { controller = "Contact", action = "Index" },
                new[] { "platformAthletic.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "join-us",
                new { controller = "Home", action = "JoinUs" },
                new[] { "platformAthletic.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "team-registration",
                new { controller = "Account", action = "RegisterTeam" },
                new[] { "platformAthletic.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "individual-registration",
                new { controller = "Account", action = "RegisterIndividual" },
                new[] { "platformAthletic.Areas.Default.Controllers" }
            );
            context.MapRoute(
                null,
                "join",
                new { controller = "Home", action = "Join" },
                new[] { "platformAthletic.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "register",
                new { controller = "Account", action = "Register" },
                new[] { "platformAthletic.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "register-success",
                new { controller = "Account", action = "RegisterSuccess" },
                new[] { "platformAthletic.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "thanks",
                new { controller = "Account", action = "Thanks" },
                new[] { "platformAthletic.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "set-colors",
                new { controller = "Account", action = "SetColors" },
                new[] { "platformAthletic.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "select-season",
                new { controller = "Account", action = "SelectSeason" },
                new[] { "platformAthletic.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "logout",
                new { controller = "Login", action = "Logout" },
                new[] { "platformAthletic.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "forgot-password",
                new { controller = "Login", action = "ForgotPassword" },
                new[] { "platformAthletic.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "is-activated",
                new { controller = "Login", action = "IsActivated" },
                new[] { "platformAthletic.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "account",
                new { controller = "Account", action = "Index" },
                new[] { "platformAthletic.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "settings",
                new { controller = "Account", action = "Settings" },
                new[] { "platformAthletic.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "ajax-settings",
                new { controller = "Account", action = "AjaxSettings" },
                new[] { "platformAthletic.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "scheduling",
                new { controller = "Schedule", action = "Index" },
                new[] { "platformAthletic.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "create-group",
                new { controller = "Schedule", action = "CreateGroup" },
                new[] { "platformAthletic.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "edit-group",
                new { controller = "Schedule", action = "EditGroup" },
                new[] { "platformAthletic.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "remove-group",
                new { controller = "Schedule", action = "RemoveGroup" },
                new[] { "platformAthletic.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "assign-players",
                new { controller = "Schedule", action = "AssignPlayers" },
                new[] { "platformAthletic.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "change-password",
                new { controller = "Account", action = "ChangePassword" },
                new[] { "platformAthletic.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "billing",
                new { controller = "Account", action = "Billing" },
                new[] { "platformAthletic.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "equipment",
                new { controller = "Account", action = "Equipment" },
                new[] { "platformAthletic.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "manage-players",
                new { controller = "ManagePlayer", action = "Index" },
                new[] { "platformAthletic.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "manage-editplayer",
                new { controller = "ManagePlayer", action = "EditPlayer" },
                new[] { "platformAthletic.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "resend-activation",
                new { controller = "ManagePlayer", action = "SendActivation" },
                new[] { "platformAthletic.Areas.Default.Controllers" }
            );


            context.MapRoute(
                null,
                "features",
                new { controller = "Feature", action = "Index" },
                new[] { "platformAthletic.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "features/{id}",
                new { controller = "Feature", action = "Index" },
                new[] { "platformAthletic.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "contact-us",
                new { controller = "Contact", action = "Index" },
                new[] { "platformAthletic.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "getting-started",
                new { controller = "GettingStarted", action = "Index" },
                new[] { "platformAthletic.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "faq",
                new { controller = "Faq", action = "Index" },
                new[] { "platformAthletic.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "about-us",
                new { controller = "AboutUs", action = "Index" },
                new[] { "platformAthletic.Areas.Default.Controllers" }
            );
           
            context.MapRoute(
                null,
                "video",
                new { controller = "Video", action = "Index", SortType = 1 },
                new[] { "platformAthletic.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "video-pillar",
                new { controller = "Video", action = "Index", SortType = 2 },
                new[] { "platformAthletic.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "video-code",
                new { controller = "Video", action = "Video" },
                new[] { "platformAthletic.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "video-pillar-code",
                new { controller = "Video", action = "PillarVideo" },
                new[] { "platformAthletic.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "video-json",
                new { controller = "Video", action = "JsonVideos" },
                new[] { "platformAthletic.Areas.Default.Controllers" }
            );

            context.MapRoute(
               null,
               "video-json-pillars",
               new { controller = "Video", action = "JsonPillars" },
               new[] { "platformAthletic.Areas.Default.Controllers" }
           );

            context.MapRoute(
               null,
               "video/SetPreview",
               new { controller = "Video", action = "SetPreview" },
               new[] { "platformAthletic.Areas.Default.Controllers" }
            );

            context.MapRoute(
               null,
               "video/SetCode",
               new { controller = "Video", action = "SetCode" },
               new[] { "platformAthletic.Areas.Default.Controllers" }
            );
            context.MapRoute(
               null,
               "video/SetLorem",
               new { controller = "Video", action = "SetLorem" },
               new[] { "platformAthletic.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "team-addplayer",
                new { controller = "Team", action = "AddPlayer" },
                new[] { "platformAthletic.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "team-maxplayers",
                new { controller = "Team", action = "MaxPlayers" },
                new[] { "platformAthletic.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "team-removeplayer",
                new { controller = "Team", action = "DeletePlayer" },
                new[] { "platformAthletic.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "team-removeplayer",
                new { controller = "Team", action = "DeletePlayer" },
                new[] { "platformAthletic.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "set-sbc",
                new { controller = "Team", action = "SetSbc" },
                new[] { "platformAthletic.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "set-attendance",
                new { controller = "Team", action = "SetAttendance" },
                new[] { "platformAthletic.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "add-post",
                new { controller = "Home", action = "AddPost" },
                new[] { "platformAthletic.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "edit-post",
                new { controller = "Home", action = "EditPost" },
                new[] { "platformAthletic.Areas.Default.Controllers" }
            );
            context.MapRoute(
                null,
                "delete-post",
                new { controller = "Home", action = "DeletePost" },
                new[] { "platformAthletic.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "my-page",
                new { controller = "User", action = "MyPage" },
                new[] { "platformAthletic.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "player-page/{id}",
                new { controller = "Player", action = "Item" },
                new[] { "platformAthletic.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "set-field",
                new { controller = "Player", action = "SetField" },
                new[] { "platformAthletic.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "set-field-position",
                new { controller = "Player", action = "SelectFieldPosition" },
                new[] { "platformAthletic.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "set-pillar",
                new { controller = "Player", action = "Pillar" },
                new[] { "platformAthletic.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "get-text-above",
                new { controller = "Player", action = "GetTextAbove" },
                new[] { "platformAthletic.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "team-table",
                new { controller = "Table", action = "Team" },
                new[] { "platformAthletic.Areas.Default.Controllers" }
            );


            context.MapRoute(
                null,
                "table",
                new { controller = "Table", action = "Index" },
                new[] { "platformAthletic.Areas.Default.Controllers" }
            );

            context.MapRoute(
               null,
               "leaderboard",
               new { controller = "LeaderBoard", action = "Index" },
               new[] { "platformAthletic.Areas.Default.Controllers" }
           );

            context.MapRoute(
                null,
                "team-leaderboard",
                new { controller = "LeaderBoard", action = "Team" },
                new[] { "platformAthletic.Areas.Default.Controllers" }
            );


            context.MapRoute(
                null,
                "leader-board",
                new { controller = "OldLeaderBoard", action = "Team" },
                new[] { "platformAthletic.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "national-leader-board",
                new { controller = "OldLeaderBoard", action = "Index" },
                new[] { "platformAthletic.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "report",
                new { controller = "Report", action = "Index" },
                new[] { "platformAthletic.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "attendance-report",
                new { controller = "Report", action = "Attendance" },
                new[] { "platformAthletic.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "progress-report-print",
                new { controller = "Report", action = "ProgressPrint" },
                new[] { "platformAthletic.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "attendance-report-print",
                new { controller = "Report", action = "AttendancePrint" },
                new[] { "platformAthletic.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "progress-report",
                new { controller = "Report", action = "Progress" },
                new[] { "platformAthletic.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "privacy",
                new { controller = "Page", action = "Privacy" },
                new[] { "platformAthletic.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "term-and-condition",
                new { controller = "Page", action = "TermAndCondition" },
                new[] { "platformAthletic.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "view-workout",
                new { controller = "Table", action = "Preview" },
                new[] { "platformAthletic.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "invoice",
                new { controller = "Invoice", action = "Index" },
                new[] { "platformAthletic.Areas.Default.Controllers" }
            );

            context.MapRoute(
                "notFoundPage",
                "not-found-page",
                new { controller = "Error", action = "NotFoundPage" },
                new[] { "platformAthletic.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "online",
                new { controller = "Home", action = "Online" },
                new[] { "platformAthletic.Areas.Default.Controllers" }
            );


            context.MapRoute(
                null,
                "user/{id}",
                new { controller = "User", action = "Index" },
                new {id = @"\d+" },
                new[] { "platformAthletic.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "user/{action}/{id}",
                new { controller = "User", action = "Index", id = UrlParameter.Optional },
                new[] { "platformAthletic.Areas.Default.Controllers" }
            );

            context.MapRoute(
                null,
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new[] { "platformAthletic.Areas.Default.Controllers" }
            );
        }
    }
}
