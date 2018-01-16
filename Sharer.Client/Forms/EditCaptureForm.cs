using Sharer.Client.Helpers;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sharer.Client.Forms {
	public partial class EditCaptureForm : Form {
		private enum Tools {
			Move,
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
		private Image _screenshot;

		private ArrowForm _arrowForm;

		public EditCaptureForm(Image screenshot, Rectangle area) {
			InitializeComponent();

			this.MaximumSize = SystemInformation.PrimaryMonitorSize;
			this.AutoScroll = false;
			//this.pictureBox1.Image = screenshot;
			this.pictureBox1.BackgroundImage = screenshot;
			this.pictureBox1.Image = new Bitmap(screenshot.Width, screenshot.Height);
			this.pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;

			_area = area;
			_screenshot = screenshot;

			this.DoubleBuffered = true;
			this.SetStyle(ControlStyles.ResizeRedraw, true);
			this._selectedTool = Tools.Move;


			// hack to allow buttons be transparent
			//this.buttonDrawArrow.Parent = this.Parent;
			//this.buttonDrawRectangle.Parent = this;
			//this.buttonUpload.Parent = this.pictureBox1;

			SetStyle(ControlStyles.DoubleBuffer, true);
			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.ResizeRedraw, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, false);

			_arrowForm = new ArrowForm();
			//_arrowForm.Size = new Size(58, 33);
		}

		private async void EditCaptureForm_Load(object sender, EventArgs e) {
			this.Location = new Point(_area.Location.X - 8, _area.Location.Y - 31);
			this.Size = new Size(_area.Size.Width + 16, _area.Size.Height + 38);
			_arrowForm.Show(this);
			//if (this.pictureBox1.Width >= this.MaximumSize.Width - 54 || this.pictureBox1.Height >= this.MaximumSize.Height - 71) {
			//	this.Location = new Point(0);
			//	this.SetAutoScrollMargin(this.pictureBox1.Width - 54, this.pictureBox1.Height - 71);
			//} else {
			//	this.Location = new Point(_area.X - 8, _area.Y - 31);
			//}
		}

		//const int WM_NCHITTEST = 0x0084;
		//const int HTCLIENT = 1;
		//const int HTCAPTION = 2;
		//protected override void WndProc(ref Message m) {
		//	base.WndProc(ref m);
		//	switch (m.Msg) {
		//		case WM_NCHITTEST:
		//			if (m.Result == (IntPtr)HTCLIENT) {
		//				m.Result = (IntPtr)HTCAPTION;
		//			}
		//			break;
		//	}
		//}
		//protected override CreateParams CreateParams {
		//	get {
		//		CreateParams cp = base.CreateParams;
		//		cp.Style |= 0x40000;
		//		return cp;
		//	}
		//}

		private void EditCaptureForm_KeyDown(object sender, KeyEventArgs e) {
			if (e.KeyCode == Keys.Escape) {
				CloseCancel();
			}
		}

		private void EditCaptureForm_Resize(object sender, EventArgs e) {
			DrawBoarder();
		}

		private void DrawBoarder() {
			using (Graphics g = Graphics.FromImage(this.pictureBox1.Image)) {
				g.Clear(Color.Transparent);
				g.DrawRectangle(Pens.Red, this.Location.X + 58, this.Location.Y + 10, this.ClientRectangle.Width - 1, this.ClientRectangle.Height - 1);
			}
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

		private Point _startCursorPoint;
		private Point _startFormPoint;

		private void pictureBox1_MouseDown(object sender, MouseEventArgs e) {
			_startPoint = e.Location;
			_drawing = true;
			_startCursorPoint = Cursor.Position;
			_startFormPoint = this.Location;
		}

		private void pictureBox1_MouseMove(object sender, MouseEventArgs e) {
			if (this._selectedTool == Tools.Move) {
				this.pictureBox1.Cursor = Cursors.SizeAll;
			} else if (this._selectedTool == Tools.Arrow) {
				this.pictureBox1.Cursor = Cursors.UpArrow;
			} else if (this._selectedTool == Tools.Rectangle) {
				this.pictureBox1.Cursor = Cursors.Cross;
			}
			//if (_drawing) {
			//	_endPoint = e.Location;
			//}
			if (_drawing) {
				_endPoint = e.Location;
				if (this._selectedTool == Tools.Move) {
					Point dif = Point.Subtract(Cursor.Position, new Size(_startCursorPoint));
					this.Location = Point.Add(_startFormPoint, new Size(dif));
				}
			}
			DrawBoarder();
			_arrowForm.Location = new Point(this.Location.X - _arrowForm.Width - 2, this.Location.Y + 1);
			this.pictureBox1.Invalidate();
		}

		private void pictureBox1_MouseUp(object sender, MouseEventArgs e) {
			if (_drawing) {
				_drawing = false;
			}

			DrawTool(Graphics.FromImage(pictureBox1.BackgroundImage), _selectedTool);
			_startPoint = new Point(0);
			_endPoint = new Point(0);
			this._selectedTool = Tools.Move;
		}

		private void pictureBox1_Move(object sender, EventArgs e) {
			this.pictureBox1.Location = new Point(-this.Location.X - 58, -this.Location.Y - 10);
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
