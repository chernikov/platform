using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;
using System.Text;
using platformAthletic.Global;

namespace platformAthletic.Helpers
{
    public static class CustomHelpers
    {

        public static MvcHtmlString Nl2Br(this HtmlHelper html, string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return new MvcHtmlString(string.Empty);
            }
            return new MvcHtmlString(input.Replace("\r\n", "<br />\r\n"));
        }

        public static MvcHtmlString PageLinks(this HtmlHelper html, int currentPage, int totalPages, Func<int, string> pageUrl)
        {
            var builder = new StringBuilder();
            for (int i = 1; i <= totalPages; i++)
            {
                if (((i <= 3) || (i > (totalPages - 3))) || ((i > (currentPage - 2)) && (i < (currentPage + 2))))
                {
                    var subBuilder = new TagBuilder("a");
                    subBuilder.MergeAttribute("href", pageUrl.Invoke(i));
                    subBuilder.InnerHtml = i.ToString(CultureInfo.InvariantCulture);
                    if (i == currentPage)
                    {
                        subBuilder.AddCssClass("selected");
                    }
                    builder.AppendLine(subBuilder.ToString());
                }
                else if ((i == 4) && (currentPage > 5))
                {
                    builder.AppendLine(" ... ");
                }
                else if ((i == (totalPages - 3)) && (currentPage < (totalPages - 4)))
                {
                    builder.AppendLine(" ... ");
                }
            }
            return new MvcHtmlString(builder.ToString());
        }

        public static MvcHtmlString PageLinksMessage(this HtmlHelper html, int currentPage, int totalPages, int itemPerPage, Func<int, string> pageUrl)
        {
            var builder = new StringBuilder();
            for (int i = 1; i <= totalPages; i++)
            {
                if (((i <= 3) || (i > (totalPages - 3))) || ((i > (currentPage - 2)) && (i < (currentPage + 2))))
                {
                    var subBuilder = new TagBuilder("a");
                    subBuilder.MergeAttribute("href", pageUrl.Invoke(i));
                    subBuilder.InnerHtml = string.Format("{0}-{1}", (i - 1) * itemPerPage + 1, i * itemPerPage);
                    if (i == currentPage)
                    {
                        subBuilder.AddCssClass("selected");
                    }
                    builder.AppendLine(subBuilder.ToString());
                }
                else if ((i == 4) && (currentPage > 5))
                {
                    builder.AppendLine(" ... ");
                }
                else if ((i == (totalPages - 3)) && (currentPage < (totalPages - 4)))
                {
                    builder.AppendLine(" ... ");
                }
            }
            return new MvcHtmlString(builder.ToString());
        }

        public static MvcHtmlString PageLinkLancer(this HtmlHelper html, int currentPage, int totalPages, Func<int, string> pageUrl)
        {
            var sb = new StringBuilder();

            // Previous
            if (currentPage > 1)
            {
                var subBuilder = new TagBuilder("a");
                subBuilder.MergeAttribute("href", pageUrl.Invoke(1));
                subBuilder.InnerHtml = "Предыдущая";
                sb.AppendFormat("<div class=\"prev\">← {0}</div>", subBuilder);
            }
            else
            {
                sb.Append("<div class=\"empty-prev\">&nbsp;</div>");
            }
            sb.Append("<div class=\"current\">");
            for (int i = 1; i <= totalPages; i++)
            {
                if (((i <= 3) || (i > (totalPages - 3))) || ((i > (currentPage - 2)) && (i < (currentPage + 2))))
                {

                    if (i == currentPage)
                    {
                        var subBuilder = new TagBuilder("span");
                        subBuilder.MergeAttribute("style", "font-weight: bold;");
                        subBuilder.InnerHtml = i.ToString(CultureInfo.InvariantCulture);
                        sb.AppendLine(subBuilder.ToString());
                    }
                    else
                    {
                        var subBuilder = new TagBuilder("a");
                        subBuilder.MergeAttribute("href", pageUrl.Invoke(i));
                        subBuilder.InnerHtml = i.ToString(CultureInfo.InvariantCulture);
                        sb.AppendLine(subBuilder.ToString());
                    }
                }
                else if ((i == 4) && (currentPage > 5))
                {
                    sb.AppendLine(" ... ");
                }
                else if ((i == (totalPages - 3)) && (currentPage < (totalPages - 4)))
                {
                    sb.AppendLine(" ... ");
                }
            }
            sb.Append("</div>");
            // Next
            if (currentPage < totalPages)
            {
                var subBuilder = new TagBuilder("a");
                subBuilder.MergeAttribute("href", pageUrl.Invoke(totalPages));
                subBuilder.InnerHtml = "Следующая";
                sb.AppendFormat("<div class=\"next\">{0} →</div>", subBuilder);
            }
            else
            {
                sb.Append("<div class=\"empty-next\">&nbsp;</div>");
            }

            return new MvcHtmlString(sb.ToString());
        }

