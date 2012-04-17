namespace Gma.QrCodeNet.Demo
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.textBoxInput = new System.Windows.Forms.TextBox();
            this.buttonSave = new System.Windows.Forms.Button();
            this.checkBoxArtistic = new System.Windows.Forms.CheckBox();
            this.qrCodeGraphicControl1 = new Gma.QrCodeNet.Encoding.Windows.Forms.QrCodeGraphicControl();
            this.qrCodeImgControl1 = new Gma.QrCodeNet.Encoding.Windows.Forms.QrCodeImgControl();
            ((System.ComponentModel.ISupportInitialize)(this.qrCodeImgControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // textBoxInput
            // 
            this.textBoxInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxInput.Location = new System.Drawing.Point(12, 13);
            this.textBoxInput.Multiline = true;
            this.textBoxInput.Name = "textBoxInput";
            this.textBoxInput.Size = new System.Drawing.Size(443, 38);
            this.textBoxInput.TabIndex = 0;
            this.textBoxInput.Text = "QrCode.Net";
            this.textBoxInput.TextChanged += new System.EventHandler(this.textBoxInput_TextChanged);
            // 
            // buttonSave
            // 
            this.buttonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSave.Image = global::Gma.QrCodeNet.Demo.Properties.Resources.save;
            this.buttonSave.Location = new System.Drawing.Point(461, 13);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(36, 38);
            this.buttonSave.TabIndex = 2;
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // checkBoxArtistic
            // 
            this.checkBoxArtistic.AutoSize = true;
            this.checkBoxArtistic.Location = new System.Drawing.Point(13, 56);
            this.checkBoxArtistic.Name = "checkBoxArtistic";
            this.checkBoxArtistic.Size = new System.Drawing.Size(57, 17);
            this.checkBoxArtistic.TabIndex = 4;
            this.checkBoxArtistic.Text = "Artistic";
            this.checkBoxArtistic.UseVisualStyleBackColor = true;
            this.checkBoxArtistic.CheckedChanged += new System.EventHandler(this.checkBoxArtistic_CheckedChanged);
            // 
            // qrCodeGraphicControl1
            // 
            this.qrCodeGraphicControl1.BackColor = System.Drawing.SystemColors.Control;
            this.qrCodeGraphicControl1.ErrorCorrectLevel = Gma.QrCodeNet.Encoding.ErrorCorrectionLevel.M;
            this.qrCodeGraphicControl1.Location = new System.Drawing.Point(13, 92);
            this.qrCodeGraphicControl1.Name = "qrCodeGraphicControl1";
            this.qrCodeGraphicControl1.QuietZoneModule = Gma.QrCodeNet.Encoding.Windows.Render.QuietZoneModules.Two;
            this.qrCodeGraphicControl1.Size = new System.Drawing.Size(442, 153);
            this.qrCodeGraphicControl1.TabIndex = 5;
            // 
            // qrCodeImgControl1
            // 
            this.qrCodeImgControl1.ErrorCorrectLevel = Gma.QrCodeNet.Encoding.ErrorCorrectionLevel.M;
            this.qrCodeImgControl1.Image = ((System.Drawing.Image)(resources.GetObject("qrCodeImgControl1.Image")));
            this.qrCodeImgControl1.Location = new System.Drawing.Point(13, 303);
            this.qrCodeImgControl1.Name = "qrCodeImgControl1";
            this.qrCodeImgControl1.QuietZoneModule = Gma.QrCodeNet.Encoding.Windows.Render.QuietZoneModules.Two;
            this.qrCodeImgControl1.Size = new System.Drawing.Size(442, 192);
            this.qrCodeImgControl1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.qrCodeImgControl1.TabIndex = 6;
            this.qrCodeImgControl1.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(509, 519);
            this.Controls.Add(this.qrCodeImgControl1);
            this.Controls.Add(this.qrCodeGraphicControl1);
            this.Controls.Add(this.checkBoxArtistic);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.textBoxInput);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "QrCode.Net Demo";
            ((System.ComponentModel.ISupportInitialize)(this.qrCodeImgControl1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxInput;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.CheckBox checkBoxArtistic;
        private Encoding.Windows.Forms.QrCodeGraphicControl qrCodeGraphicControl1;
        private Encoding.Windows.Forms.QrCodeImgControl qrCodeImgControl1;
    }
}

