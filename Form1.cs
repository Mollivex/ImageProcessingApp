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
        private List<Bitmap> _bitmaps = new List<Bitmap>();
        private Random _random = new Random();

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
            var pixels = GetPixels(bitmap);

            // Number of pixel for 1% of trackbar
            var pixelInStep = (bitmap.Height * bitmap.Width) / 100;
            var currentPixelSet = new List<Pixel>(pixels.Count - pixelInStep);

            // Choose random number of pixels for current trackBar % status
            for (int i = 1; i < trackBar1.Maximum; i++)
            {
                for (int j = 0; j<pixelInStep; j++)
                {
                    // random number of pixels from bitmap
                    var index = _random.Next(pixels.Count);
                    // place those pixels in list
                    currentPixelSet.Add(pixels[index]);
                    // delete those number of pixels from main collection
                    pixels.RemoveAt(index);
                }
                // create a new image (bitmap)
                var currentBitmap = new Bitmap(bitmap.Height, bitmap.Width);

                // processing
                foreach (var pixel in currentPixelSet)
                    currentBitmap.SetPixel(pixel.Point.X, pixel.Point.Y, pixel.Color);
                
                Text = $"{i} %";
            }

            // add full current image for the last iteration (100%)
            _bitmaps.Add(bitmap);
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
