namespace APP
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
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.button_add = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_key = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_val = new System.Windows.Forms.TextBox();
            this.button_edit = new System.Windows.Forms.Button();
            this.button_del = new System.Windows.Forms.Button();
            this.button_queryS = new System.Windows.Forms.Button();
            this.button_queryAll = new System.Windows.Forms.Button();
            this.textBox_msg = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button_delall = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button_add
            // 
            this.button_add.Location = new System.Drawing.Point(286, 13);
            this.button_add.Name = "button_add";
            this.button_add.Size = new System.Drawing.Size(75, 23);
            this.button_add.TabIndex = 0;
            this.button_add.Text = "添加";
            this.button_add.UseVisualStyleBackColor = true;
            this.button_add.Click += new System.EventHandler(this.button_add_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(12, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Key：";
            // 
            // textBox_key
            // 
            this.textBox_key.Location = new System.Drawing.Point(54, 13);
            this.textBox_key.Name = "textBox_key";
            this.textBox_key.Size = new System.Drawing.Size(226, 21);
            this.textBox_key.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(12, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "Val：";
            // 
            // textBox_val
            // 
            this.textBox_val.Location = new System.Drawing.Point(54, 42);
            this.textBox_val.Name = "textBox_val";
            this.textBox_val.Size = new System.Drawing.Size(226, 21);
            this.textBox_val.TabIndex = 2;
            // 
            // button_edit
            // 
            this.button_edit.Location = new System.Drawing.Point(286, 42);
            this.button_edit.Name = "button_edit";
            this.button_edit.Size = new System.Drawing.Size(75, 23);
            this.button_edit.TabIndex = 0;
            this.button_edit.Text = "修改";
            this.button_edit.UseVisualStyleBackColor = true;
            this.button_edit.Click += new System.EventHandler(this.button_edit_Click);
            // 
            // button_del
            // 
            this.button_del.Location = new System.Drawing.Point(286, 81);
            this.button_del.Name = "button_del";
            this.button_del.Size = new System.Drawing.Size(75, 23);
            this.button_del.TabIndex = 0;
            this.button_del.Text = "删除";
            this.button_del.UseVisualStyleBackColor = true;
            this.button_del.Click += new System.EventHandler(this.button_del_Click);
            // 
            // button_queryS
            // 
            this.button_queryS.Location = new System.Drawing.Point(54, 81);
            this.button_queryS.Name = "button_queryS";
            this.button_queryS.Size = new System.Drawing.Size(114, 23);
            this.button_queryS.TabIndex = 0;
            this.button_queryS.Text = "根据Key获取";
            this.button_queryS.UseVisualStyleBackColor = true;
            this.button_queryS.Click += new System.EventHandler(this.button_queryS_Click);
            // 
            // button_queryAll
            // 
            this.button_queryAll.Location = new System.Drawing.Point(183, 81);
            this.button_queryAll.Name = "button_queryAll";
            this.button_queryAll.Size = new System.Drawing.Size(97, 23);
            this.button_queryAll.TabIndex = 0;
            this.button_queryAll.Text = "获取所有";
            this.button_queryAll.UseVisualStyleBackColor = true;
            this.button_queryAll.Click += new System.EventHandler(this.button_queryAll_Click);
            // 
            // textBox_msg
            // 
            this.textBox_msg.Location = new System.Drawing.Point(15, 155);
            this.textBox_msg.Multiline = true;
            this.textBox_msg.Name = "textBox_msg";
            this.textBox_msg.Size = new System.Drawing.Size(346, 247);
            this.textBox_msg.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(12, 127);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 16);
            this.label3.TabIndex = 1;
            this.label3.Text = "结果";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(54, 120);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(114, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "清屏";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(183, 120);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(97, 23);
            this.button2.TabIndex = 0;
            this.button2.Text = "添加多条(Key=list)";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button_delall
            // 
            this.button_delall.Location = new System.Drawing.Point(286, 120);
            this.button_delall.Name = "button_delall";
            this.button_delall.Size = new System.Drawing.Size(75, 23);
            this.button_delall.TabIndex = 0;
            this.button_delall.Text = "删除多条";
            this.button_delall.UseVisualStyleBackColor = true;
            this.button_delall.Click += new System.EventHandler(this.button_delall_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(373, 414);
            this.Controls.Add(this.textBox_msg);
            this.Controls.Add(this.textBox_val);
            this.Controls.Add(this.textBox_key);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button_queryAll);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button_queryS);
            this.Controls.Add(this.button_delall);
            this.Controls.Add(this.button_del);
            this.Controls.Add(this.button_edit);
            this.Controls.Add(this.button_add);
            this.Name = "Form1";
            this.Text = "ServiceStack-redis";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_add;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_key;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_val;
        private System.Windows.Forms.Button button_edit;
        private System.Windows.Forms.Button button_del;
        private System.Windows.Forms.Button button_queryS;
        private System.Windows.Forms.Button button_queryAll;
        private System.Windows.Forms.TextBox textBox_msg;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button_delall;
    }
}

