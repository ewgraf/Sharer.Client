using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sharer.Client.Entities;
using Sharer.Client.Forms;
using Sharer.Client.Helpers;

namespace Sharer.Client {
	public partial class MainForm : Form {
		private static Mutex _mutex;
		private Account _account;
		private AreaSelectionForm _selectionForm;
		private OpenWithListener _openWithListener;
		private string _openWithSharerFileUploadPath;
		public string lastLink;
		private UploadHistory _history = new UploadHistory();
		private readonly Action _contextMenuUploadCancelled;
		private readonly Action _contextMenuUploadFinished;
		private CancellationTokenSource _uploadCancellationTokenSource;

		private readonly Icon _icon;

		private class ContextMenuItems {
			public const string Account = "Account";
			public const string CaptureArea = "Capture area . . .";
			public const string CaptureScreen = "Capture screen";
			public const string Upload = "Upload . . .";
			public const string History = "History";
			public const string Cancel = "(cancel)";
			public const string Settings = "Settings";
			public const string Exit = "Exit";
		}

		public MainForm(string openWithSharerFileUploadPath, Mutex mutex) {
			_openWithSharerFileUploadPath = openWithSharerFileUploadPath;
			_openWithListener = new OpenWithListener();
			_mutex = mutex;

			// start listener in separate thread
			Task.Run(() => _openWithListener.Start(filePath => {
				try {
					UploadPath(filePath, this, CancellationToken.None).Wait();
					return true;
				} catch (Exception ex) {
					MessageBox.Show(ex.ToString());
					return true;
				}
			}));

			InitializeComponent();
			_icon = this.notifyIcon1.Icon;
			SetupCheckBoxEditBeforeUpload();

			this.notifyIcon1.ContextMenuStrip = BuildContextMenuStrip();
			SetupContextMenuStrip(
				out _contextMenuUploadCancelled,
				out _contextMenuUploadFinished
			);
			_uploadCancellationTokenSource = new CancellationTokenSource();
		}

		#region MainForm

		private void SetupCheckBoxEditBeforeUpload() {
			new ToolTip() {
				ShowAlways = true,
				InitialDelay = 1,
				AutoPopDelay = 10000
			}.SetToolTip(this.checkBox_EditBeforeUpload, $@"'Edit' form will be shown before uploading");
		}

		#endregion

		#region ContextMenuStrip

		private ContextMenuStrip BuildContextMenuStrip() {
			var strip = new ContextMenuStrip();
			strip.Items.Add(ContextMenuItems.Account);
			strip.Items.Add("-");
			strip.Items.Add(ContextMenuItems.CaptureArea, ImageHelper.DrawString("#", new Font("Arial", 13)));
			strip.Items.Add(ContextMenuItems.CaptureScreen, ImageHelper.DrawString("@"));
			strip.Items.Add(ContextMenuItems.Upload, ImageHelper.DrawString("^"));
			strip.Items.Add("-");
			strip.Items.Add(new ToolStripMenuItem(ContextMenuItems.History, null, _history.HistoryItems) { Name = ContextMenuItems.History });
			strip.Items.Add("-");
			strip.Items.Add(ContextMenuItems.Settings, ImageHelper.DrawString("⚙", new Font("Arial", 16), 0, -3));
			strip.Items.Add("-");
			strip.Items.Add(ContextMenuItems.Exit);
			return strip;
		}

		private void SetupContextMenuStrip(out Action uploadCancelled, out Action uploadFinished) {
			this.notifyIcon1.ContextMenuStrip.ItemClicked += menu_ItemClicked;
			uploadCancelled = () => {
				(this.notifyIcon1.ContextMenuStrip.Items["History"] as ToolStripMenuItem).Text = ContextMenuItems.History;
			};
			uploadFinished = () => {
				try {
					if (this.notifyIcon1.ContextMenuStrip.InvokeRequired) {
						this.notifyIcon1.ContextMenuStrip.Invoke(new Action(() => {
							(this.notifyIcon1.ContextMenuStrip.Items["History"] as ToolStripMenuItem).Text = ContextMenuItems.History;
							(this.notifyIcon1.ContextMenuStrip.Items["History"] as ToolStripMenuItem).DropDownItems.Clear();
							(this.notifyIcon1.ContextMenuStrip.Items["History"] as ToolStripMenuItem).DropDownItems.AddRange(_history.HistoryItems);
						}));
					} else {
						(this.notifyIcon1.ContextMenuStrip.Items["History"] as ToolStripMenuItem).Text = ContextMenuItems.History;
						(this.notifyIcon1.ContextMenuStrip.Items["History"] as ToolStripMenuItem).DropDownItems.Clear();
						(this.notifyIcon1.ContextMenuStrip.Items["History"] as ToolStripMenuItem).DropDownItems.AddRange(_history.HistoryItems);
					}
				} catch (Exception ex) {
					;
				}
			};
			//_showProgress = (string text) => {
			//	//Task.Run(() => {
			//		_showProgressToken = new CancellationTokenSource();
			//		char[] progressChars = new[] { '-', '\\', '|', '/' };
			//		int i = 0;
			//		while (!_showProgressToken.IsCancellationRequested) {
			//			this.notifyIcon1.ContextMenuStrip.Invoke(new Action<string>(UpdateProgressText), new object[] { $"{text}{progressChars[i++]}" });
			//			if (i == progressChars.Length) {
			//				i = 0;
			//			}
			//			this.notifyIcon1.ContextMenuStrip.Invalidate();
			//			this.Invalidate();
			//			Thread.Sleep(500);
			//		}
			//	//});
			//};
		}

