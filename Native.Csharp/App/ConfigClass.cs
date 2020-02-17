using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;

namespace Native.Csharp.App
{
    public class ConfigLoader
    {
        public class ConfigClass
        {
            public long[] Admin = new long[0];
            public long[] Groups = new long[0];
            public string FontFamily = "黑体";
            public float FontSize = 44;
            public string EmojiFontFamily = "Segoe UI Emoji";
            public float EmojiFontSize = 44;
            public string Color = "#FFFFFF";
            public string BorderColor = "#000000";
            public int BorderWidth = 5;
            public string FacePath = "wxFace/48/";
            public string WelcomeString = "添加成功。您可以发送文字、表情或图片。";
            public double TimeSpan = 0.5;
            public int MaxImageWidth = 960;
            public int MaxImageHeight = 540;
            public bool AllowImage = true;
            public bool ShowName = true;
        }
        public ConfigClass Config = new ConfigClass();

        public bool Load()
        {
            try
            {
                Config = JsonConvert.DeserializeObject<ConfigClass>(File.ReadAllText(Path.Combine(Common.AppDirectory, "config.json"), Encoding.UTF8));
                Common.CqApi.AddLoger(Sdk.Cqp.Enum.LogerLevel.Info, "提示", "已载入配置文件");
                return true;
            }
            catch
            {
                Common.CqApi.AddLoger(Sdk.Cqp.Enum.LogerLevel.Info, "提示", "找不到配置文件，使用默认配置文件");
                return false;
            }
        }

        public bool Save()
        {
            try
            {
                File.WriteAllText(Path.Combine(Common.AppDirectory, "config.json"), JsonConvert.SerializeObject(Config), Encoding.UTF8);
                Common.CqApi.AddLoger(Sdk.Cqp.Enum.LogerLevel.Info, "提示", "已保存配置文件");
                return true;
            }
            catch
            {
                Common.CqApi.AddLoger(Sdk.Cqp.Enum.LogerLevel.Error, "错误", "无法保存配置文件，请确认是否有权限");
                return false;
            }
        }
    }
}
