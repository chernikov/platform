using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Attributes.Validation;
using platformAthletic.Model;

namespace platformAthletic.Models.ViewModels.User
{ 
	public class UserView : BaseUserView
    {
        public string AvatarPath { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [ValidPhone(ErrorMessage = "Enter Correct Phone Number")]
        public string PhoneNumber { get; set; }

        public DateTime? PaidTill { get; set; }

        public int? PlayerOfTeamID { get; set; }

        public int? FieldPositionID { get; set; }

        public int? Year { get; set; }

        public double Squat { get; set; }

        public double Bench { get; set; }

        public double Clean { get; set; }

        public string Height { get; set; }

        public string Weight { get; set; }

        public string BodyFat { get; set; }

        public double? _40YardDash { get; set; }

        public double? Vertical { get; set; }

        public double? _3Cone { get; set; }

        public double? TDrill { get; set; }

        public string PrimaryColor { get; set; }

        public string SecondaryColor { get; set; }

        private IEnumerable<Team> Teams
        {
            get
            {
                var repository = DependencyResolver.Current.GetService<IRepository>();
                return repository.Teams.ToList();
            }
        }

        public IEnumerable<SelectListItem> SelectListPlayerOfTeamID
        {
            get
            {
                yield return new SelectListItem
                {
                    Value = "",
                    Text = "None",
                    Selected = !PlayerOfTeamID.HasValue
                };
                foreach (var team in Teams)
                {
                    yield return new SelectListItem
                    {
                        Value = team.ID.ToString(),
                        Text = team.Name,
                        Selected = team.ID == PlayerOfTeamID
                    };
                }
            }
        }

        private IEnumerable<FieldPosition> FieldPositions
        {
            get
            {
                var repository = DependencyResolver.Current.GetService<IRepository>();
                return repository.FieldPositions.ToList();
            }
        }

        public IEnumerable<SelectListItem> SelectListFieldPositionID
        {
            get
            {
                yield return new SelectListItem
                {
                    Value = "",
                    Text = "None",
                    Selected = !FieldPositionID.HasValue
                };
                foreach (var fieldPosition in FieldPositions)
                {
                    yield return new SelectListItem
                    {
                        Value = fieldPosition.ID.ToString(),
                        Text = fieldPosition.Name,
                        Selected = fieldPosition.ID == FieldPositionID
                    };
                }
            }
        }
    }

}