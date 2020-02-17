using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Native.Csharp.App
{
    public partial class SettingForm : Form
    {
        public SettingForm()
        {
            InitializeComponent();
            List<string> admin = new List<string>();
            List<string> groups = new List<string>();
            foreach (long l in Common.ConfigLoader.Config.Admin.ToList())
            {
                admin.Add(l.ToString());
            }
            foreach (long l in Common.ConfigLoader.Config.Groups.ToList())
            {
                groups.Add(l.ToString());
            }
            textAdmin.Text = string.Join(" ", admin);
            textGroups.Text = string.Join(" ", groups);
            checkImage.Checked = Common.ConfigLoader.Config.AllowImage;
            checkNick.Checked = Common.ConfigLoader.Config.ShowName;
        }

        private void Confirm(object sender, EventArgs e)
        {
            try
            {
                List<long> admin = new List<long>();
                List<long> groups = new List<long>();
                foreach (string s in textAdmin.Text.Split(' '))
                {
                    admin.Add(Convert.ToInt64(s));
                }
                foreach (string s in textGroups.Text.Split(' '))
                {
                    groups.Add(Convert.ToInt64(s));
                }
                Common.ConfigLoader.Config.Admin = admin.ToArray();
                Common.ConfigLoader.Config.Groups = groups.ToArray();
                Common.ConfigLoader.Config.AllowImage = checkImage.Checked;
                Common.ConfigLoader.Config.ShowName = checkNick.Checked;
                if (Common.ConfigLoader.Save())
                {
                    MessageBox.Show("配置已保存。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                }
                else
                {
                    MessageBox.Show("配置保存出错，请检查是否有权限。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("填写错误。\n" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Common.CqApi.AddLoger(Sdk.Cqp.Enum.LogerLevel.Error, "错误", ex.Message);
            }
        }

        private void About(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://xzonn.github.io/posts/Danmaku-Wall.html");
        }
    }
}
