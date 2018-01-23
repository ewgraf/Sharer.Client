using Sharer.Client.Helpers;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Sharer.Client.Forms {
	public partial class EditCaptureForm : Form {
		private enum Tools {
			None,
			Move,
			Arrow,
			Rectangle,
			ResizeRight,
			ResizeTopRight,
			ResizeTop,
			ResizeTopLeft,
			ResizeLeft,
			ResizeBottomLeft,
			ResizingBottom,
			ResizingBottomRight,
		}

		private Rectangle _area;
		private Tools _selectedTool = Tools.Move;
		private bool _drawing;
		private Point _startPoint;
		private Point _endPoint;

		public Image Image { get; set; }

		public EditCaptureForm(Image screenshot, Rectangle area) {
			InitializeComponent();

			this.MaximumSize = SystemInformation.PrimaryMonitorSize;
			this.AutoScroll = false;
			this.pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
			this.pictureBox1.BackgroundImage = screenshot;
			this.pictureBox1.Size = screenshot.Size;
			this.pictureBox1.Image = new Bitmap(screenshot.Width, screenshot.Height);
			using (Graphics g = Graphics.FromImage(pictureBox1.Image)) {
				g.Clear(Color.Transparent);
			}

			_area = area;

			// hack to allow buttons be transparent
			//this.buttonDrawArrow.Parent = this.Parent;
			//this.buttonDrawRectangle.Parent = this;
			//this.buttonUpload.Parent = this.pictureBox1;
		}

		private void EditCaptureForm_Load(object sender, EventArgs e) {
			this.Location = new Point(_area.Location.X, _area.Location.Y);
			this.Size = new Size(_area.Size.Width, _area.Size.Height);
			//this.pictureBox1.Location = new Point(
			//	this.pictureBox1.Location.X + 8,
			//	this.pictureBox1.Location.Y + 31
			//);
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
			//-------------------------------------------------------------
			using (Graphics g = Graphics.FromImage(pictureBox1.Image)) {
				g.Clear(Color.Transparent);
				g.DrawRectangle(Pens.Red, this.Location.X, this.Location.Y, this.ClientRectangle.Width - 1, this.ClientRectangle.Height - 1);
			}
			this.pictureBox1.Invalidate();
		}

		private void EditCaptureForm_LocationChanged(object sender, EventArgs e) {
			this.pictureBox1.Location = new Point(-this.Location.X, -this.Location.Y);
		}

		#region Buttons

		private void buttonUpload_Click(object sender, EventArgs e) {
			var area = new Rectangle(this.Location.X, this.Location.Y, this.ClientRectangle.Width, this.ClientRectangle.Height);
			Image = pictureBox1.BackgroundImage.Crop(area);
			CloseOK();
		}

		private void buttonDrawArrow_Click(object sender, EventArgs e) {
			_selectedTool = Tools.Arrow;
		}

		private void buttonDrawRectangle_Click(object sender, EventArgs e) {
			_selectedTool = Tools.Rectangle;
		}

		#endregion

		private Point _startCursorPoint;
		private Point _startFormPoint;

		private void pictureBox1_MouseDown(object sender, MouseEventArgs e) {
			_startPoint = e.Location;
			_drawing = true;
			_startCursorPoint = Cursor.Position;
			_startFormPoint = this.Location;
			//_startFormPoint.X += 8;
			//_startFormPoint.Y += 31;
			Point p = this.PointToClient(Cursor.Position);
			//p.Y -= 31;
			//p.X -= 8;
			if ((p.X > (this.Width - indent)) && p.Y < indent) { // ↗
				_selectedTool = Tools.ResizeTopRight;
			} else if (p.X < indent && p.Y < indent) { // ↖
				_selectedTool = Tools.ResizeTopLeft;
			} else if (p.X < indent && (p.Y > (this.Height - indent))) { // ↙
				_selectedTool = Tools.ResizeBottomLeft;
			} else if ((p.X > (this.Width - indent) && (p.Y > (this.Height - indent)))) { // ↘
				_selectedTool = Tools.ResizingBottomRight;
			} else if (p.X > (this.Width - indent)) { // →
				_selectedTool = Tools.ResizeRight;
			} else if (p.Y < indent) { // ↑
				_selectedTool = Tools.ResizeTop;
			} else if (p.X < indent) { // ←
				_selectedTool = Tools.ResizeLeft;
			} else if (p.Y > (this.Height - indent)) { // ↓
				_selectedTool = Tools.ResizingBottom;
			} else if (_selectedTool != Tools.Arrow && _selectedTool != Tools.Rectangle) {
				_selectedTool = Tools.Move;
			}
		}

		const int indent = 10;

		private bool _movingPictureBox = false;

		private void pictureBox1_MouseMove(object sender, MouseEventArgs e) {
			//Debug.WriteLine(this.pictureBox1.Location);
			//if (_movingPictureBox) {
			//	_movingPictureBox = false;
			//	return;
			//}

			Point p = this.PointToClient(Cursor.Position);
			if (_selectedTool == Tools.Arrow) {
				this.pictureBox1.Cursor = Cursors.UpArrow;
			} else if (_selectedTool == Tools.Rectangle) {
				this.pictureBox1.Cursor = Cursors.Cross;
			} else if ((p.X > (this.Width - indent)) && p.Y < indent) { // ↗
				this.pictureBox1.Cursor = Cursors.SizeNESW;
			} else if (p.X < indent && p.Y < indent) {
				this.pictureBox1.Cursor = Cursors.SizeNWSE; // ↖
			} else if (p.X < indent && (p.Y > (this.Height - indent))) { // ↙
				this.pictureBox1.Cursor = Cursors.SizeNESW;
			} else if ((p.X > (this.Width - indent) && (p.Y > (this.Height - indent)))) { // ↘
				this.pictureBox1.Cursor = Cursors.SizeNWSE;
			} else if (p.X > (this.Width - indent)) {
				this.pictureBox1.Cursor = Cursors.SizeWE; // →
			} else if (p.Y < indent) {
				this.pictureBox1.Cursor = Cursors.SizeNS; // ↑
			} else if (p.X < indent) { // ←
				this.pictureBox1.Cursor = Cursors.SizeWE;
			} else if (p.Y > (this.Height - indent)) {
				this.pictureBox1.Cursor = Cursors.SizeNS; // ↓
			} else {
				this.pictureBox1.Cursor = Cursors.SizeAll;
			}
			if (_drawing) {
				_endPoint = e.Location;
				//_endPoint.X -= 8;
				//_endPoint.Y -= 31;
				if (_selectedTool == Tools.ResizeTopRight) { // ↗
					this.Location = new Point(this.Location.X, this.Location.Y + p.Y);
					this.Size = new Size(p.X, this.Height - p.Y);
				} else if (_selectedTool == Tools.ResizeTopLeft) { // ↖
					this.Location = new Point(this.Location.X + p.X, this.Location.Y + p.Y);
					this.Size = new Size(this.Width - p.X, this.Height - p.Y);
				} else if (_selectedTool == Tools.ResizeBottomLeft) { // ↙
					this.Location = new Point(this.Location.X + p.X, this.Location.Y);
					this.Size = new Size(this.Width - p.X, p.Y);
				} else if (_selectedTool == Tools.ResizingBottomRight) { // ↘
					this.Size = new Size(p.X, p.Y);
				} else if (_selectedTool == Tools.ResizeRight) { // →
					this.Size = new Size(p.X, this.Size.Height);
				} else if (_selectedTool == Tools.ResizeTop) { // ↑
					this.Location = new Point(this.Location.X, this.Location.Y + p.Y);
					this.Size = new Size(this.Width, this.Height - p.Y);
				} else if (_selectedTool == Tools.ResizeLeft) { // ←
					this.Location = new Point(this.Location.X + p.X, this.Location.Y);
					this.Size = new Size(this.Width - p.X, this.Height);
				} else if (_selectedTool == Tools.ResizingBottom) { // ↓
					this.Size = new Size(this.Width, p.Y);
				} else if (_selectedTool == Tools.Move) {
					Point dif = Point.Subtract(Cursor.Position, new Size(_startCursorPoint));
					this.Location = Point.Add(_startFormPoint, new Size(dif));
					//_movingPictureBox = true;
					//this.pictureBox1.Location = new Point(
					//	this.pictureBox1.Location.X + 8,
					//	this.pictureBox1.Location.Y + 31
					//);
					//this.Location = new Point(this.Location.X + 8, this.Location.Y + 31);
					//this.Location.X += 8;
					//this.Location.Y += 31;
				}
			}

			// -------------------------------------------------------------
			using (Graphics g = Graphics.FromImage(pictureBox1.Image)) {
				g.Clear(Color.Transparent);
				g.DrawRectangle(Pens.Red, this.Location.X, this.Location.Y, this.ClientRectangle.Width - 1, this.ClientRectangle.Height - 1);
			}

			this.pictureBox1.Invalidate();
		}

		private void pictureBox1_MouseUp(object sender, MouseEventArgs e) {
			if (_drawing) {
				_drawing = false;
			}

			DrawTool(Graphics.FromImage(pictureBox1.BackgroundImage), _selectedTool);
			_startPoint = new Point(0);
			_endPoint = new Point(0);
			_selectedTool = Tools.Move;
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
				Pen pen = new Pen(Color.FromArgb(255, 255, 0, 0), 3);
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
