using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Native.Csharp.App
{
    class Setting
    {
        private static SettingForm form = null;

        public static int Open()
        {
            if (form == null)
            {
                form = new SettingForm();
                form.Closing += Closing;
                form.Show();
                return 1;
            }
            else
            {
                form.Activate();
                form.Show();
                return 0;
            }
        }

        private static void Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            form = null;
        }
    }
}
