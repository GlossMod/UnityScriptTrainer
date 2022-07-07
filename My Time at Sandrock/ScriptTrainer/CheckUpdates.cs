using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ScriptTrainer
{
    public class CheckUpdates
    {


        public CheckUpdates(out string text)
        {
            text = "无法检查更新";

            string url = "https://mod.3dmgame.com/mod/API/185597";
            // 本地版本
            float localVersion = constant.version;

            WebClient MyWebClient = new WebClient();
            MyWebClient.Credentials = CredentialCache.DefaultCredentials;//获取或设置用于向Internet资源的请求进行身份验证的网络凭据
            Byte[] pageData = MyWebClient.DownloadData(url); //从指定网站下载数据
            string pageHtml = Encoding.Default.GetString(pageData);  //如果获取网站页面采用的是GB2312，则使用这句    
                                                                     //string pageHtml = Encoding.UTF8.GetString(pageData); //如果获取网站页面采用的是UTF-8，则使用这句

            JObject jsonData = (JObject)JsonConvert.DeserializeObject(pageHtml);

            float serverVersion = (float)jsonData["mods_version"];

            // 比较版本号
            if (localVersion < serverVersion)
            {
                text = "有新版本可用！";
            }
            else
            {
                text = "已是最新版本！";
            }
        }
    }
}
