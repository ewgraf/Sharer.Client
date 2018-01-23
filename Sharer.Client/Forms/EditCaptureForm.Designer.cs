namespace Sharer.Client.Forms
{
    partial class EditCaptureForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.buttonUpload = new System.Windows.Forms.Button();
			this.buttonDrawRectangle = new System.Windows.Forms.Button();
			this.buttonDrawArrow = new System.Windows.Forms.Button();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// buttonUpload
			// 
			this.buttonUpload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonUpload.BackColor = System.Drawing.Color.White;
			this.buttonUpload.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonUpload.Font = new System.Drawing.Font("Consolas", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.buttonUpload.ForeColor = System.Drawing.Color.LightSkyBlue;
			this.buttonUpload.Location = new System.Drawing.Point(214, 215);
			this.buttonUpload.Name = "buttonUpload";
			this.buttonUpload.Size = new System.Drawing.Size(58, 34);
			this.buttonUpload.TabIndex = 1;
			this.buttonUpload.Text = "↑☁";
			this.buttonUpload.UseVisualStyleBackColor = false;
			this.buttonUpload.Click += new System.EventHandler(this.buttonUpload_Click);
			// 
			// buttonDrawRectangle
			// 
			this.buttonDrawRectangle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.buttonDrawRectangle.BackColor = System.Drawing.Color.White;
			this.buttonDrawRectangle.BackgroundImage = global::Sharer.Client.Properties.Resources.Red_rectangle;
			this.buttonDrawRectangle.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.buttonDrawRectangle.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonDrawRectangle.Location = new System.Drawing.Point(76, 215);
			this.buttonDrawRectangle.Name = "buttonDrawRectangle";
			this.buttonDrawRectangle.Size = new System.Drawing.Size(58, 34);
			this.buttonDrawRectangle.TabIndex = 3;
			this.buttonDrawRectangle.UseVisualStyleBackColor = false;
			this.buttonDrawRectangle.Click += new System.EventHandler(this.buttonDrawRectangle_Click);
			// 
			// buttonDrawArrow
			// 
			this.buttonDrawArrow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.buttonDrawArrow.BackColor = System.Drawing.Color.White;
			this.buttonDrawArrow.BackgroundImage = global::Sharer.Client.Properties.Resources.Red_arrow;
			this.buttonDrawArrow.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.buttonDrawArrow.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonDrawArrow.Location = new System.Drawing.Point(12, 215);
			this.buttonDrawArrow.Name = "buttonDrawArrow";
			this.buttonDrawArrow.Size = new System.Drawing.Size(58, 34);
			this.buttonDrawArrow.TabIndex = 2;
			this.buttonDrawArrow.UseVisualStyleBackColor = false;
			this.buttonDrawArrow.Click += new System.EventHandler(this.buttonDrawArrow_Click);
			// 
			// pictureBox1
			// 
			this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pictureBox1.BackColor = System.Drawing.Color.Black;
			this.pictureBox1.Location = new System.Drawing.Point(0, 0);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(283, 260);
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
			this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
			this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
			this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
			// 
			// EditCaptureForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoScroll = true;
			this.ClientSize = new System.Drawing.Size(284, 261);
			this.ControlBox = false;
			this.Controls.Add(this.buttonUpload);
			this.Controls.Add(this.buttonDrawRectangle);
			this.Controls.Add(this.buttonDrawArrow);
			this.Controls.Add(this.pictureBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "EditCaptureForm";
			this.TopMost = true;
			this.TransparencyKey = System.Drawing.SystemColors.MenuHighlight;
			this.Load += new System.EventHandler(this.EditCaptureForm_Load);
			this.LocationChanged += new System.EventHandler(this.EditCaptureForm_LocationChanged);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EditCaptureForm_KeyDown);
			this.Resize += new System.EventHandler(this.EditCaptureForm_Resize);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button buttonDrawArrow;
        private System.Windows.Forms.Button buttonDrawRectangle;
        private System.Windows.Forms.Button buttonUpload;
	}
}