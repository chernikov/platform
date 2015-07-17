using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Model;

namespace platformAthletic.Models.ViewModels.User
{
    public class PlayerUserView : BaseUserView
    {
        public int FieldPositionID { get; set; }

        public int PlayerOfTeamID { get; set; }

        public string TeamName { get; set; }

        public string AvatarPath { get; set; }

        public int? GroupID { get; set; }

        public string FullAvatarPath
        {
            get
            {
                if (string.IsNullOrWhiteSpace(AvatarPath))
                {
                    return "/Media/images/no-userpic.png";
                }
                return AvatarPath;
            }
        }

        [Required]
        [StringLength(500, ErrorMessage = "The first name  can not exceed 500 characters")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "The last name can not exceed 500 characters")]
        public string LastName { get; set; }

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
                return FieldPositions.Select(p => new SelectListItem
                {
                    Value = p.ID.ToString(),
                    Text = p.Name,
                    Selected = p.ID == FieldPositionID
                });
            }
        }

        private IEnumerable<Group> Groups
        {
            get
            {
                var repository = DependencyResolver.Current.GetService<IRepository>();
                return repository.Groups.Where(p => p.TeamID == PlayerOfTeamID).ToList();
            }
        }

        public IEnumerable<SelectListItem> SelectListGroupID
        {
            get
            {
                yield return new SelectListItem
                {
                    Value = "",
                    Text = TeamName,
                    Selected = !GroupID.HasValue
                };
                foreach (var group in Groups)
                {
                    yield return new SelectListItem
                    {
                        Value = group.ID.ToString(),
                        Text = group.Name,
                        Selected = group.ID == GroupID
                    };
                }
            }
        }
    }
}