namespace UserGuiLib.WinFormsExample
{
    partial class Example
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.userGuiControl1 = new UserGuiLib.GDI.UserGuiControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.userGuiControl2 = new UserGuiLib.GDI.UserGuiControl();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(969, 602);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.userGuiControl1);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(961, 573);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Color picker";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // userGuiControl1
            // 
            this.userGuiControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userGuiControl1.Location = new System.Drawing.Point(3, 3);
            this.userGuiControl1.Name = "userGuiControl1";
            this.userGuiControl1.Size = new System.Drawing.Size(955, 567);
            this.userGuiControl1.TabIndex = 0;
            this.userGuiControl1.Text = "userGuiControl1";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.userGuiControl2);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(274, 224);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Tree";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // userGuiControl2
            // 
            this.userGuiControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userGuiControl2.Location = new System.Drawing.Point(3, 3);
            this.userGuiControl2.Name = "userGuiControl2";
            this.userGuiControl2.Size = new System.Drawing.Size(268, 218);
            this.userGuiControl2.TabIndex = 0;
            this.userGuiControl2.Text = "userGuiControl2";
            // 
            // Example
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(969, 602);
            this.Controls.Add(this.tabControl1);
            this.Name = "Example";
            this.Text = "Example";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private GDI.UserGuiControl userGuiControl1;
        private GDI.UserGuiControl userGuiControl2;
    }
}