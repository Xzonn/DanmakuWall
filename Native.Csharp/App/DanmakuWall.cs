using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Drawing.Drawing2D;

namespace Native.Csharp.App
{
    public partial class DanmakuWall : Form
    {
        List<Message> messages = new List<Message>();
        static Font font = new Font(Common.Config.FontFamily, Common.Config.FontSize, FontStyle.Bold, GraphicsUnit.Pixel), emojiFont = new Font(Common.Config.EmojiFontFamily, Common.Config.EmojiFontSize, FontStyle.Bold, GraphicsUnit.Pixel);
        static SolidBrush fill = new SolidBrush(ColorTranslator.FromHtml(Common.Config.Color));
        static Pen border = new Pen(new SolidBrush(ColorTranslator.FromHtml(Common.Config.BorderColor)), Common.Config.BorderWidth);
        Bitmap backgroundImage = new Bitmap(1, 1);

        /// <summary>
        /// 弹幕信息类型。
        /// </summary>
        private class Message
        {
            public string Text;
            public DateTime Time;
            public int Top;
            public Bitmap Bitmap;
            public double Speed = 100.0;
        }

        /// <summary>
        /// 弹幕墙主体。
        /// </summary>
        public DanmakuWall()
        {
            InitializeComponent();
            Width = Screen.PrimaryScreen.WorkingArea.Size.Width;
            Height = Screen.PrimaryScreen.WorkingArea.Size.Height;
            pictureBox.Size = Size;
            border.LineJoin = LineJoin.Round;
            timer.Start();
            SendDanmaku(@"弹幕墙已经开启！");
        }

