using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Models.Info;

namespace platformAthletic.Areas.Admin.Controllers
{
    public class PriceController : AdminController
    {
        [HttpGet]
        public ActionResult Index()
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

        [HttpPost]
        public ActionResult Index(PriceInfoView priceInfoView)
        {
            if (ModelState.IsValid)
            {
                var teamPrice = Repository.Settings.First(p => p.Name == "TeamPrice");
                teamPrice.Value = priceInfoView.TeamPrice.ToString();
                Repository.SaveSetting(teamPrice);

                var individualPrice = Repository.Settings.First(p => p.Name == "IndividualPrice");
                individualPrice.Value = priceInfoView.IndividualPrice.ToString();
                Repository.SaveSetting(individualPrice);

                var teamPriceImage = Repository.Settings.First(p => p.Name == "TeamPriceImagePath");
                teamPriceImage.Value = priceInfoView.TeamPriceImagePath.ToString();
                Repository.SaveSetting(teamPriceImage);

                var individualPriceImage = Repository.Settings.First(p => p.Name == "IndividualPriceImagePath");
                individualPriceImage.Value = priceInfoView.IndividualPriceImagePath.ToString();
                Repository.SaveSetting(individualPriceImage);
                ViewBag.Message = "Saved";
            }
            return View(priceInfoView);
        }
    }
}
