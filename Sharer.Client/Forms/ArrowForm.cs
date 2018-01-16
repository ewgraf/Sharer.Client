using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sharer.Client.Forms {
	public partial class ArrowForm : Form {
		public ArrowForm() {
			InitializeComponent();
		}

		private void ArrowForm_MouseHover(object sender, EventArgs e) {
			this.FormBorderStyle = FormBorderStyle.Fixed3D;
		}
	}
}
