namespace MrLunarFestival2011
{
    partial class LunarConfig
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
            this.label1 = new System.Windows.Forms.Label();
            this.ProSel = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.HarvestDistance = new System.Windows.Forms.NumericUpDown();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.MountName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.HarvestDistance)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Harvest Distance";
            // 
            // ProSel
            // 
            this.ProSel.FormattingEnabled = true;
            this.ProSel.Items.AddRange(new object[] {
            "Eastern Kingdoms (Alliance)",
            "Kalimdor (Alliance)",
            "Northrend (Alliance)"});
            this.ProSel.Location = new System.Drawing.Point(12, 53);
            this.ProSel.Name = "ProSel";
            this.ProSel.Size = new System.Drawing.Size(200, 21);
            this.ProSel.TabIndex = 3;
            this.ProSel.SelectedIndexChanged += new System.EventHandler(this.ProSel_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(63, 37);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Profile Selection";
            // 
            // HarvestDistance
            // 
            this.HarvestDistance.Location = new System.Drawing.Point(134, 12);
            this.HarvestDistance.Name = "HarvestDistance";
            this.HarvestDistance.Size = new System.Drawing.Size(80, 20);
            this.HarvestDistance.TabIndex = 6;
            this.HarvestDistance.ValueChanged += new System.EventHandler(this.HarvestDistance_ValueChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(134, 106);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(86, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "Save Settings";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 83);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Flying Mount Name";
            // 
            // MountName
            // 
            this.MountName.Location = new System.Drawing.Point(105, 80);
            this.MountName.Name = "MountName";
            this.MountName.Size = new System.Drawing.Size(107, 20);
            this.MountName.TabIndex = 9;
            this.MountName.TextChanged += new System.EventHandler(this.MountName_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(-2, 116);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(109, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Mr.LunarFestival2011";
            // 
            // LunarConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(226, 129);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.MountName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.HarvestDistance);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ProSel);
            this.Controls.Add(this.label1);
            this.Name = "LunarConfig";
            this.Text = "Mr.LunarFestival 2011";
            ((System.ComponentModel.ISupportInitialize)(this.HarvestDistance)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox ProSel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown HarvestDistance;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox MountName;
        private System.Windows.Forms.Label label4;
    }
}