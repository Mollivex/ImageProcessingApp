using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageProcessingApp
{
    public partial class Form1 : Form
    {
        public List<Bitmap> _bitmaps = new List<Bitmap>();
        public Form1()
        {
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // clear the last one image
                pictureBox1.Image = null;
                _bitmaps.Clear();

                // declare a new variable where our new image will save
                var bitmap = new Bitmap(openFileDialog1.FileName);
            }
        }

        /// <summary>
        /// Processing opened image
        /// </summary>
        private void RunProcessing(Bitmap bitmap)
        {

        }

        /// <summary>
        /// Get information about each pixel of image
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        private List<Pixel> GetPixels(Bitmap bitmap)
        {
            var pixels = new List<Pixel>(bitmap.Height * bitmap.Width);

            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    pixels.Add(new Pixel()
                    {
                        Color = bitmap.GetPixel(x, y),
                        Point = new Point() { X = x, Y = y }
                    });
                }
            }
            return pixels;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            // exception handlings if the list of elements _bitmap is null or exist, but doesn't has any elements
            if (_bitmaps == null || _bitmaps.Count == 0)
                return;

            pictureBox1.Image = _bitmaps[trackBar1.Value - 1];
        }
    }
}
