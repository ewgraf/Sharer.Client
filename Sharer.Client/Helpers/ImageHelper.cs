using System.Drawing;
using System.Drawing.Drawing2D;

namespace Sharer.Client.Helpers {
	public static class ImageHelper {
		public static Image DrawString(string s, Font font = null, int wshift = 0, int hshift = 0) {
			var image = new Bitmap(16, 16);
			using (Graphics g = Graphics.FromImage(image)) {
				var rect = new Rectangle(0, 0, image.Width, image.Height);
				var brush = new LinearGradientBrush(new Point(0, 8), new Point(16, 8), Color.FromArgb(255, 251, 251, 251), Color.FromArgb(255, 243, 243, 243));
				g.FillRectangle(brush, rect);
				rect = new Rectangle(0, 0, image.Width - wshift, image.Height - hshift);
				var format = new StringFormat {
					LineAlignment = StringAlignment.Center,
					Alignment = StringAlignment.Center
				};
				if (font == null) {
					font = new Font("Courier New", 13);
				}
				g.DrawString(s, font, Brushes.Black, rect, format);
			}
			return image;
		}

		public static Image Crop(this Image image, Rectangle area) {
			Bitmap target = new Bitmap(area.Width, area.Height);
			using (Graphics g = Graphics.FromImage(target)) {
				g.DrawImage(image, new Rectangle(0, 0, target.Width, target.Height), area, GraphicsUnit.Pixel);
			}
			return target;
		}
	}
}
