using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using platformAthletic.Model;
using System.Web.Mvc;

namespace platformAthletic.Models.Info
{
    public class VideoCollection
    {
        public enum SortTypeEnum
        {
            Training = 0x01,
            Pillar = 0x02
        }

        public class InnerVideo
        {
            public int ID { get; set; }

            public string TrainingName { get; set; }

            public string Header { get; set; }

            public string VideoUrl { get; set; }

            public string VideoCode { get; set; }

            public string Preview { get; set; }
        }

        public class SortPart
        {
            public string Header { get; set; }

            public List<InnerVideo> List { get; set; }

            public SortPart()
            {
                List = new List<InnerVideo>();
            }
        }

        private IRepository repository = DependencyResolver.Current.GetService<IRepository>();


        public SortTypeEnum SortType { get; set; }

        public string SearchString { get; set; }

        public int StartID { get; set; }

        public List<InnerVideo> Videos { get; set; }


        public IEnumerable<SortPart> SortedParts
        {
            get
            {
                SortPart sortPart = null;
                char currentHeader = ' ';
                foreach (var item in Videos.OrderBy(p => p.TrainingName))
                {
                    var firstChar = item.TrainingName[0];
                    if (currentHeader != firstChar && (!(firstChar >= '0' && firstChar <= '9' && currentHeader >= '0' && currentHeader <= '9')))
                    {
                        if (sortPart != null)
                        {
                            yield return sortPart;
                        }
                        currentHeader = firstChar;
                        if (firstChar >= '0' && firstChar <= '9')
                        {
                            sortPart = new SortPart()
                            {
                                Header = "0-9"
                            };
                        }
                        else
                        {
                            sortPart = new SortPart()
                            {
                                Header = firstChar.ToString().ToUpper()
                            };
                        }
                    }
                    sortPart.List.Add(item);
                }
                if (sortPart != null)
                {
                    yield return sortPart;
                }
            }
        }

        public VideoCollection(SortTypeEnum sortType, string searchString)
        {
            SortType = sortType;
            SearchString = searchString;
            Process();

            if (string.IsNullOrWhiteSpace(searchString) && Videos.Count > 0)
            {
                StartID = Videos.OrderBy(p => p.Header).FirstOrDefault().ID;
            }
            else
            {
                var item = Videos.FirstOrDefault(p => p.Header.IndexOf(SearchString, StringComparison.InvariantCultureIgnoreCase) != -1);
                if (item != null)
                {
                    StartID = item.ID;
                }
                else if (Videos.Count > 0)
                {
                    StartID = Videos.OrderBy(p => p.Header).FirstOrDefault().ID;
                }
            }
        }

        private void Process()
        {
            switch ((VideoCollection.SortTypeEnum)SortType)
            {
                case VideoCollection.SortTypeEnum.Training:
                    Videos = repository.Videos.Select(p => new VideoCollection.InnerVideo()
                    {
                        ID = p.ID,
                        Header = p.Training.Name ?? p.Header,
                        VideoCode = p.VideoCode,
                        VideoUrl = p.VideoUrl,
                        TrainingName = p.Training.Name ?? p.Header,
                        Preview = p.Preview,
                    }).ToList();
                    break;

                case VideoCollection.SortTypeEnum.Pillar:
                    Videos = repository.PillarTypes.Select(p => new VideoCollection.InnerVideo()
                    {
                        ID = p.ID,
                        Header = p.Name,
                        VideoCode = p.VideoCode,
                        VideoUrl = p.VideoUrl,
                        TrainingName = p.Name,
                        Preview = p.Preview,
                    }).ToList();
                    break;
            }
        }
    }
}