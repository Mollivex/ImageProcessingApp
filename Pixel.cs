using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessingApp
{
    public class Pixel
    {
        /// <summary>
        /// Represents pixel point coordinates in 2D plane
        /// </summary>
        public Point Point { get; set; }

        /// <summary>
        /// Represents pixel point ARGB color 
        /// </summary>
        public Color Color { get; set; }
    }
}
