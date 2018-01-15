using System;
using System.Drawing;
using System.Windows.Forms;

namespace Sharer.Client
{
	public partial class AreaSelectionForm : Form
	{
		private bool _drawingRectangleMode;
		private bool _drawingRectangleNow;
		private Point RectStartPoint;

		public Rectangle SelectedRectangle;
		private RedAreaForm formForDrawingRedArea;

		public AreaSelectionForm()
		{
			InitializeComponent();

			// to make form almost invisible, but still clickable
			this.BackColor = Color.Black;
			this.Opacity = 0.005d;
			this.TopMost = true;
			this.Cursor = Cursors.Cross;
			this.ShowInTaskbar = false;
		}

		private void Form2_Load(object sender, EventArgs e)
		{
			//this.ShowInTaskbar = false;
			//this.LostFocus += Form2_LostFocus;

			_drawingRectangleMode = true;
			_drawingRectangleNow = false;            
            
			SelectedRectangle = new Rectangle();
            
			formForDrawingRedArea = new RedAreaForm();
			formForDrawingRedArea.Show();
		}

		private void Form2_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right) {
				SelectedRectangle = Rectangle.Empty;
				formForDrawingRedArea.Close();
				this.Close();
			}
			if (_drawingRectangleMode) {
				if (e.Button == MouseButtons.Left) {
					RectStartPoint = e.Location;
					_drawingRectangleNow = true;
				}
			}
		}

		private void Form2_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left && _drawingRectangleMode == true) {
				this.Cursor = Cursors.Default;

				_drawingRectangleMode = false;
				_drawingRectangleNow = false;

				formForDrawingRedArea.Close();

				this.DialogResult = DialogResult.OK;
				this.Close();
			}
		}

		private void Form2_MouseMove(object sender, MouseEventArgs e)
		{
			if (_drawingRectangleMode && _drawingRectangleNow) {
				SelectedRectangle.Location = new Point(
					Math.Min(RectStartPoint.X, e.X),
					Math.Min(RectStartPoint.Y, e.Y));
				SelectedRectangle.Size = new Size(
					Math.Abs(RectStartPoint.X - e.X),
					Math.Abs(RectStartPoint.Y - e.Y));
				formForDrawingRedArea.SelectedRectangle = SelectedRectangle;
				formForDrawingRedArea.Invalidate();
			}
		}

		private void Form2_FormClosing(object sender, FormClosingEventArgs e)
		{
			this.formForDrawingRedArea.Close();
		}

		private void Form2_KeyDown(object sender, KeyEventArgs e)
		{
			if(!(e.Control && e.Shift && e.KeyCode == Keys.D3)) {
				this.DialogResult = DialogResult.Cancel;
				this.Close();
			}
		}
	}
}
