using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Model;
using platformAthletic.Helpers;

namespace platformAthletic.Models.ViewModels
{
    public class UserPillarView
    {
        public int ID { get; set; }

        public int UserID { get; set; }

        public int PillarTypeID { get; set; }

        public string TextValue { get; set; }

        public int Value { get; set; }

        public DateTime AddedDate { get; set; }

        private IEnumerable<PillarType> PillarTypes
        {
            get
            {
                var repository = DependencyResolver.Current.GetService<IRepository>();
                return repository.PillarTypes.ToList();
            }
        }

        public IEnumerable<SelectListItem> SelectListPillarTypeID
        {
            get
            {
                return PillarTypes.Select(p => new SelectListItem
                                                    {
                                                        Value = p.ID.ToString(),
                                                        Text = p.Name,
                                                        Selected = p.ID == PillarTypeID
                                                    });
            }
        }


        public string TextAbove
        {
            get
            {
                var @type = PillarTypes.FirstOrDefault(p => p.ID == PillarTypeID);
                if (@type == null)
                {
                    @type = PillarTypes.First();
                }
                return @type.TextAbove;
            }
        }

        public string Placeholder
        {
            get
            {
                var @type = PillarTypes.FirstOrDefault(p => p.ID == PillarTypeID);
                if (@type == null)
                {
                    @type = PillarTypes.First();
                }
                return @type.Placeholder;
            }
        }


        public UserPillarView()
        {
            AddedDate = DateTime.Now.Current();
        }

    }
}