		#endregion

		private async void Form1_Load(object sender, EventArgs e) {
			notifyIcon1_MouseClick(sender, new MouseEventArgs(MouseButtons.Right, 0, 0, 0, 0));
			this.WindowState = FormWindowState.Minimized;
			this.ShowInTaskbar = false;
			if (!Auth()) {
				Application.Exit();
				return;
			}
			//notifyIcon1.ShowBalloonTip(250, $"Good day, {_account.Email.Split('@')[0]}-san!", "Sharer is working in the background", ToolTipIcon.Info);

			if (_openWithSharerFileUploadPath != null) {
				await UploadPath(_openWithSharerFileUploadPath, this, CancellationToken.None);
			}

			InitKeyHooks();
		}

		private bool Auth() {
			_account = AuthHelper.TryAuth();
			if (_account != null) {
				linkLabelEmail.Text = _account.Email;
				return true;
			} else {
				return false;
			}
		}

		public void SetClipboardAndShowToast(string filePath, string link) {
			RunSTATask(() => {
				Clipboard.SetText(link);
			});
			ToastHelper.ShowToast(link, filePath, (s, a) => {
				if (link != null) {
					Process.Start(link);
				}
			});
		}

		private Image TryOpenImage(string path) {
			Image image = null;
			try {
				using (var bitmap = new Bitmap(path)) {
					image = (Image)bitmap.Resize(64, 64).Clone();
				}
			} catch { }
			return image;
		}

		public async Task UploadPath(string filePath, MainForm form, CancellationToken token) {
			Account account = null;
			string result = null;
			if (!Sharer.FileSizeCorrect(filePath)) {
				MessageBox.Show($"Sorry, but uploading file '{filePath}' exceeds size limits of {Sharer.MaxMb}Mb");
				return;
			}

			try {
				string size = new FileInfo(filePath).Length.ToInformationPrefixString();
				StartDisplayProgress(size);

				if (string.IsNullOrEmpty(filePath)) {
					throw new ArgumentOutOfRangeException(nameof(filePath));
				}
				if (form == null) {
					throw new ArgumentNullException(nameof(form));
				}

				account = ConfigHelper.FindAccount();
				if (account == null) {
					throw new InvalidOperationException("Account was not found, try to relog");
				}
				
				result = await NetHelper.UploadPath(filePath, account, token);

				if (result.Length > 0) {
					if (result[0] == '+') {
						result = result.Substring(1, result.Length - 1);
						form.SetClipboardAndShowToast(filePath, result);
						_history.Add($"{result}{Path.GetExtension(filePath)}", TryOpenImage(filePath));
						_contextMenuUploadFinished();
					} else if (result[0] == '-') {
						throw new InvalidOperationException(result.Substring(1, result.Length - 1).Replace("\\n", Environment.NewLine));
					} else {
						throw new InvalidOperationException($"Server refused uploading file. May you retry");
					}
				}
			} catch (Exception ex) {
				MessageBox.Show(ex.Message, $"Failed to upload");
				return;
			} finally {
				StopDisplayProgress();
			}
		}
		
		public static Task RunSTATask(Action action) {
			var tcs = new TaskCompletionSource<Action>();
			Thread thread = new Thread(() => {
				try {
					action();
				} catch (Exception e) {
					tcs.SetException(e);
				}
			});
			thread.SetApartmentState(ApartmentState.STA);
			thread.Start();
			return tcs.Task;
		}

