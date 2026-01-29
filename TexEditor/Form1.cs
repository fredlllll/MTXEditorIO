using MTXEditorIO.Raw.TexPC;
using System.Globalization;

namespace TexEditor
{
    public partial class Form1 : Form
    {
        const int rowHeight = 164;
        private readonly List<TextureView> currentTextureViews = new List<TextureView>();
        private string lastLoadedPath = string.Empty;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "Tex Files|*tex|All Files|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                LoadTex(ofd.FileName);
            }
        }

        private void FreeLoadedTextures()
        {
            foreach (var tv in currentTextureViews)
            {
                tv.pictureBox.Image.Dispose();
                tv.Dispose();
            }
            currentTextureViews.Clear();
            tableLayoutPanel1.Controls.Clear();
        }

        private void LoadTex(string path)
        {
            FreeLoadedTextures();
            var tex = new TexPC();
            using var fs = File.OpenRead(path);
            tex.ReadFrom(fs);
            tableLayoutPanel1.RowCount = (int)Math.Ceiling(tex.images.Length / 3f);
            foreach (RowStyle style in tableLayoutPanel1.RowStyles)
            {
                style.Height = rowHeight;
                style.SizeType = SizeType.Absolute;
            }
            foreach (var img in tex.images)
            {
                var textureView = new TextureView();
                textureView.bottomLabel.Text = img.header.checksum.ToString("X8", CultureInfo.InvariantCulture);
                textureView.pictureBox.Image = ImageConversion.GetImageFromTexImg(img);
                tableLayoutPanel1.Controls.Add(textureView);
                currentTextureViews.Add(textureView);
            }
            tableLayoutPanel1.Height = tableLayoutPanel1.PreferredSize.Height;
            lastLoadedPath = path;
        }



        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog();
            sfd.Filter = "Tex Files|*tex|All Files|*.*";
            sfd.FileName = lastLoadedPath;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                SaveTex(sfd.FileName);
            }
        }

        private void SaveTex(string path)
        {
            var tex = new TexPC();
            tex.images = new TexPCImg[currentTextureViews.Count];
            for (int i = 0; i < tex.images.Length; i++)
            {
                var tv = currentTextureViews[i];
                var img = tex.images[i] = ImageConversion.GetTexImgFromImage((Bitmap)tv.pictureBox.Image);
                img.header.checksum = uint.Parse(tv.bottomLabel.Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
                tex.images = tex.images.Append(img).ToArray();
            }
            using var fs = new FileStream(path, FileMode.Create, FileAccess.Write);
            tex.WriteTo(fs);
        }
    }
}
