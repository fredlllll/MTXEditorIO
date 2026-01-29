namespace TexEditor
{
    partial class TextureView
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            pictureBox = new PictureBox();
            contextMenuStrip1 = new ContextMenuStrip(components);
            replaceTextureToolStripMenuItem = new ToolStripMenuItem();
            saveTextureToolStripMenuItem = new ToolStripMenuItem();
            txtName = new TextBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox).BeginInit();
            contextMenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // pictureBox
            // 
            pictureBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pictureBox.BackColor = SystemColors.ControlDark;
            pictureBox.Location = new Point(0, 0);
            pictureBox.Name = "pictureBox";
            pictureBox.Size = new Size(193, 193);
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox.TabIndex = 0;
            pictureBox.TabStop = false;
            pictureBox.MouseClick += PictureBox_MouseClick;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { replaceTextureToolStripMenuItem, saveTextureToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(157, 48);
            // 
            // replaceTextureToolStripMenuItem
            // 
            replaceTextureToolStripMenuItem.Name = "replaceTextureToolStripMenuItem";
            replaceTextureToolStripMenuItem.Size = new Size(156, 22);
            replaceTextureToolStripMenuItem.Text = "Replace Texture";
            replaceTextureToolStripMenuItem.Click += ReplaceTextureToolStripMenuItem_Click;
            // 
            // saveTextureToolStripMenuItem
            // 
            saveTextureToolStripMenuItem.Name = "saveTextureToolStripMenuItem";
            saveTextureToolStripMenuItem.Size = new Size(156, 22);
            saveTextureToolStripMenuItem.Text = "Save Texture";
            saveTextureToolStripMenuItem.Click += SaveTextureToolStripMenuItem_Click;
            // 
            // txtName
            // 
            txtName.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            txtName.Location = new Point(3, 199);
            txtName.Name = "txtName";
            txtName.Size = new Size(141, 23);
            txtName.TabIndex = 2;
            // 
            // TextureView
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(txtName);
            Controls.Add(pictureBox);
            Name = "TextureView";
            Size = new Size(193, 225);
            ((System.ComponentModel.ISupportInitialize)pictureBox).EndInit();
            contextMenuStrip1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        public PictureBox pictureBox;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem replaceTextureToolStripMenuItem;
        private ToolStripMenuItem saveTextureToolStripMenuItem;
        public TextBox txtName;
    }
}
