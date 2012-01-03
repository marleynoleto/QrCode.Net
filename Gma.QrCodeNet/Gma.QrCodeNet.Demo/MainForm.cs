//using System;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using Gma.QrCodeNet.Encoding.Windows.Controls;
using Gma.QrCodeNet.Encoding;
using System.Diagnostics;

namespace Gma.QrCodeNet.Demo
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void textBoxInput_TextChanged(object sender, EventArgs e)
        {
            qrCodeControl1.Text = textBoxInput.Text;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
        	
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = @"PNG (*.png)|*.png|Bitmap (*.bmp)|*.bmp";
            saveFileDialog.FileName = Path.GetFileName(GetFileNameProposal());
            saveFileDialog.DefaultExt = "png";

            if (saveFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            using (Bitmap bitmap = new Bitmap(qrCodeControl1.Size.Width, qrCodeControl1.Size.Height))
            {
                qrCodeControl1.DrawToBitmap(bitmap, new Rectangle(new Point(0, 0), bitmap.Size));
                bitmap.Save(
                    saveFileDialog.FileName,
                    saveFileDialog.FileName.EndsWith("png")
                        ? ImageFormat.Png
                        : ImageFormat.Bmp);
            }

        }

        private string GetFileNameProposal()
        {
            return textBoxInput.Text.Length > 10 ? textBoxInput.Text.Substring(0, 10) : textBoxInput.Text;
        }

        private void checkBoxArtistic_CheckedChanged(object sender, EventArgs e)
        {
            qrCodeControl1.Artistic = checkBoxArtistic.Checked;
        }
        
        void BtPerformanceClick(object sender, EventArgs e)
        {
        	QrEncoder encoder = new QrEncoder();
        	QrCode qrCode;
        	
        	Stopwatch sw = new Stopwatch();
        	
        	string testStr = "QrCode.Netlasdkfjwtazkjv;zlxkhgalkejt;ikcjvlskdf;algkdsa;lskdfja;lskdfjlfkaslkdgjalskjga";
        	
        	sw.Start();
        	for(int numTimes = 0; numTimes < 1000; numTimes++)
        	{
        		qrCode = encoder.ZXEncode(testStr);
        	}
        	sw.Stop();
        	
        	lblZXing.Text = string.Format("ZX Elapsed={0}", sw.ElapsedMilliseconds);
        	
        	sw.Reset();
        	
        	sw.Start();
        	for(int numTimes = 0; numTimes < 1000; numTimes++)
        	{
        		qrCode = encoder.NEncode(testStr);
        	}
        	sw.Stop();
        	
        	lblNetResult.Text = string.Format("Elapsed={0}", sw.ElapsedMilliseconds);
        	
        	
        }
    }
}
