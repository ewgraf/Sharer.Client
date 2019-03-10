using System;
using System.Drawing;
using System.Windows.Forms;

namespace Sharer.Client {
	public partial class AreaSelectionForm : Form {
		public Rectangle SelectedRectangle;

		private bool _drawingRectangleMode;
		private bool _drawingRectangleNow;
		private Point _rectStartPoint;
		private RedAreaForm _formForDrawingRedArea;

		public AreaSelectionForm() {
			InitializeComponent();

			this.WindowState = FormWindowState.Normal;
			this.StartPosition = FormStartPosition.Manual;
			this.TopMost = true;
			this.Cursor = Cursors.Cross;
			this.ShowInTaskbar = false;
			// to make form almost invisible, but still clickable
			this.BackColor = Color.Black;
			this.Opacity = 0.005d;
		}

		private void Form2_Load(object sender, EventArgs e) {
			this.Location = new Point(SystemInformation.VirtualScreen.Left, SystemInformation.VirtualScreen.Top);
			this.Size = new Size(SystemInformation.VirtualScreen.Width, SystemInformation.VirtualScreen.Height);

			_drawingRectangleMode = true;
			_drawingRectangleNow = false;
            
			SelectedRectangle = new Rectangle();
            
			_formForDrawingRedArea = new RedAreaForm();
			_formForDrawingRedArea.Show();
		}

		private void Form2_MouseDown(object sender, MouseEventArgs e) {
			if (e.Button == MouseButtons.Right) {
				SelectedRectangle = Rectangle.Empty;
				_formForDrawingRedArea.Close();
				this.Close();
			}
			if (_drawingRectangleMode) {
				if (e.Button == MouseButtons.Left) {
					_rectStartPoint = e.Location;
					_drawingRectangleNow = true;
				}
			}
		}

		private void Form2_MouseUp(object sender, MouseEventArgs e) {
			if (e.Button == MouseButtons.Left && _drawingRectangleMode == true) {
				this.Cursor = Cursors.Default;

				_drawingRectangleMode = false;
				_drawingRectangleNow = false;

				_formForDrawingRedArea.Close();

				this.DialogResult = DialogResult.OK;
				this.Close();
			}
		}

		private void Form2_MouseMove(object sender, MouseEventArgs e) {
			if (_drawingRectangleMode && _drawingRectangleNow) {
				SelectedRectangle.Location = new Point(
					Math.Min(_rectStartPoint.X, e.X),
					Math.Min(_rectStartPoint.Y, e.Y));
				SelectedRectangle.Size = new Size(
					Math.Abs(_rectStartPoint.X - e.X),
					Math.Abs(_rectStartPoint.Y - e.Y));
				_formForDrawingRedArea.SelectedRectangle = SelectedRectangle;
				_formForDrawingRedArea.Invalidate();
			}
		}

		private void Form2_FormClosing(object sender, FormClosingEventArgs e) {
			this._formForDrawingRedArea.Close();
		}

		private void Form2_KeyDown(object sender, KeyEventArgs e) {
			if (!(e.Control && e.Shift && e.KeyCode == Keys.D3)) {
				this.DialogResult = DialogResult.Cancel;
				this.Close();
			}
		}
	}
}
