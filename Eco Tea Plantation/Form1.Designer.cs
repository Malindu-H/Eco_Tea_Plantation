
namespace Eco_Tea_Plantation
{
    partial class Form1
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
            this.ustb = new System.Windows.Forms.TextBox();
            this.pwtb = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.loginbut = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ustb
            // 
            this.ustb.Location = new System.Drawing.Point(219, 78);
            this.ustb.Multiline = true;
            this.ustb.Name = "ustb";
            this.ustb.Size = new System.Drawing.Size(197, 20);
            this.ustb.TabIndex = 0;
            this.ustb.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // pwtb
            // 
            this.pwtb.Location = new System.Drawing.Point(219, 152);
            this.pwtb.Multiline = true;
            this.pwtb.Name = "pwtb";
            this.pwtb.Size = new System.Drawing.Size(197, 20);
            this.pwtb.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(216, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "User Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(216, 125);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Password";
            // 
            // loginbut
            // 
            this.loginbut.Location = new System.Drawing.Point(219, 216);
            this.loginbut.Name = "loginbut";
            this.loginbut.Size = new System.Drawing.Size(197, 23);
            this.loginbut.TabIndex = 4;
            this.loginbut.Text = "LOGIN";
            this.loginbut.UseVisualStyleBackColor = true;
            this.loginbut.Click += new System.EventHandler(this.loginbut_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(141, 292);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "00:00:00";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(561, 402);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.loginbut);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pwtb);
            this.Controls.Add(this.ustb);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox ustb;
        private System.Windows.Forms.TextBox pwtb;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button loginbut;
        private System.Windows.Forms.Label label3;
    }
}

