using Sharer.Client.Helpers;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Sharer.Client.Forms {
	public partial class EditCaptureForm : Form {
		private enum Tools {
			Arrow,
			Rectangle,
			None
		}

		private Rectangle _area;

		private Tools _selectedTool = Tools.None;
		private bool _drawing;
		private Point _startPoint;
		private Point _endPoint;

		public Image Image { get; set; }

		public EditCaptureForm(Image screenshot, Rectangle area) {
			InitializeComponent();

			this.MaximumSize = SystemInformation.PrimaryMonitorSize;
			this.AutoScroll = false;
			this.pictureBox1.Image = screenshot;
			this.pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;

			_area = area;

			// hack to allow buttons be transparent
			//this.buttonDrawArrow.Parent = this.Parent;
			//this.buttonDrawRectangle.Parent = this;
			//this.buttonUpload.Parent = this.pictureBox1;
		}

		private void EditCaptureForm_Load(object sender, EventArgs e) {
			this.Location = new Point(_area.Location.X - 8, _area.Location.Y - 31);
			this.Size = new Size(_area.Size.Width + 16, _area.Size.Height + 38);
			//if (this.pictureBox1.Width >= this.MaximumSize.Width - 54 || this.pictureBox1.Height >= this.MaximumSize.Height - 71) {
			//	this.Location = new Point(0);
			//	this.SetAutoScrollMargin(this.pictureBox1.Width - 54, this.pictureBox1.Height - 71);
			//} else {
			//	this.Location = new Point(_area.X - 8, _area.Y - 31);
			//}
		}

		private void EditCaptureForm_KeyDown(object sender, KeyEventArgs e) {
			if (e.KeyCode == Keys.Escape) {
				CloseCancel();
			}
		}

		private void EditCaptureForm_Resize(object sender, EventArgs e) {
		}

		private void EditCaptureForm_LocationChanged(object sender, EventArgs e) {
			this.pictureBox1.Location = new Point(-this.Location.X - 8, -this.Location.Y - 31);
		}

		#region Buttons

		private void buttonUpload_Click(object sender, EventArgs e) {
			var area = new Rectangle(this.Location.X + 8, this.Location.Y + 31, this.ClientRectangle.Width, this.ClientRectangle.Height);
			Image = pictureBox1.Image.Crop(area);
			CloseOK();
		}

		private void buttonDrawArrow_Click(object sender, EventArgs e) {
			_selectedTool = Tools.Arrow;
		}

		private void buttonDrawRectangle_Click(object sender, EventArgs e) {
			_selectedTool = Tools.Rectangle;
		}

		#endregion

		private void pictureBox1_MouseDown(object sender, MouseEventArgs e) {
			_startPoint = e.Location;
			_drawing = true;
		}

		private void pictureBox1_MouseMove(object sender, MouseEventArgs e) {
			if (_drawing) {
				_endPoint = e.Location;
			}
			this.pictureBox1.Invalidate();
		}

		private void pictureBox1_MouseUp(object sender, MouseEventArgs e) {
			if (_drawing) {
				_drawing = false;
			}

			DrawTool(Graphics.FromImage(pictureBox1.Image), _selectedTool);
			_startPoint = new Point(0);
			_endPoint = new Point(0);
		}

		private void pictureBox1_Paint(object sender, PaintEventArgs e) {
			if (_drawing == false || _startPoint.IsEmpty || _endPoint.IsEmpty || _selectedTool == Tools.None) {
				return;
			}

			DrawTool(e.Graphics, _selectedTool);
		}

		private void CloseOK() {
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void CloseCancel() {
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void DrawTool(Graphics g, Tools tool) {
			if (tool == Tools.Rectangle) {
				Pen pen = new Pen(Color.FromArgb(255, 255, 0, 0), 2);
				var area = new Rectangle(
					Math.Min(_startPoint.X, _endPoint.X),
					Math.Min(_startPoint.Y, _endPoint.Y),
					Math.Abs(_endPoint.X - _startPoint.X),
					Math.Abs(_endPoint.Y - _startPoint.Y)
				);
				g.DrawRectangle(pen, area);
			} else if (tool == Tools.Arrow) {
				Pen pen = new Pen(Color.FromArgb(255, 255, 0, 0), 8);
				pen.StartCap = LineCap.Round;
				pen.EndCap = LineCap.ArrowAnchor;
				g.DrawLine(pen, _startPoint, _endPoint);
			}
		}
	}
}
