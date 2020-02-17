namespace Native.Csharp.App
{
    partial class SettingForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingForm));
            this.labelAdmin = new System.Windows.Forms.Label();
            this.labelGroup = new System.Windows.Forms.Label();
            this.textAdmin = new System.Windows.Forms.TextBox();
            this.textGroups = new System.Windows.Forms.TextBox();
            this.buttonConfirm = new System.Windows.Forms.Button();
            this.linkAbout = new System.Windows.Forms.LinkLabel();
            this.checkImage = new System.Windows.Forms.CheckBox();
            this.checkNick = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // labelAdmin
            // 
            this.labelAdmin.Location = new System.Drawing.Point(10, 10);
            this.labelAdmin.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.labelAdmin.Name = "labelAdmin";
            this.labelAdmin.Size = new System.Drawing.Size(120, 30);
            this.labelAdmin.TabIndex = 0;
            this.labelAdmin.Text = "管理员：";
            this.labelAdmin.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textAdmin
            // 
            this.textAdmin.Location = new System.Drawing.Point(130, 10);
            this.textAdmin.Name = "textAdmin";
            this.textAdmin.Size = new System.Drawing.Size(240, 29);
            this.textAdmin.TabIndex = 1;
            // 
            // labelGroup
            // 
            this.labelGroup.Location = new System.Drawing.Point(10, 50);
            this.labelGroup.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.labelGroup.Name = "labelGroup";
            this.labelGroup.Size = new System.Drawing.Size(120, 30);
            this.labelGroup.TabIndex = 2;
            this.labelGroup.Text = "QQ群：";
            this.labelGroup.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textGroups
            // 
            this.textGroups.Location = new System.Drawing.Point(130, 50);
            this.textGroups.Name = "textGroups";
            this.textGroups.Size = new System.Drawing.Size(240, 29);
            this.textGroups.TabIndex = 3;
            // 
            // checkImage
            // 
            this.checkImage.Location = new System.Drawing.Point(130, 90);
            this.checkImage.Name = "checkImage";
            this.checkImage.Size = new System.Drawing.Size(120, 30);
            this.checkImage.TabIndex = 4;
            this.checkImage.Text = "显示图片";
            this.checkImage.UseVisualStyleBackColor = true;
            // 
            // checkNick
            // 
            this.checkNick.Location = new System.Drawing.Point(130, 130);
            this.checkNick.Name = "checkNick";
            this.checkNick.Size = new System.Drawing.Size(120, 30);
            this.checkNick.TabIndex = 5;
            this.checkNick.Text = "显示昵称";
            this.checkNick.UseVisualStyleBackColor = true;
            // 
            // buttonConfirm
            // 
            this.buttonConfirm.Location = new System.Drawing.Point(130, 170);
            this.buttonConfirm.Name = "buttonConfirm";
            this.buttonConfirm.Size = new System.Drawing.Size(120, 30);
            this.buttonConfirm.TabIndex = 6;
            this.buttonConfirm.Text = "保存";
            this.buttonConfirm.UseVisualStyleBackColor = true;
            this.buttonConfirm.Click += new System.EventHandler(this.Confirm);
            // 
            // linkAbout
            // 
            this.linkAbout.Location = new System.Drawing.Point(130, 210);
            this.linkAbout.Name = "linkAbout";
            this.linkAbout.Size = new System.Drawing.Size(120, 30);
            this.linkAbout.TabIndex = 7;
            this.linkAbout.TabStop = true;
            this.linkAbout.Text = "帮助文档";
            this.linkAbout.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.linkAbout.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.About);
            // 
            // SettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(380, 250);
            this.Controls.Add(this.linkAbout);
            this.Controls.Add(this.buttonConfirm);
            this.Controls.Add(this.checkNick);
            this.Controls.Add(this.checkImage);
            this.Controls.Add(this.textGroups);
            this.Controls.Add(this.labelGroup);
            this.Controls.Add(this.textAdmin);
            this.Controls.Add(this.labelAdmin);
            this.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "SettingForm";
            this.Text = "设置";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelAdmin;
        private System.Windows.Forms.TextBox textAdmin;
        private System.Windows.Forms.Label labelGroup;
        private System.Windows.Forms.TextBox textGroups;
        private System.Windows.Forms.CheckBox checkImage;
        private System.Windows.Forms.CheckBox checkNick;
        private System.Windows.Forms.Button buttonConfirm;
        private System.Windows.Forms.LinkLabel linkAbout;
    }
}