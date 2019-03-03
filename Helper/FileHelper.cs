using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Helper
{
    public class FileHelper
    {
        /// <summary>
        /// 根据正则表达式获取目标路径下，深度遍历所有匹配的文件
        /// </summary>
        /// <param name="dirPath">文件夹目录</param>
        /// <param name="fileList">存放的列表容器</param>
        /// <param name="regexPattern">正则表达式模板</param>
        /// <returns></returns>
        public static List<string> QueryFiles(string dirPath, List<string> fileList, string regexPattern = "")
        {
            fileList.AddRange(Directory.GetFiles(dirPath).Where(x => Regex.IsMatch(x, regexPattern)));
            foreach (string directory in Directory.GetDirectories(dirPath))
                QueryFiles(directory, fileList, regexPattern);
            return fileList;
        }
        /// <summary>
        /// 启动Chrome浏览器并关闭同源策略
        /// </summary>
        /// <param name="chromePath"></param>
        public static void RunChromeToNoCors(string chromePath)
        {
            Process.Start($"{chromePath}", " --disable-web-security --user-data-dir");
        }
    }
}
