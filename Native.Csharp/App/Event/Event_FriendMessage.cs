using Native.Csharp.App.Interface;
using Native.Csharp.App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Net;

namespace Native.Csharp.App.Event
{
    public class Event_FriendMessage : IEvent_FriendMessage
    {
        #region --公开方法--
        /// <summary>
        /// Type=201 好友已添加<para/>
        /// 处理好友已经添加事件
        /// </summary>
        /// <param name="sender">事件的触发对象</param>
        /// <param name="e">事件的附加参数</param>
        public void ReceiveFriendIncrease (object sender, FriendIncreaseEventArgs e)
        {
            // 本子程序会在酷Q【线程】中被调用，请注意使用对象等需要初始化(CoInitialize,CoUninitialize)。
            // 这里处理消息
            Common.CqApi.SendPrivateMessage(e.FromQQ, "你好，我是弹幕墙！\n如果您想发送弹幕，只需向我发送消息！\n文字、表情、图片都是可以的，但是语音、文件是不会被显示的。\n当然，请不要给我发红包，我也无法领取。\n如有问题，请联系开发者 Xzonn (QQ: 1223378017)。\n祝您玩的开心！");
            // Common.CqApi.SendPrivateMessage(Common.Config.Admin[0], $"已添加新的好友：{e.FromQQ}");
            Common.CqApi.AddLoger(Sdk.Cqp.Enum.LogerLevel.Info_Receive, "好友", $"已添加新的好友：{e.FromQQ}");

            e.Handled = true;   // 关于返回说明, 请参见 "Event_FriendMessage.ReceiveFriendMessage" 方法
        }

        /// <summary>
        /// Type=301 收到好友添加请求<para/>
        /// 处理收到的好友添加请求
        /// </summary>
        /// <param name="sender">事件的触发对象</param>
        /// <param name="e">事件的附加参数</param>
        public void ReceiveFriendAddRequest (object sender, FriendAddRequestEventArgs e)
        {
            // 本子程序会在酷Q【线程】中被调用，请注意使用对象等需要初始化(CoInitialize,CoUninitialize)。
            // 这里处理消息

            e.Handled = false;   // 关于返回说明, 请参见 "Event_ReceiveMessage.ReceiveFriendMessage" 方法
        }

        /// <summary>
        /// Type=21 好友消息<para/>
        /// 处理收到的好友消息
        /// </summary>
        /// <param name="sender">事件的触发对象</param>
        /// <param name="e">事件的附加参数</param>
        public void ReceiveFriendMessage (object sender, PrivateMessageEventArgs e)
        {
            // 本子程序会在酷Q【线程】中被调用，请注意使用对象等需要初始化(CoInitialize,CoUninitialize)。
            // 这里处理消息
            try
            {
                if (e.Msg[0] == '#')
                {
                    if (Array.IndexOf(Common.Config.Admin, e.FromQQ) > -1)
                    {
                        string[] command = e.Msg.Substring(1).Split(' ');
                        switch (command[0])
                        {
                            case "显示":
                            case "关闭":
                                if (Common.DanmakuWall.Visible)
                                {
                                    Common.DanmakuWall.Visible = false;
                                    Common.CqApi.SendPrivateMessage(e.FromQQ, "已关闭弹幕墙。");
                                }
                                else
                                {
                                    Common.DanmakuWall.Visible = true;
                                    Common.CqApi.SendPrivateMessage(e.FromQQ, "已显示弹幕墙。");
                                }
                                break;
                            case "清屏":
                                Common.DanmakuWall.ClearDanmaku();
                                Common.CqApi.SendPrivateMessage(e.FromQQ, "已清屏。");
                                break;
                            case "帮助":
                                Common.CqApi.SendPrivateMessage(e.FromQQ, "可选命令：[显示|关闭|清屏|帮助]。");
                                break;
                            default:
                                Common.CqApi.SendPrivateMessage(e.FromQQ, "出错了：不存在此命令。");
                                break;
                        }
                    }
                    else
                    {
                        Common.CqApi.SendPrivateMessage(e.FromQQ, "出错了：您不是管理员，请不要以 # 为开头发送消息。");
                        Common.CqApi.AddLoger(Sdk.Cqp.Enum.LogerLevel.Info_Receive, "提示", $"{e.FromQQ} 正在尝试发送命令。");
                    }
                }
                else
                {
                    string message = e.Msg;
                    string ImagePattern = @"\[CQ:image,file=([A-F0-9]+\.(?:jpg|png|bmp|jpeg|gif)?)\]";
                    string ImagePathPattern = @"\[CQ:image,path=.+?\]";
                    message = Regex.Replace(message, ImagePattern, x => "[CQ:image,path=" + Common.CqApi.ReceiveImage(x.Groups[1].Value) + "]");
                    if (message == "") return;
                    Common.DanmakuWall.SendDanmaku(Common.CqApi.CqCode_UnTrope(message));
                    message = Regex.Replace(message, ImagePathPattern, "<图片>");
                    Common.CqApi.SendPrivateMessage(e.FromQQ, $"已收到弹幕：{message}");
                }
                e.Handled = true;
                return;
            }
            catch (Exception ex)
            {
                Common.CqApi.SendPrivateMessage(e.FromQQ, "出错了：" + ex.Message);
            }
            // e.Handled 相当于 原酷Q事件的返回值
            // 如果要回复消息，请调用api发送，并且置 true - 截断本条消息，不再继续处理 //注意：应用优先级设置为"最高"(10000)时，不得置 true
            // 如果不回复消息，交由之后的应用/过滤器处理，这里置 false  - 忽略本条消息
        }
        #endregion
    }
}