        public static MvcHtmlString PageLinksLogoden(this HtmlHelper html, int currentPage, int totalPages, Func<int, string> pageUrl)
        {
            var builder = new StringBuilder();

            var subLeftArrowBuilderA = new TagBuilder("a");
            if (currentPage - 1 > 0)
            {
                subLeftArrowBuilderA.MergeAttribute("href", pageUrl.Invoke(currentPage - 1));
                subLeftArrowBuilderA.AddCssClass("left-arrow");
            }
            else
            {
                subLeftArrowBuilderA.AddCssClass("left-arrow-d");
            }
            builder.AppendLine(subLeftArrowBuilderA.ToString());
            for (int i = 1; i <= totalPages; i++)
            {
                if (((i <= 3) || (i > (totalPages - 3))) || ((i > (currentPage - 2)) && (i < (currentPage + 2))))
                {
                    var subBuilder = new TagBuilder("a")
                    {
                        InnerHtml = i.ToString(CultureInfo.InvariantCulture)
                    };
                    subBuilder.MergeAttribute("href", pageUrl.Invoke(i));
                    if (i == currentPage)
                    {
                        subBuilder.AddCssClass("selected");
                    }
                    builder.AppendLine(subBuilder.ToString());
                }
                else if ((i == 4) && (currentPage > 5))
                {
                    builder.AppendLine(" ... ");
                }
                else if ((i == (totalPages - 3)) && (currentPage < (totalPages - 4)))
                {
                    builder.AppendLine(" ... ");
                }
            }
            var subRightArrowBuilderA = new TagBuilder("a");
            if (currentPage < totalPages)
            {
                subRightArrowBuilderA.MergeAttribute("href", pageUrl.Invoke(currentPage + 1));
                subRightArrowBuilderA.AddCssClass("right-arrow");
            }
            else
            {
                subRightArrowBuilderA.AddCssClass("right-arrow-d");
            }
            builder.AppendLine(subRightArrowBuilderA.ToString());

            return new MvcHtmlString(builder.ToString());
        }


        public static MvcHtmlString PageLinkPlatform(this HtmlHelper html, int currentPage, int totalPages, Func<int, string> pageUrl)
        {
            var builder = new StringBuilder();
            var ulBuilder = new TagBuilder("ul");
            var liBuilder = new TagBuilder("li");

            var subLeftArrowBuilderA = new TagBuilder("a");
            subLeftArrowBuilderA.AddCssClass("arrow-left");
            if (currentPage - 1 > 0)
            {
                subLeftArrowBuilderA.MergeAttribute("href", pageUrl.Invoke(currentPage - 1));
            }
            liBuilder.InnerHtml = subLeftArrowBuilderA.ToString();
            builder.AppendLine(liBuilder.ToString());
            for (int i = 1; i <= totalPages; i++)
            {
                if (((i <= 3) || (i > (totalPages - 3))) || ((i > (currentPage - 2)) && (i < (currentPage + 2))))
                {
                    liBuilder = new TagBuilder("li");
                    var subBuilder = new TagBuilder("a")
                    {
                        InnerHtml = i.ToString(CultureInfo.InvariantCulture)
                    };
                    subBuilder.MergeAttribute("href", pageUrl.Invoke(i));
                    if (i == currentPage)
                    {
                        liBuilder.AddCssClass("selected");
                    }
                    liBuilder.InnerHtml = subBuilder.ToString();
                    builder.AppendLine(liBuilder.ToString());
                }
                else if ((i == 4) && (currentPage > 5))
                {
                    liBuilder = new TagBuilder("li");
                    liBuilder.InnerHtml = "... ";
                    builder.AppendLine(liBuilder.ToString());
                }
                else if ((i == (totalPages - 3)) && (currentPage < (totalPages - 4)))
                {
                    liBuilder = new TagBuilder("li");
                    liBuilder.InnerHtml = "... ";
                    builder.AppendLine(liBuilder.ToString());
                }
            }
            liBuilder = new TagBuilder("li");
            var subRightArrowBuilderA = new TagBuilder("a");
            subRightArrowBuilderA.AddCssClass("arrow-right");
            if (currentPage < totalPages)
            {
                subRightArrowBuilderA.MergeAttribute("href", pageUrl.Invoke(currentPage + 1));
            }
            liBuilder.InnerHtml = subRightArrowBuilderA.ToString();
            builder.AppendLine(liBuilder.ToString());

            ulBuilder.InnerHtml = builder.ToString();

            return new MvcHtmlString(ulBuilder.ToString());
        }
    }
}