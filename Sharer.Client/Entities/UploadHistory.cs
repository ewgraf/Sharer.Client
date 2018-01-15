using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Sharer.Client.Entities {
	public class UploadHistory {
		private static ToolStripMenuItem EmptyHistory = new ToolStripMenuItem("(Empty)") { Enabled = false };
		private FixedSizedQueue<ToolStripMenuItem> _queue = new FixedSizedQueue<ToolStripMenuItem>();

		public ToolStripMenuItem[] HistoryItems {
			get {
				List<ToolStripMenuItem> items = new List<ToolStripMenuItem>();
				ToolStripMenuItem item;
				while (_queue.TryDequeue(out item)) {
					items.Add(item);
				}
				items.ForEach(i => _queue.Enqueue(i));
				if (items.Count > 0) {                    
					return items.ToArray();
				} else {
					return new[] { EmptyHistory };
				}
			}
		}

		public UploadHistory() {
			_queue.MaximumLength = 255; // 255 items will be enough for everybody :)
		}

		public void Add(string link, Image image) {
			ToolStripMenuItem historyItem = new ToolStripMenuItem(Path.GetFileName(link));
			string linkWithoutExtension = Path.Combine(Path.GetDirectoryName(link), Path.GetFileNameWithoutExtension(link));
			historyItem.Image = image;
			historyItem.Click += (o, e) => Process.Start(linkWithoutExtension);
			historyItem.ToolTipText = $"{DateTime.Now:HH:mm:ss}";
			_queue.Enqueue(historyItem);
		}
	}
}
