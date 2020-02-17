using Native.Csharp.App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Native.Csharp.App
{
    class HandleMessage
    {
        static Dictionary<long, DateTime> danmakuSender = new Dictionary<long, DateTime>();

        public static bool ReceiveMessage(long fromQQ, string raw, bool response = true)
        {
            try
            {
                if (raw[0] == '#')
                {
                    if (Array.IndexOf(Common.ConfigLoader.Config.Admin, fromQQ) > -1)
                    {
                        string[] command = raw.Substring(1).Split(' ');
                        switch (command[0])
                        {
                            case "显示":
                            case "关闭":
                                if (Common.DanmakuWall.Visible)
                                {
                                    Common.DanmakuWall.Visible = false;
                                    if (response)
                                        Common.CqApi.SendPrivateMessage(fromQQ, "已关闭弹幕墙。");
                                }
                                else
                                {
                                    Common.DanmakuWall.Visible = true;
                                    if (response)
                                        Common.CqApi.SendPrivateMessage(fromQQ, "已显示弹幕墙。");
                                }
                                break;
                            case "清屏":
                                Common.DanmakuWall.ClearDanmaku();
                                if (response)
                                    Common.CqApi.SendPrivateMessage(fromQQ, "已清屏。");
                                break;
                            case "帮助":
                                if (response)
                                    Common.CqApi.SendPrivateMessage(fromQQ, "可选命令：[显示|关闭|清屏|帮助]。");
                                break;
                            default:
                                if (response)
                                    Common.CqApi.SendPrivateMessage(fromQQ, "出错了：不存在此命令。");
                                break;
                        }
                    }
                    else
                    {
                        if (response)
                            Common.CqApi.SendPrivateMessage(fromQQ, "出错了：您不是管理员，请不要以 # 为开头发送消息。");
                        Common.CqApi.AddLoger(Sdk.Cqp.Enum.LogerLevel.Info_Receive, "提示", $"{fromQQ} 正在尝试发送命令。");
                    }
                }
                else
                {
                    if (danmakuSender.ContainsKey(fromQQ))
                    {
                        if ((DateTime.Now - danmakuSender[fromQQ]).TotalSeconds < Common.ConfigLoader.Config.TimeSpan)
                        {
                            if (response)
                                Common.CqApi.SendPrivateMessage(fromQQ, $"您发送的频率过快，请 {Common.ConfigLoader.Config.TimeSpan} 秒后再发送。");
                            return false;
                        }
                        else
                        {
                            danmakuSender[fromQQ] = DateTime.Now;
                        }
                    }
                    else
                    {
                        danmakuSender.Add(fromQQ, DateTime.Now);
                    }
                    string message = raw;
                    string ImagePattern = @"\[CQ:image,file=([A-F0-9]+\.(?:jpg|png|bmp|jpeg|gif)?)\]";
                    string ImagePathPattern = @"\[CQ:image,path=.+?\]";
                    message = Regex.Replace(message, ImagePattern, x => "[CQ:image,path=" + Common.CqApi.ReceiveImage(x.Groups[1].Value) + "]");
                    if (message == "") return false;
                    Common.DanmakuWall.SendDanmaku((Common.ConfigLoader.Config.ShowName ? Common.Users.getName(fromQQ) + "：" : "") + Common.CqApi.CqCode_UnTrope(message));
                    message = Regex.Replace(message, ImagePathPattern, "<图片>");
                    if (response)
                        Common.CqApi.SendPrivateMessage(fromQQ, $"已收到弹幕：{message}");
                }
                return true;
            }
            catch (Exception ex)
            {
                if (response)
                    Common.CqApi.SendPrivateMessage(fromQQ, "出错了：" + ex.Message);
                return false;
            }
        }
    }
}