        public void SendDanmaku(string danmaku)
        {
            string MsgPattern = @"\[CQ:(image|face|emoji),[a-z]+=(.+?)\]";
            string MsgPatternWithoutId = @"\[CQ:(?:image|face|emoji),[a-z]+=.+?\]";
            string CqPattern = @"\[CQ:.*?\]";

            List<string> splitDanmaku = Regex.Split(danmaku, MsgPatternWithoutId).ToList();
            MatchCollection splitMsg = Regex.Matches(danmaku, MsgPattern);
            List<Bitmap> bitmaps = new List<Bitmap>();

            try
            {
                foreach (Match match in splitMsg)
                {
                    if (splitDanmaku[0] != "")
                        bitmaps.Add(StringToBitmap(Regex.Replace(splitDanmaku[0], CqPattern, ""), font));
                    splitDanmaku.RemoveAt(0);
                    switch (match.Groups[1].Value)
                    {
                        case "image":
                            string imagePath = match.Groups[2].Value;
                            if (!File.Exists(imagePath))
                                break;
                            Bitmap subBitmap = (Bitmap)Image.FromFile(imagePath);
                            int sourWidth = subBitmap.Width, sourHeight = subBitmap.Height;
                            int destWidth = 960, destHeight = 540, minWidth = 48, minHeight = 48;
                            int subWidth, subHeight;
                            if (sourHeight > destHeight || sourWidth > destWidth)
                            {
                                if ((sourWidth * destHeight) > (sourHeight * destWidth))
                                {
                                    subWidth = destWidth;
                                    subHeight = (destWidth * sourHeight) / sourWidth;
                                }
                                else
                                {
                                    subHeight = destHeight;
                                    subWidth = (sourWidth * destHeight) / sourHeight;
                                }
                                Bitmap destBitmap = new Bitmap(subWidth, subHeight);
                                Graphics destG = Graphics.FromImage(destBitmap);
                                destG.DrawImage(subBitmap, 0, 0, subWidth, subHeight);
                                destG.Dispose();
                                subBitmap.Dispose();
                                bitmaps.Add(destBitmap);
                            }
                            else if (sourHeight < minHeight || sourWidth < minWidth)
                            {
                                if ((sourWidth * destHeight) > (sourHeight * destWidth))
                                {
                                    subWidth = minWidth;
                                    subHeight = (minWidth * sourHeight) / sourWidth;
                                }
                                else
                                {
                                    subHeight = minHeight;
                                    subWidth = (sourWidth * minHeight) / sourHeight;
                                }
                                Bitmap destBitmap = new Bitmap(subWidth, subHeight);
                                Graphics destG = Graphics.FromImage(destBitmap);
                                destG.DrawImage(subBitmap, 0, 0, subWidth, subHeight);
                                destG.Dispose();
                                subBitmap.Dispose();
                                bitmaps.Add(destBitmap);
                            }
                            else
                            {
                                bitmaps.Add(subBitmap);
                            }
                            break;
                        case "face":
                            string facePath = Path.Combine(Common.AppDirectory, Common.Config.FacePath, match.Groups[2].Value + ".png");
                            if (File.Exists(facePath))
                            {
                                bitmaps.Add((Bitmap)Image.FromFile(facePath));
                            }
                            break;
                        case "emoji":
                            bitmaps.Add(StringToBitmap(char.ConvertFromUtf32(Convert.ToInt32(match.Groups[2].Value)), emojiFont));
                            break;
                    }
                }
                if (splitDanmaku[0] != "")
                    bitmaps.Add(StringToBitmap(Regex.Replace(splitDanmaku[0], CqPattern, ""), font));


                int width = 0, height = 0;
                if (bitmaps.Count == 0)
                    return;
                foreach (Bitmap wb in bitmaps)
                {
                    width += wb.Width;
                    height = Math.Max(height, wb.Height);
                }

                Bitmap bitmap = new Bitmap(width, height);
                Graphics g = Graphics.FromImage(bitmap);
                int x = 0;
                foreach (Bitmap wb in bitmaps)
                {
                    g.DrawImage(wb, new Point(x, (height - wb.Height) / 2));
                    x += wb.Width;
                }
                g.Dispose();
                Message message = new Message
                {
                    Text = danmaku,
                    Time = DateTime.Now,
                    Top = -1,
                    Bitmap = bitmap,
                    Speed = (bitmap.Width + Width) / 12.0
                };
                lock (messages)
                {
                    messages.Add(message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("出错了：" + ex.Message);
            }
        }

        /// <summary>
        /// 刷新弹幕显示。
        /// </summary>
        public void RefreshDanmaku()
        {
            backgroundImage.Dispose();
            backgroundImage = new Bitmap(Width, Height);
            int[] drawed = new int[Height];
            int by;
            Graphics g = Graphics.FromImage(backgroundImage);
            List<string> newMessage = new List<string>();
            List<Message> remove = new List<Message>();

            try
            {
                lock (messages)
                {
                    foreach (Message message in messages)
                    {
                        if (message.Top == -1)
                        {
                            for (by = 0; by < Height; by++)
                            {
                                if (drawed[by] == 0) break;
                            }
                            message.Top = by;
                        }
                        int left = Width - (int)((DateTime.Now - message.Time).TotalSeconds * message.Speed);
                        if (left + message.Bitmap.Width >= Width)
                        {
                            for (by = message.Top; by < Math.Min(Height, message.Top + message.Bitmap.Height); by++)
                            {
                                drawed[by] = 1;
                            }
                        }
                        else if (left + message.Bitmap.Width <= 0)
                        {
                            remove.Add(message);
                        }
                        g.DrawImage(message.Bitmap, new Point(left, message.Top));
                    }
                    foreach (Message removeMessage in remove)
                    {
                        messages.Remove(removeMessage);
                    }
                }
                if (InvokeRequired)
                {
                    Invoke(new MethodInvoker(delegate
                    {
                        pictureBox.Image = backgroundImage;
                    }));
                }
                else
                {
                    pictureBox.Image = backgroundImage;
                }
            }
            finally
            {
                g.Dispose();
            }
        }

        /// <summary>
        /// 计时器计时。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (Visible) RefreshDanmaku();
        }

        /// <summary>
        /// 将字符串绘制为 Bitmap。
        /// </summary>
        /// <param name="s">要绘制的字符串。</param>
        /// <param name="font">绘制使用的字体。</param>
        /// <returns>绘制的 Bitmap。</returns>
        private Bitmap StringToBitmap(string s, Font font)
        {
            SizeF fontSize = TextRenderer.MeasureText(s, font);
            Size size = new Size((int)(fontSize.Width), (int)(fontSize.Height + 2 * Common.Config.BorderWidth));
            Bitmap bitmap = new Bitmap(size.Width, size.Height);
            Graphics g = Graphics.FromImage(bitmap);
            GraphicsPath path = new GraphicsPath();
            path.AddString(s, font.FontFamily, (int)font.Style, font.Size, new Rectangle(Common.Config.BorderWidth, Common.Config.BorderWidth, size.Width, size.Height), StringFormat.GenericTypographic);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.DrawPath(border, path);
            g.FillPath(fill, path);
            g.Dispose();
            path.Dispose();
            return bitmap;
        }

        public void ClearDanmaku()
        {
            lock (messages)
            {
                messages.Clear();
            }
        }
    }
}
