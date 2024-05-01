
namespace Eco_Tea_Plantation
{
    partial class Form4
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
            this.pictureBoxPreview = new System.Windows.Forms.PictureBox();
            this.textBoxQRCodeResult = new System.Windows.Forms.TextBox();
            this.time = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPreview)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxPreview
            // 
            this.pictureBoxPreview.Location = new System.Drawing.Point(400, 45);
            this.pictureBoxPreview.Name = "pictureBoxPreview";
            this.pictureBoxPreview.Size = new System.Drawing.Size(291, 270);
            this.pictureBoxPreview.TabIndex = 0;
            this.pictureBoxPreview.TabStop = false;
            // 
            // textBoxQRCodeResult
            // 
            this.textBoxQRCodeResult.Location = new System.Drawing.Point(400, 345);
            this.textBoxQRCodeResult.Name = "textBoxQRCodeResult";
            this.textBoxQRCodeResult.Size = new System.Drawing.Size(198, 20);
            this.textBoxQRCodeResult.TabIndex = 1;
            // 
            // time
            // 
            this.time.Location = new System.Drawing.Point(400, 382);
            this.time.Name = "time";
            this.time.Size = new System.Drawing.Size(198, 20);
            this.time.TabIndex = 2;
            this.time.TextChanged += new System.EventHandler(this.time_TextChanged);
            // 
            // Form4
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.time);
            this.Controls.Add(this.textBoxQRCodeResult);
            this.Controls.Add(this.pictureBoxPreview);
            this.Name = "Form4";
            this.Text = "Form4";
            this.Load += new System.EventHandler(this.Form4_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPreview)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxPreview;
        private System.Windows.Forms.TextBox textBoxQRCodeResult;
        private System.Windows.Forms.TextBox time;
    }
}