		// покрыть тестом большим файлом
		private void UploadImage(Image image, CancellationToken token) {
			try {
				Cursor.Current = Cursors.WaitCursor;
				string path = Directory.GetParent(Sharer.LastUploadFile).FullName;
				if (!Directory.Exists(path)) {
					Directory.CreateDirectory(path);
				}
				if (File.Exists(Sharer.LastUploadFile)) {
					File.Delete(Sharer.LastUploadFile);
					while (File.Exists(Sharer.LastUploadFile)) {
						Thread.Sleep(5);
					}
				}
				image.Save(Sharer.LastUploadFile);
				image.Dispose();
				while (FileHelper.IsLocked(Sharer.LastUploadFile)) {
					Thread.Sleep(5);
				}
				Task.Run(() => UploadPath(Sharer.LastUploadFile, this, token));
			} catch (Exception ex) {
				MessageBox.Show($"at UploadImage {ex.ToString()}");
			} finally {
				Cursor.Current = Cursors.Default;
			}
		}

		private async void InitKeyHooks() {
			InterceptKeys.SetHooks(
				CaptureScreen,          // Ctrl+Shift+2 @
				CaptureArea,            // Ctrl+Shift+3 #
				SelectAndUploadFiles,   // Ctrl+Shift+6 ^
				_uploadCancellationTokenSource.Token
			);
		}

		private void CaptureArea(CancellationToken token) {
			try {
				_selectionForm.Close();
				_selectionForm = null;
			} catch {
			}
			_selectionForm = new AreaSelectionForm();
			var result = _selectionForm.ShowDialog();
			if (result == DialogResult.Cancel) {
				return;
			}
			if (result == DialogResult.OK) {
				Rectangle selected = _selectionForm.SelectedRectangle;
				if (selected == Rectangle.Empty) {
					return;
				}
				Image screenshot = ScreenCaptureHelper.CaptureScreen();
				EditAndUploadIfChecked(screenshot, selected, token);
			}
		}

		private void CaptureScreen(CancellationToken token) {
			Image image = ScreenCaptureHelper.CaptureScreen();
			UploadImage(image, token);
		}

		private void EditAndUploadIfChecked(Image image, Rectangle area, CancellationToken token) {
			if (checkBox_EditBeforeUpload.Checked) {
				var edit = new EditCaptureForm(image, area);
				if (edit.ShowDialog() == DialogResult.OK) {
					UploadImage(edit.Image, token);
				}
			} else {
				UploadImage(image, token);
			}
		}


		private bool _uploading;

		private Task StartDisplayProgress(string size) {
			Cursor.Current = Cursors.WaitCursor;
			return Task.Run(() => {
				char[] progressChars = new[] { '—', '\\', '|', '/' };
				int i = 0;
				Font font38 = new Font("Courier New", 38);
				Font font = new Font("Courier New", 20);
				var rect = new Rectangle(0, 0, _icon.Width, _icon.Height);
				var format = new StringFormat {
					LineAlignment = StringAlignment.Center,
					Alignment = StringAlignment.Center
				};
				_uploading = true;
				while (_uploading) {
					try {
						this.notifyIcon1.ContextMenuStrip.Invoke(new Action<string>(UpdateProgress), new object[] { $"< Uploading {size} {progressChars[i]} >" });
					} catch { }
					
					Bitmap icon = _icon.ToBitmap();
					using (Graphics g = Graphics.FromImage(icon)) {
						if (i == 2) {
							g.DrawString(progressChars[i].ToString(), font38, Brushes.Black, rect, format);
						} else {
							g.DrawString(progressChars[i].ToString(), font, Brushes.Black, rect, format);
						}
					}
					this.notifyIcon1.Icon = Icon.FromHandle(icon.GetHicon());
					if (++i == progressChars.Length) {
						i = 0;
					}
					Thread.Sleep(100);
				}
				try {
					this.notifyIcon1.Icon = _icon;
					if (this.notifyIcon1.ContextMenuStrip.InvokeRequired) {
						this.notifyIcon1.ContextMenuStrip.Invoke(new Action(() => {
							(this.notifyIcon1.ContextMenuStrip.Items["History"] as ToolStripMenuItem).Text = ContextMenuItems.History;
							(this.notifyIcon1.ContextMenuStrip.Items["History"] as ToolStripMenuItem).DropDownItems.Clear();
							(this.notifyIcon1.ContextMenuStrip.Items["History"] as ToolStripMenuItem).DropDownItems.AddRange(_history.HistoryItems);
						}));
					} else {
						(this.notifyIcon1.ContextMenuStrip.Items["History"] as ToolStripMenuItem).Text = ContextMenuItems.History;
						(this.notifyIcon1.ContextMenuStrip.Items["History"] as ToolStripMenuItem).DropDownItems.Clear();
						(this.notifyIcon1.ContextMenuStrip.Items["History"] as ToolStripMenuItem).DropDownItems.AddRange(_history.HistoryItems);
					}
				} catch (Exception ex) {
					;
				}
			});
		}

