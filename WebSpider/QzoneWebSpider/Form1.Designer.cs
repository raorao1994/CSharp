namespace QzoneWebSpider
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.start = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.stopbtn = new System.Windows.Forms.Button();
            this.textBoxList = new System.Windows.Forms.TextBox();
            this.encodingselect = new System.Windows.Forms.ComboBox();
            this.DeepSelect = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.URLText = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // start
            // 
            this.start.Location = new System.Drawing.Point(333, 57);
            this.start.Name = "start";
            this.start.Size = new System.Drawing.Size(75, 23);
            this.start.TabIndex = 0;
            this.start.Text = "开始";
            this.start.UseVisualStyleBackColor = true;
            this.start.Click += new System.EventHandler(this.start_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.stopbtn);
            this.groupBox1.Controls.Add(this.textBoxList);
            this.groupBox1.Controls.Add(this.encodingselect);
            this.groupBox1.Controls.Add(this.DeepSelect);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.URLText);
            this.groupBox1.Controls.Add(this.start);
            this.groupBox1.Location = new System.Drawing.Point(13, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(414, 455);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "选项栏";
            // 
            // stopbtn
            // 
            this.stopbtn.Location = new System.Drawing.Point(333, 87);
            this.stopbtn.Name = "stopbtn";
            this.stopbtn.Size = new System.Drawing.Size(75, 23);
            this.stopbtn.TabIndex = 5;
            this.stopbtn.Text = "停止运行";
            this.stopbtn.UseVisualStyleBackColor = true;
            this.stopbtn.Click += new System.EventHandler(this.stopbtn_Click);
            // 
            // textBoxList
            // 
            this.textBoxList.Location = new System.Drawing.Point(6, 125);
            this.textBoxList.Multiline = true;
            this.textBoxList.Name = "textBoxList";
            this.textBoxList.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxList.Size = new System.Drawing.Size(402, 310);
            this.textBoxList.TabIndex = 4;
            // 
            // encodingselect
            // 
            this.encodingselect.FormattingEnabled = true;
            this.encodingselect.Items.AddRange(new object[] {
            "Default",
            "UTF-8",
            "ASCII"});
            this.encodingselect.Location = new System.Drawing.Point(249, 59);
            this.encodingselect.Name = "encodingselect";
            this.encodingselect.Size = new System.Drawing.Size(69, 20);
            this.encodingselect.TabIndex = 3;
            this.encodingselect.Text = "Default";
            // 
            // DeepSelect
            // 
            this.DeepSelect.FormattingEnabled = true;
            this.DeepSelect.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10"});
            this.DeepSelect.Location = new System.Drawing.Point(90, 59);
            this.DeepSelect.Name = "DeepSelect";
            this.DeepSelect.Size = new System.Drawing.Size(69, 20);
            this.DeepSelect.TabIndex = 3;
            this.DeepSelect.Text = "1";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 110);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "爬取信息：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(165, 62);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 12);
            this.label4.TabIndex = 2;
            this.label4.Text = "网页编码格式：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "加载深度：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "URL：";
            // 
            // URLText
            // 
            this.URLText.Location = new System.Drawing.Point(60, 20);
            this.URLText.Name = "URLText";
            this.URLText.Size = new System.Drawing.Size(348, 21);
            this.URLText.TabIndex = 1;
            // 
            // groupBox2
            // 
            this.groupBox2.Location = new System.Drawing.Point(434, 13);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(356, 454);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "其他信息";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(802, 479);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "Qzone爬取QQ号工具";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button start;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox DeepSelect;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox URLText;
        private System.Windows.Forms.TextBox textBoxList;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button stopbtn;
        private System.Windows.Forms.ComboBox encodingselect;
        private System.Windows.Forms.Label label4;
    }
}

