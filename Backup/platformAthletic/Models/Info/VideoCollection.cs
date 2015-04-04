using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using platformAthletic.Model;

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

        public SortTypeEnum SortType { get; set; }

        public string SearchString { get; set; }

        public List<InnerVideo> Videos { get; set; }

        public List<InnerVideo> SearchedVideos
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(SearchString))
                {
                    return Videos.Where(p => p.Header.IndexOf(SearchString, StringComparison.InvariantCultureIgnoreCase) != -1).ToList();
                }
                return Videos;
            }
        }


        public IEnumerable<SortPart> SortedParts
        {
            get
            {
                SortPart sortPart = null;
                char currentHeader = ' ';
                foreach (var item in SearchedVideos.OrderBy(p => p.TrainingName))
                {
                    var firstChar = item.TrainingName[0];
                    if (currentHeader != firstChar)
                    {
                        if (sortPart != null)
                        {
                            yield return sortPart;
                        }
                        currentHeader = firstChar;
                        sortPart = new SortPart()
                        {
                            Header = firstChar.ToString().ToUpper()
                        };
                    }
                    sortPart.List.Add(item);
                }
                if (sortPart != null)
                {
                    yield return sortPart;
                }
            }
        }
    }
}