		private void StopDisplayProgress() {
			Cursor.Current = Cursors.Default;
			_uploading = false;
			this.notifyIcon1.Icon = _icon;
		}


		private async Task SelectAndUploadFiles(CancellationToken token) {
			string[] selectedFiles = OpenSelectFilesDialog();
			foreach (string filePath in selectedFiles) {
				try {
					Task.Run(() => UploadPath(filePath, this, token));
				} catch(Exception ex) {
					ToastHelper.ShowToast($"Failed uploading file: '{filePath}'", ex.ToString());
				}
			}
		}

		private void UpdateProgress(string text) {
			(this.notifyIcon1.ContextMenuStrip.Items["History"] as ToolStripMenuItem).Text = text;
		}

		private string[] OpenSelectFilesDialog() {
			using (var ofd = new OpenFileDialog()) {
				// Set the file dialog to filter for graphics files.
				ofd.Filter = "All files (*.*)|*.*";

				// Allow the user to select multiple images.
				ofd.Multiselect = true;
				ofd.Title = "Select files to share";
				if(ofd.ShowDialog() == DialogResult.OK) {
					return ofd.FileNames;
				} else {
					return Array.Empty<string>();
				}
			}
		}

		private void Form1_Resize(object sender, EventArgs e) {
			if (this.WindowState == FormWindowState.Minimized) {
				this.Hide();
			} else if (this.WindowState == FormWindowState.Normal) {
			}
		}

		private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e) {
			if (e.Button == MouseButtons.Left) {
				if (this.WindowState == FormWindowState.Minimized) {
					this.Show();
					this.WindowState = FormWindowState.Normal;
				} else if (this.WindowState == FormWindowState.Normal) {
					this.WindowState = FormWindowState.Minimized;
				}
			}
		}

		// tray right-click menu
		private void notifyIcon1_MouseClick(object sender, MouseEventArgs e) {
			if (e.Button == MouseButtons.Right) {
				notifyIcon1.ContextMenuStrip.Show(Cursor.Position);
			}
		}

		private async void menu_ItemClicked(object sender, ToolStripItemClickedEventArgs e) {
			switch (e.ClickedItem.Text) {
				case ContextMenuItems.Account:
					Process.Start(Sharer.Uris.AccountPage);
					break;
				case ContextMenuItems.CaptureArea:
					_uploadCancellationTokenSource = new CancellationTokenSource();
					CaptureArea(_uploadCancellationTokenSource.Token);
					break;
				case ContextMenuItems.CaptureScreen:
					_uploadCancellationTokenSource = new CancellationTokenSource();
					CaptureScreen(_uploadCancellationTokenSource.Token);
					break;
				case ContextMenuItems.Upload:
					_uploadCancellationTokenSource = new CancellationTokenSource();
					SelectAndUploadFiles(_uploadCancellationTokenSource.Token);
					break;
				case ContextMenuItems.Settings:
					notifyIcon1_MouseDoubleClick(sender, new MouseEventArgs(MouseButtons.Left, 0, 0, 0, 0));
					break;
				case ContextMenuItems.Exit:
					Application.Exit();
					break;
				default:
					if (e.ClickedItem.Text.Contains(ContextMenuItems.Cancel)) {
						_uploadCancellationTokenSource.Cancel();
					}
					break;
			}
		}
		
		private void logoutButton_Click(object sender, EventArgs e) {
			ConfigHelper.ClearAccount();
			_mutex.ReleaseMutex();
			Application.Restart();
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
			if (e.CloseReason == CloseReason.UserClosing) {
				e.Cancel = true;
				this.WindowState = FormWindowState.Minimized;
			}
		}

		private void linkLabelEmail_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
			Process.Start(Sharer.Uris.AccountPage);
		}
	}
}

#region Optional TODO's
// TODO: ffmpeg video record
//private List<Image> frames = new List<Image>();
//private async void Form1_Load(object sender, EventArgs e) {
//	RecordOneSecond60Frames();
//	int width = frames[0].Width;
//	int height = frames[0].Height; zz
//	  create instance of video writer
//	VideoFileWriter writer = new VideoFileWriter();
//	create new video file
//	writer.Open(@"D:\frames\out.webm", width, height, 60, VideoCodec.Default);
//	foreach (var f in frames) {
//		writer.WriteVideoFrame((Bitmap)f);
//	}
//	writer.Close();
//	writer.Dispose();
//	frames.Clear();
//	int i = 0;
//	frames.ForEach(n => n.Save(string.Format(@"D:\frames\{0:00}.png", i++), System.Drawing.Imaging.ImageFormat.Png));
//}
#endregion