using BCnEncoder.Decoder;
using BCnEncoder.Shared;
using CSWavefront.Raw;
using MTXEditorIO.Raw.Col;
using MTXEditorIO.Raw.Img;
using MTXEditorIO.Raw.Pre;
using MTXEditorIO.Raw.ScnTHUG1;
using MTXEditorIO.Raw.TexPC;
using MTXEditorIO.Raw.Zqb;
using MTXEditorIO.Util;
using MTXEditorIO.Util.Hashing;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Data;
using System.Numerics;
using System.Text.Unicode;

namespace Test
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            System.Globalization.CultureInfo.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

            string folder = "I:\\Projects\\MTX Mototrax PRO Modding\\MTX Mototrax PRO\\data\\images";
            foreach (var file in Directory.GetFiles(folder, "*Zimg", SearchOption.AllDirectories))
            {
                using var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                var img = new Img();
                try
                {
                    img.ReadFrom(fs);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error reading ZQB: {file}, ex: {ex}");
                    continue;
                }
                Console.WriteLine($"{img.header} {Path.GetRelativePath(folder,file)}");
            }


            /*var col = new Col();
            col.ReadFrom(fs);

            Console.WriteLine($"Done reading, trailing bytes: {fs.Length - fs.Position}, streampos at {fs.Position}");

            var model = new ObjFile();
            var levelMesh = model.objects["aaa"];
            var polys = levelMesh.polygons["bbb"];

            Console.WriteLine(col.header);
            for (int i = 0; i < col.objects.Length; ++i)
            {
                var obj = col.objects[i];

                foreach (var face in obj.faces)
                {
                    var v1 = obj.vertices[face.VertIndex1].GetPos(obj);
                    var v2 = obj.vertices[face.VertIndex2].GetPos(obj);
                    var v3 = obj.vertices[face.VertIndex3].GetPos(obj);


                    int vertexIndex = model.vertices.Count;
                    model.vertices.Add(new Vector4(v1.x, v1.y, v1.z, 0) / 100);
                    model.vertices.Add(new Vector4(v2.x, v2.y, v2.z, 0) / 100);
                    model.vertices.Add(new Vector4(v3.x, v3.y, v3.z, 0) / 100);

                    var ov1 = new PolygonVertex();
                    ov1.vertex = vertexIndex;
                    var ov2 = new PolygonVertex();
                    ov2.vertex = vertexIndex + 1;
                    var ov3 = new PolygonVertex();
                    ov3.vertex = vertexIndex + 2;

                    var oface = new Polygon();
                    oface.vertices.Add(ov1);
                    oface.vertices.Add(ov2);
                    oface.vertices.Add(ov3);
                    polys.Add(oface);
                }
            }
            Console.WriteLine("");

            ObjSaver.Save(model, "GhostTown.obj");
            */

        }
    }
}
