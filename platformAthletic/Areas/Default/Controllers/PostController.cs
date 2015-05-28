using AutoMapper;
using ImageResizer;
using ManageAttribute;
using platformAthletic.Global;
using platformAthletic.Model;
using platformAthletic.Models.ViewModels;
using platformAthletic.Tools;
using platformAthletic.Tools.GenerateTools;
using platformAthletic.Tools.Video;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace platformAthletic.Areas.Default.Controllers
{
    public class PostController : DefaultController
    {
        protected string DestinationDir = "Media/files/postimages/";
        //
        // GET: /Default/Post/

        public ActionResult Index(int id)
        {
            var post = Repository.Posts.FirstOrDefault(p => p.ID == id);
            if (post != null)
            {
                Repository.ViewPost(post.ID);
                return View(post);
            }
            return RedirectToAction("/");
        }

        public ActionResult List(int page = 1)
        {
            IQueryable<Post> list = null;
            if (CurrentUser.InRoles("coach,player"))
            {
                var team = CurrentUser.TeamOfPlay ?? CurrentUser.OwnTeam;
                if (team != null)
                {
                    var coach = team.User;
                    var admins = Repository.Users.Where(p => p.UserRoles.Any(r => string.Compare(r.Role.Code, "admin", true) == 0)).Select(p => p.ID);
                    list = Repository.Posts.Where(p => p.UserID == coach.ID || admins.Contains(p.UserID)).OrderByDescending(p => p.ID).Skip(2);
                }
            } else 
            {
                var admins = Repository.Users.Where(p => p.UserRoles.Any(r => string.Compare(r.Role.Code, "admin", true) == 0)).Select(p => p.ID);
                list = Repository.Posts.Where(p => admins.Contains(p.UserID)).OrderByDescending(p => p.ID);
            }
            if (list != null)
            {
                var data = new PageableData<Post>();
                data.Init(list, page, "Index", itemPerPage: 5);
                return View(data);
            }
            return null;
        }


        [HttpGet]
        public ActionResult Create()
        {
            return View("Edit", new PostView());
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var post = Repository.Posts.FirstOrDefault(p => p.ID == id);
            if (post != null && (post.UserID == CurrentUser.ID || CurrentUser.InRoles("admin")))
            {
                var postView = (PostView)Mapper.Map(post, typeof(Post), typeof(PostView));
                return View(postView);
            }
            return null;
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Edit(PostView postView)
        {
            var existPost = Repository.Posts.FirstOrDefault(p => p.ID == postView.ID);
            if (existPost == null || (existPost.UserID == CurrentUser.ID || CurrentUser.InRoles("admin")))
            {
                if (ModelState.IsValid)
                {
                    var post = (Post)Mapper.Map(postView, typeof(PostView), typeof(Post));
                    post.UserID = CurrentUser.ID;

                    if (!string.IsNullOrWhiteSpace(postView.VideoUrl))
                    {
                        var videoCode = VideoHelper.GetVideoByUrl(postView.VideoUrl, 800, 600);
                        var url = VideoHelper.GetVideoThumbByUrl(postView.VideoUrl);
                        var webClient = new WebClient();
                        var bytes = webClient.DownloadData(url);
                        var stream = new MemoryStream(bytes);
                        var uFile = StringExtension.GenerateNewFile() + Path.GetExtension(url);
                        var preview = "/" + Path.Combine(DestinationDir, uFile);
                        var filePath = Path.Combine(Path.Combine(Server.MapPath("~"), DestinationDir), uFile);

                        ImageBuilder.Current.Build(stream, filePath, new ResizeSettings("maxwidth=1600&crop=auto"));
                        post.VideoCode = videoCode;
                        post.VideoPreview = preview;
                    }
                    if (post.ID == 0)
                    {
                        Repository.CreatePost(post);
                    }
                    else
                    {
                        Repository.UpdatePost(post);
                    }
                    return View("_OK");
                }
            }
            return View(postView);
        }

        [ValidateInput(false)]
        [HttpPost]
        public FineUploaderResult UploadFile(FineUpload upload)
        {

            var uFile = StringExtension.GenerateNewFile() + Path.GetExtension(upload.Filename);
            var filePath = Path.Combine(Path.Combine(Server.MapPath("~"), DestinationDir), uFile);
            try
            {
                ImageBuilder.Current.Build(upload.InputStream, filePath, new ResizeSettings("maxwidth=1600&crop=auto"));
            }
            catch (Exception ex)
            {
                return new FineUploaderResult(false, error: ex.Message);
            }
            return new FineUploaderResult(true, new { fileUrl = "/" + DestinationDir + uFile });
        }

            
/*
        [TempAction]
        public ActionResult GeneratePosts()
        {
            var logger = new StringBuilder();

            var listOfCodes = new List<string>();
            var regexTemplate = ".*http://www\\.youtube\\.com/watch\\?v=(?<code>.*?)\" target=\"_blank\">";
            for (int i = 0; i < 100; i++)
            {
                var client = new WebClient();

                var httpPage = client.DownloadString("http://randomyoutube.net/");

                var regex = new Regex(regexTemplate);
                var matches = regex.Match(httpPage);
                if (matches.Success)
                {
                    var code = matches.Groups["code"].Value;
                    listOfCodes.Add(code);
                }
            }
            var admins = Repository.Users.Where(p => p.UserRoles.Any(r => string.Compare(r.Role.Code, "admin", true) == 0)).Select(p => p.ID);

            var rand = new Random((int)DateTime.Now.Ticks);
            for (int i = 0; i < 100; i++)
            {
                try
                {
                    var isPromoted = rand.Next(100) % 2 == 0;
                    var isMine = rand.Next(100) % 2 == 0;
                    int authorID = 0;
                    if (!isMine)
                    {
                        authorID = admins.OrderBy(p => Guid.NewGuid()).First();
                        logger.AppendLine("Is ADMIN AUTHOR");
                    }
                    else
                    {
                        authorID = CurrentUser.ID;
                        logger.AppendLine("Is CURRENT USER AUTHOR");
                    }
                    var isVideo = rand.Next(100) % 2 == 0;

                    var post = new Post();

                    post.UserID = authorID;
                    post.Promoted = isPromoted;
                    logger.AppendLine("Is PROMOTED");
                    post.Text = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed vel ultricies enim. In hac habitasse platea dictumst. In erat dui, sagittis id ligula et, tincidunt dapibus turpis. Fusce ullamcorper quam eu felis porttitor tempus. Suspendisse gravida lacus id tincidunt faucibus. Nullam finibus bibendum est, at sollicitudin nulla dapibus eu. In hendrerit ex sit amet lobortis dictum. Phasellus placerat lacus eget magna cursus, sit amet tincidunt magna rhoncus. Sed aliquam lacinia dignissim. Integer porttitor tempor tortor, hendrerit porttitor erat venenatis eu. Lorem ipsum dolor sit amet, consectetur adipiscing elit.

Cras imperdiet posuere erat, ac egestas nulla. Pellentesque congue at odio a lacinia. Nulla fringilla lorem eget ullamcorper rhoncus. Nulla sollicitudin tellus libero, vitae pellentesque dui pharetra eget. Cras sit amet suscipit felis, id faucibus ex. Proin non condimentum arcu. Praesent laoreet lorem ac velit bibendum, et commodo nisl luctus. Vivamus facilisis purus id est commodo, ac blandit lectus facilisis. Nunc volutpat viverra ultrices. Quisque cursus vehicula justo, porta imperdiet nisl elementum sit amet. Suspendisse potenti. Vestibulum posuere rhoncus nunc non rutrum. Donec euismod ac tortor et accumsan.

Lorem ipsum dolor sit amet, consectetur adipiscing elit. Morbi laoreet, velit sed dictum feugiat, dui est tempus massa, nec ullamcorper diam tellus at mi. Morbi ut gravida nulla, porttitor sollicitudin tellus. Sed feugiat purus sed rhoncus varius. Praesent luctus libero non dui molestie, non luctus turpis dictum. Donec mattis, augue id consectetur viverra, leo ligula rhoncus enim, id ultrices felis magna in magna. Vivamus luctus dolor et ante sollicitudin lobortis. Mauris eu arcu in sapien vestibulum gravida. Nulla et velit nisl. Donec id metus vitae erat volutpat tincidunt. Vivamus aliquet est velit, sed imperdiet leo eleifend in. Curabitur auctor tempor lorem, id sodales risus porttitor quis. Mauris malesuada vitae purus ac euismod. Maecenas vel orci in velit imperdiet aliquam quis vel lectus. Maecenas pellentesque turpis eu imperdiet ornare.

Ut sed augue blandit, auctor risus quis, ultrices massa. Integer consectetur nisi nec nisi placerat consequat. Donec ultrices nisl ut augue cursus ullamcorper. Donec mattis tempor rhoncus. In non venenatis augue. Cras lorem tellus, interdum eu tincidunt sit amet, ullamcorper a magna. Pellentesque at elementum risus, sed pretium diam. Vivamus tincidunt aliquam lectus, sed accumsan enim. Nullam malesuada vel tortor nec imperdiet. Maecenas metus enim, aliquet eu velit id, accumsan faucibus magna.

Pellentesque eget imperdiet felis. Nulla hendrerit magna vel felis pharetra, non maximus metus condimentum. Fusce vel auctor purus. Aenean placerat nibh nibh, in efficitur dui maximus a. Vivamus imperdiet nisi nunc, sit amet venenatis ligula placerat nec. Morbi congue varius ante, id fringilla elit imperdiet vitae. Nulla a lectus tincidunt, dapibus libero ut, pharetra enim. Etiam placerat dolor metus, sit amet blandit mauris posuere a.";

                    if (isVideo)
                    {
                        logger.AppendLine("Is VIDEO");
                        var code = listOfCodes.OrderBy(p => Guid.NewGuid()).First();

                        var videoUrl = "http://www.youtube.com/watch?v=" + code;
                        var videoCode = VideoHelper.GetVideoByUrl(videoUrl, 800, 600);

                        var url = VideoHelper.GetVideoThumbByUrl(videoUrl);
                        var webClient = new WebClient();
                        var bytes = webClient.DownloadData(url);
                        var stream = new MemoryStream(bytes);

                        var uFile = StringExtension.GenerateNewFile() + Path.GetExtension(url);
                        var preview = "/" + Path.Combine(DestinationDirVideo, uFile);
                        var filePath = Path.Combine(Path.Combine(Server.MapPath("~"), DestinationDirVideo), uFile);

                        ImageBuilder.Current.Build(stream, filePath, new ResizeSettings("maxwidth=1600&crop=auto"));
                        post.VideoCode = videoCode;
                        post.VideoUrl = videoUrl;
                        post.VideoPreview = preview;
                        post.TitleImagePath = "";
                    }
                    else
                    {
                        logger.AppendLine("Is NO VIDEO");
                        var file = Imaginarium.GetRandomSourceImage();
                        var fs = new FileStream(file, FileMode.Open);

                        var uFile = StringExtension.GenerateNewFile() + Path.GetExtension(file);
                        var preview = "/" + Path.Combine(DestinationDirVideo, uFile);
                        var filePath = Path.Combine(Path.Combine(Server.MapPath("~"), DestinationDirVideo), uFile);
                        ImageBuilder.Current.Build(fs, filePath, new ResizeSettings("maxwidth=1600&crop=auto"));
                        post.TitleImagePath = preview;
                    }
                    post.Header = GenerateData.Name.GetRandom() + " " + GenerateData.Team.GetRandom();
                    Repository.CreatePost(post);

                    logger.AppendLine(string.Format("POST {0} CREATED", post.Header));
                }
                catch (Exception ex)
                {
                    logger.AppendLine(string.Format("ERROR  {0}", ex.Message));
                }
            }
            return Content(logger.ToString());
        }*/

    }
}
