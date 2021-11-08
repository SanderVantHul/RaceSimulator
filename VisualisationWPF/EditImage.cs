using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Color = System.Drawing.Color;

namespace VisualisationWPF
{
    public static class EditImage
    {
        private static Dictionary<string, Bitmap> _imageCache = new Dictionary<string, Bitmap>();

        public static Bitmap GetBitmap(string imageUrl)
        {
            if (_imageCache.ContainsKey(imageUrl))
            {
                return _imageCache[imageUrl];
            }

            var temp = new Bitmap(imageUrl);
            _imageCache.Add(imageUrl, temp);
            return temp;
        }

        public static void ClearCache()
        {
            _imageCache.Clear();
        }

        public static Bitmap CreateBitmap(int width, int height)
        {
            string key = "empty";

            if (!_imageCache.ContainsKey(key))
            {
                //voeg toe aan dictionary
                var background = new Bitmap(width, height);
                _imageCache.Add(key, background);

                //verander de kleur
                var color = new SolidBrush(Color.GreenYellow).Color;
                Graphics.FromImage(_imageCache[key]).Clear(color);
            }
            return (Bitmap)GetBitmap(key).Clone();
        }

        public static BitmapSource CreateBitmapSourceFromGdiBitmap(Bitmap bitmap)
        {
            if (bitmap == null)
                throw new ArgumentNullException("bitmap");

            var rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);

            var bitmapData = bitmap.LockBits(
                rect,
                ImageLockMode.ReadWrite,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            try
            {
                var size = (rect.Width * rect.Height) * 4;

                return BitmapSource.Create(
                    bitmap.Width,
                    bitmap.Height,
                    bitmap.HorizontalResolution,
                    bitmap.VerticalResolution,
                    PixelFormats.Bgra32,
                    null,
                    bitmapData.Scan0,
                    size,
                    bitmapData.Stride);
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }
        }
    }
}