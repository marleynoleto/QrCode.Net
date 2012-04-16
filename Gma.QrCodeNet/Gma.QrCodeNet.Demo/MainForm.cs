using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;

namespace Gma.QrCodeNet.Demo
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            qrCodeImgControl1.DarkBrush = Brushes.Yellow;
            qrCodeImgControl1.LightBrush = Brushes.MediumVioletRed;
            qrCodeGraphicControl1.Text = textBoxInput.Text;
            qrCodeImgControl1.Text = textBoxInput.Text;
        }

        private void textBoxInput_TextChanged(object sender, EventArgs e)
        {
            qrCodeGraphicControl1.Text = textBoxInput.Text;
            qrCodeImgControl1.Text = textBoxInput.Text;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
        	
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = @"PNG (*.png)|*.png|Bitmap (*.bmp)|*.bmp|Encapsuled PostScript (*.eps)|*.eps";
            saveFileDialog.FileName = Path.GetFileName(GetFileNameProposal());
            saveFileDialog.DefaultExt = "png";

            if (saveFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

			if (saveFileDialog.FileName.EndsWith("eps"))
			{
				// Generate the matrix from scratch as it is not reachable from the qrCodeControl1
                //var encoder = new QrEncoder(qrCodeGraphicControl1.ErrorCorrectLevel);
                //QrCode qrCode;
                //encoder.TryEncode(textBoxInput.Text, out qrCode);
                BitMatrix matrix = qrCodeGraphicControl1.GetQrMatrix();

				// Initialize the EPS renderer
				var renderer = new EncapsulatedPostScriptRenderer(
					new FixedModuleSize(6, QuietZoneModules.Two), // Modules size is 2/72th inch (72 points = 1 inch)
                    Color.Black, Color.White);

				using (var file = File.OpenWrite(saveFileDialog.FileName))
				{
                    renderer.WriteToStream(matrix, file);
				}
			}
			else
			{
                using (Bitmap bitmap = new Bitmap(qrCodeGraphicControl1.Size.Width, qrCodeGraphicControl1.Size.Height))
				{
                    qrCodeGraphicControl1.DrawToBitmap(bitmap, new Rectangle(new Point(0, 0), bitmap.Size));
					bitmap.Save(
						saveFileDialog.FileName,
						saveFileDialog.FileName.EndsWith("png")
							? ImageFormat.Png
							: ImageFormat.Bmp);
				}
			}
            //    using (var file = File.CreateText(saveFileDialog.FileName))
            //    {
            //        renderer.WriteToStream(qrCode.Matrix, 6, file); // 72/6 = 12 modules per inch
            //    }
            //}
            //else
            //{
            //    using (Bitmap bitmap = new Bitmap(qrCodeControl1.Size.Width, qrCodeControl1.Size.Height))
            //    {
            //        qrCodeControl1.DrawToBitmap(bitmap, new Rectangle(new Point(0, 0), bitmap.Size));
            //        bitmap.Save(
            //            saveFileDialog.FileName,
            //            saveFileDialog.FileName.EndsWith("png")
            //                ? ImageFormat.Png
            //                : ImageFormat.Bmp);
            //    }
            //}

        }

        private string GetFileNameProposal()
        {
            return textBoxInput.Text.Length > 10 ? textBoxInput.Text.Substring(0, 10) : textBoxInput.Text;
        }

        private void checkBoxArtistic_CheckedChanged(object sender, EventArgs e)
        {
            //qrCodeControl1.Artistic = checkBoxArtistic.Checked;
        }
    }
}
