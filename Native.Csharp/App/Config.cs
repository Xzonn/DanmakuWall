namespace Native.Csharp.App
{
    public class Config
    {
        public long[] Admin = new long[0];
        public string FontFamily = "黑体";
        public float FontSize = 44;
        public string EmojiFontFamily = "Segoe UI Emoji";
        public float EmojiFontSize = 44;
        public string Color = "#FFFFFF";
        public string BorderColor = "#000000";
        public int BorderWidth = 5;
        public string FacePath = "wxFace/48/";
        public string WelcomeString = "您好，我是弹幕墙！\n如果您想发送弹幕，只需向我发送消息！\n文字、表情、图片都是可以的，但是语音、文件是不会被显示的。\n当然，请不要给我发红包，我也无法领取。\n祝您玩的开心！";
        public double TimeSpan = 0.5;
        public int MaxImageWidth = 960;
        public int MaxImageHeight = 540;
    }
}
