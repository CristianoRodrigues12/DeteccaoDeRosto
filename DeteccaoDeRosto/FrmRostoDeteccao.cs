using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace DeteccaoDeRosto
{
    public partial class FrmRostoDeteccao : Form
    {
        static readonly CascadeClassifier cascadeClassifier = new CascadeClassifier("haarcascade_frontalface_alt_tree.xml");
        VideoCapture capture;
        public FrmRostoDeteccao()
        {
            InitializeComponent();
        }

        private void abrirImagemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog() { Multiselect = false, Filter = "JPEG|*.jpg" })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    ProcessarImagem(new Bitmap(Image.FromFile(ofd.FileName)));

                }
            }
        }

        private void iniciarWebCanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(capture != null)
            {
                MessageBox.Show("A Webcam já está iniciada!");
                return;
            }

            capture = new VideoCapture(0);
            capture.ImageGrabbed += Capture_ImageGrabbed;
            capture.Start();
        }

        private void Capture_ImageGrabbed(object sender, EventArgs e)
        {
            try
            {
                Mat m = new Mat();
                capture.Retrieve(m);
                ProcessarImagem(m.Bitmap);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void pararWebCamToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (capture == null)
            {
                MessageBox.Show("A webcam já está parada!");
                return ;
            }

            capture.ImageGrabbed -= Capture_ImageGrabbed;
            capture.Stop();
            capture.Dispose();
            capture = null;
            picImagem.Image = null;
        }
        private void ProcessarImagem(Bitmap bitmap)
        {
            Image<Bgr, byte> greyImage = new Image<Bgr, byte>(bitmap);
            Rectangle[] rectanles = cascadeClassifier.DetectMultiScale(greyImage, 1.2, 1);
            foreach (var rectangle in rectanles)
            {
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    using (Pen pen = new Pen(Color.Blue, 3))
                    {
                        graphics.DrawRectangle(pen, rectangle);
                    }
                }
            }
            picImagem.Image = bitmap;
        }

        private void picImagem_Click(object sender, EventArgs e)
        {

        }
    }
}
