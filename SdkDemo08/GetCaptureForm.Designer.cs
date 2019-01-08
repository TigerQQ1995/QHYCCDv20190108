namespace SdkDemo08
{
    partial class GetCaptureForm
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
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.btnThres = new System.Windows.Forms.Button();
            this.thres_value = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnConvert = new System.Windows.Forms.Button();
            this.btnHist = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(280, 75);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(745, 553);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 0;
            this.pictureBox2.TabStop = false;
            // 
            // btnThres
            // 
            this.btnThres.Location = new System.Drawing.Point(31, 104);
            this.btnThres.Name = "btnThres";
            this.btnThres.Size = new System.Drawing.Size(75, 23);
            this.btnThres.TabIndex = 1;
            this.btnThres.Text = "threshold";
            this.btnThres.UseVisualStyleBackColor = true;
            this.btnThres.Click += new System.EventHandler(this.btnThres_Click);
            // 
            // thres_value
            // 
            this.thres_value.Location = new System.Drawing.Point(137, 54);
            this.thres_value.Name = "thres_value";
            this.thres_value.Size = new System.Drawing.Size(100, 21);
            this.thres_value.TabIndex = 2;
            this.thres_value.Text = "20";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(949, 39);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnConvert
            // 
            this.btnConvert.Location = new System.Drawing.Point(31, 170);
            this.btnConvert.Name = "btnConvert";
            this.btnConvert.Size = new System.Drawing.Size(75, 23);
            this.btnConvert.TabIndex = 5;
            this.btnConvert.Text = "convert";
            this.btnConvert.UseVisualStyleBackColor = true;
            this.btnConvert.Click += new System.EventHandler(this.btnConvert_Click);
            // 
            // btnHist
            // 
            this.btnHist.Location = new System.Drawing.Point(31, 54);
            this.btnHist.Name = "btnHist";
            this.btnHist.Size = new System.Drawing.Size(75, 23);
            this.btnHist.TabIndex = 6;
            this.btnHist.Text = "histogram";
            this.btnHist.UseVisualStyleBackColor = true;
            this.btnHist.Click += new System.EventHandler(this.btnHist_Click);
            // 
            // GetCaptureForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1105, 642);
            this.Controls.Add(this.btnHist);
            this.Controls.Add(this.btnConvert);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.thres_value);
            this.Controls.Add(this.btnThres);
            this.Controls.Add(this.pictureBox2);
            this.Name = "GetCaptureForm";
            this.Text = "GetCapture";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button btnThres;
        private System.Windows.Forms.TextBox thres_value;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnConvert;
        private System.Windows.Forms.Button btnHist;
    }
}