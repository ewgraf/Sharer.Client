using System.Drawing;
using System.Windows.Forms;

namespace Sharer.Client
{
	public partial class RedAreaForm : Form
	{
		public Rectangle SelectedRectangle;

		public RedAreaForm()
		{
			InitializeComponent();

			// to make form invisible, draw only red area
			this.BackColor = SystemColors.Control;
			this.TransparencyKey = SystemColors.Control;
			this.ShowInTaskbar = false;
		}

		private void Form3_Paint(object sender, PaintEventArgs e)
		{
			if (SelectedRectangle != null && SelectedRectangle.Width > 0 && SelectedRectangle.Height > 0) {
				Pen DashedRedPen = new Pen(Color.Red) {
					Width = 2,
					DashPattern = new float[] { 2, 3 },
				};

				e.Graphics.DrawRectangle(DashedRedPen, SelectedRectangle);
				e.Graphics.FillRectangle(Brushes.Red, SelectedRectangle.Left - 1, SelectedRectangle.Top - 1, 3, 3);
				e.Graphics.FillRectangle(Brushes.Red, SelectedRectangle.Right - 2, SelectedRectangle.Top - 1, 3, 3);
				e.Graphics.FillRectangle(Brushes.Red, SelectedRectangle.Left - 1, SelectedRectangle.Bottom - 2, 3, 3);
				//e.Graphics.FillRectangle(selectionBrush, Rect);
			}
		}
	}
}
