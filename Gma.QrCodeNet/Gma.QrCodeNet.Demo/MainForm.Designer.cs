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
        	this.qrCodeControl1 = new Gma.QrCodeNet.Encoding.Windows.Controls.QrCodeControl();
        	this.btPerformance = new System.Windows.Forms.Button();
        	this.lblZXing = new System.Windows.Forms.Label();
        	this.lblNetResult = new System.Windows.Forms.Label();
        	this.panel1 = new System.Windows.Forms.Panel();
        	this.panel1.SuspendLayout();
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
        	// qrCodeControl1
        	// 
        	this.qrCodeControl1.AutoSize = true;
        	this.qrCodeControl1.Location = new System.Drawing.Point(13, 90);
        	this.qrCodeControl1.Name = "qrCodeControl1";
        	this.qrCodeControl1.Size = new System.Drawing.Size(232, 232);
        	this.qrCodeControl1.TabIndex = 5;
        	this.qrCodeControl1.Text = "qrCodeControl1";
        	// 
        	// btPerformance
        	// 
        	this.btPerformance.Location = new System.Drawing.Point(13, 15);
        	this.btPerformance.Name = "btPerformance";
        	this.btPerformance.Size = new System.Drawing.Size(75, 23);
        	this.btPerformance.TabIndex = 6;
        	this.btPerformance.Text = "Test";
        	this.btPerformance.UseVisualStyleBackColor = true;
        	this.btPerformance.Click += new System.EventHandler(this.BtPerformanceClick);
        	// 
        	// lblZXing
        	// 
        	this.lblZXing.Location = new System.Drawing.Point(154, 20);
        	this.lblZXing.Name = "lblZXing";
        	this.lblZXing.Size = new System.Drawing.Size(100, 23);
        	this.lblZXing.TabIndex = 7;
        	this.lblZXing.Text = "Result 1";
        	// 
        	// lblNetResult
        	// 
        	this.lblNetResult.Location = new System.Drawing.Point(329, 20);
        	this.lblNetResult.Name = "lblNetResult";
        	this.lblNetResult.Size = new System.Drawing.Size(100, 23);
        	this.lblNetResult.TabIndex = 8;
        	this.lblNetResult.Text = "Result 2";
        	// 
        	// panel1
        	// 
        	this.panel1.Controls.Add(this.btPerformance);
        	this.panel1.Controls.Add(this.lblNetResult);
        	this.panel1.Controls.Add(this.lblZXing);
        	this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
        	this.panel1.Location = new System.Drawing.Point(0, 379);
        	this.panel1.Name = "panel1";
        	this.panel1.Size = new System.Drawing.Size(509, 47);
        	this.panel1.TabIndex = 9;
        	// 
        	// MainForm
        	// 
        	this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        	this.ClientSize = new System.Drawing.Size(509, 426);
        	this.Controls.Add(this.panel1);
        	this.Controls.Add(this.qrCodeControl1);
        	this.Controls.Add(this.checkBoxArtistic);
        	this.Controls.Add(this.buttonSave);
        	this.Controls.Add(this.textBoxInput);
        	this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
        	this.Name = "MainForm";
        	this.Text = "QrCode.Net Demo";
        	this.panel1.ResumeLayout(false);
        	this.ResumeLayout(false);
        	this.PerformLayout();
        }
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblNetResult;
        private System.Windows.Forms.Label lblZXing;
        private System.Windows.Forms.Button btPerformance;
        private Gma.QrCodeNet.Encoding.Windows.Controls.QrCodeControl qrCodeControl1;

        #endregion

        private System.Windows.Forms.TextBox textBoxInput;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.CheckBox checkBoxArtistic;
    }
}

