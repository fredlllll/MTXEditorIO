using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TexEditor
{
    public partial class TextureView : UserControl
    {
        public uint originalEncoding = 0;

        public TextureView()
        {
            InitializeComponent();
        }

        private void PictureBox_MouseClick(object? sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
            {
                contextMenuStrip1.Show(Cursor.Position);
            }
        }

        private void ReplaceTextureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "PNG Files|*.png|All Files|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                using var fs = File.OpenRead(ofd.FileName);
                var img = Image.FromStream(fs);
                pictureBox.Image?.Dispose();
                pictureBox.Image = img;
            }
        }

        private void SaveTextureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog();
            sfd.Filter = "PNG Files|*.png|All Files|*.*";
            sfd.FileName = txtName.Text + ".png";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                pictureBox.Image.Save(sfd.FileName);
            }
        }
    }
}
