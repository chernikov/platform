using platformAthletic.Model;
using platformAthletic.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace platformAthletic.Models.Info
{
    public class ScheduleRowInfo : CalendarRowInfo
    {
        public string StrColor
        {
            get
            {

                if (UserSeason != null)
                {
                    Color color;
                    if (!IsDefault)
                    {
                        color = Color.FromArgb(0xff, 0x33, 0x33);
                    }
                    else
                    {
                        if (UserSeason.GroupID != null)
                        {
                            if (UserSeason.SeasonID == 1)
                            {
                                //group offseason
                                color = Color.FromArgb(0x2e, 0xa4, 0xff);
                            }
                            else
                            {
                                //group inseason
                                color = Color.FromArgb(0x32, 0xA4, 0xff);
                            }
                        }
                        else
                        {
                            if (UserSeason.SeasonID == 1)
                            {
                                //team offseason
                                color = Color.FromArgb(0xff, 0x6d, 0x22);
                            }
                            else
                            {
                                //team inseason
                                color = Color.FromArgb(0xdc, 0x14, 0x3c);
                            }
                        }
                    }
                    var factor = 0.4 * NumberOfWeek / 30 + 0.6;
                    color = AdjustBrightness(color, (float)factor);
                    return ColorToString(color);
                }
                return "#000";
            }
        }

        public UserSeason UserSeason { get; set; }

        private Color AdjustBrightness(Color c1, float factor)
        {

            float r = ((c1.R * factor) > 255) ? 255 : (c1.R * factor);
            float g = ((c1.G * factor) > 255) ? 255 : (c1.G * factor);
            float b = ((c1.B * factor) > 255) ? 255 : (c1.B * factor);

            Color c = Color.FromArgb(c1.A, (int)r, (int)g, (int)b);
            return c;
        }

        public string ColorToString(Color c) 
        {
            return string.Format("#{0:X02}{1:X02}{2:X02}", c.R, c.G, c.B);
        }
    }
}