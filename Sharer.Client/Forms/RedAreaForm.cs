using System.Drawing;
using System.Windows.Forms;

namespace Sharer.Client {
	public partial class RedAreaForm : Form {
		public Rectangle SelectedRectangle { get; set; }

		public RedAreaForm() {
			InitializeComponent();

			this.WindowState = FormWindowState.Normal;
			this.StartPosition = FormStartPosition.Manual;
			this.ShowInTaskbar = false;
			// to make form invisible, draw only red area
			this.BackColor = SystemColors.Control;
			this.TransparencyKey = SystemColors.Control;
		}

		private void Form3_Paint(object sender, PaintEventArgs e) {
			if (SelectedRectangle != null && SelectedRectangle.Width > 0 && SelectedRectangle.Height > 0) {
				Pen DashedRedPen = new Pen(Color.Red) {
					Width = 2,
					DashPattern = new float[] { 2, 3 },
				};

				e.Graphics.DrawRectangle(DashedRedPen, SelectedRectangle);
				e.Graphics.FillRectangle(Brushes.Red, SelectedRectangle.Left - 1, SelectedRectangle.Top - 1, 3, 3);
				e.Graphics.FillRectangle(Brushes.Red, SelectedRectangle.Right - 2, SelectedRectangle.Top - 1, 3, 3);
				e.Graphics.FillRectangle(Brushes.Red, SelectedRectangle.Left - 1, SelectedRectangle.Bottom - 2, 3, 3);
			}
		}

		private void RedAreaForm_Load(object sender, System.EventArgs e) {
			this.Location = new Point(SystemInformation.VirtualScreen.Left, SystemInformation.VirtualScreen.Top);
			this.Size = new Size(SystemInformation.VirtualScreen.Width, SystemInformation.VirtualScreen.Height);
		}
	}
}
