using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Helper
{
    public class SpiderHelper
    {
        /// <summary>
        /// 爬取智联招聘职位列表
        /// </summary>
        /// <param name="areaName">地区名称</param>
        /// <param name="jobName">职位名称</param>
        /// <param name="page">页数</param>
        /// <returns>职位列表</returns>
        public static IEnumerable<object> SearchJob(string areaName, string jobName, int page)
        {
            string filter(string inner) => $"{inner}".Trim().Replace(Environment.NewLine, null).Replace("<b>", null).Replace("</b>", null);
            var url = $@"https://sou.zhaopin.com/jobs/searchresult.ashx?jl={areaName}&kw={jobName}&p={page}";
            return new HtmlWeb().Load(url).GetElementbyId("newlist_list_content_table").Elements("table")
            .Skip(1).Select(x =>
            {
                var tds1 = x?.Elements("tr")?.FirstOrDefault()?.Elements("td");
                var tds2 = x?.Elements("tr")?.LastOrDefault()?.Elements("td");
                return new
                {
                    JobName = filter(tds1?.ElementAtOrDefault(0)?.InnerText),
                    ResponseRate = filter(tds1?.ElementAtOrDefault(1)?.InnerText),
                    Company = filter(tds1?.ElementAtOrDefault(2)?.InnerText),
                    MonthlyPay = filter(tds1?.ElementAtOrDefault(3)?.InnerText),
                    WorkPlace = filter(tds1?.ElementAtOrDefault(4)?.InnerText),
                    ReleaseDate = filter(tds1?.ElementAtOrDefault(5)?.InnerText),
                    Desctiption = filter(tds2?.ElementAtOrDefault(0)?.InnerText),
                    Link = filter(tds1?.ElementAtOrDefault(0).Element("div").Element("a")?.GetAttributeValue("href", ""))
                };
            });
        }

    }
}
