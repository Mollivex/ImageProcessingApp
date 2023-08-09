using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

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

        private async void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await Task.Run(() => this.Close());
        }

        private async void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var sw = Stopwatch.StartNew();

                // Hide UI access during app activity
                menuStrip1.Enabled = trackBar1.Enabled = false;
                // Clear the last one image
                pictureBox1.Image = null;
                _bitmaps.Clear();
                // Declare a new variable where our new image will save
                var bitmap = new Bitmap(openFileDialog1.FileName);
                // Run image processing
                await Task.Run(() => { RunProcessing(bitmap); });
                // Enable UI access after app activity is done
                menuStrip1.Enabled = trackBar1.Enabled = Enabled;

                sw.Stop();
                Text = sw.Elapsed.ToString();
            }
        }

        /// <summary>
        /// Processing opened image
        /// </summary>
        private void RunProcessing(Bitmap bitmap)
        {
            var pixels = GetPixels(bitmap);

            // Number of pixel for 1% of trackbar
            var pixelsInStep = (bitmap.Width * bitmap.Height) / 100;
            var currentPixelSet = new List<Pixel>(pixels.Count - pixelsInStep);

            // Choose random number of pixels for current trackBar % status
            for (int i = 1; i < trackBar1.Maximum; i++)
            {
                for (int j = 0; j < pixelsInStep; j++)
                {
                    // Random number of pixels from bitmap
                    var index = _random.Next(pixels.Count);
                    // Place those pixels in list
                    currentPixelSet.Add(pixels[index]);
                    // Delete those number of pixels from main collection
                    pixels.RemoveAt(index);
                }
                // Create a new image (bitmap)
                var currentBitmap = new Bitmap(bitmap.Width, bitmap.Height);
                // Processing new bitmap
                foreach (var pixel in currentPixelSet)
                    currentBitmap.SetPixel(pixel.Point.X, pixel.Point.Y, pixel.Color);
                
                // Add new bitmap in the current collection
                _bitmaps.Add(currentBitmap);

                // Update UI element through delegate
                this.Invoke(new Action(() =>
                {
                    Text = $"{i} %";
                }));
            }

            // Add full current image for the last iteration (100%)
            _bitmaps.Add(bitmap);
        }

        /// <summary>
        /// Get information about each pixel of image
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        private List<Pixel> GetPixels(Bitmap bitmap)
        {
            var pixels = new List<Pixel>(bitmap.Width * bitmap.Height);

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
            // Exception handlings if the list of elements _bitmap is null or exist, but doesn't has any elements
            if (_bitmaps == null || _bitmaps.Count == 0)
                return;

            pictureBox1.Image = _bitmaps[trackBar1.Value - 1 ];
        }
    }
}
