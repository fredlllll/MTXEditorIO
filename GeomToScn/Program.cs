using MTXEditorIO.Raw.GeomPS2;
using MTXEditorIO.Raw.ScnTHUG1;
using GeomToScn;

if (args.Length < 1)
{
    Console.WriteLine("Usage: GeomToScn <input.geom.ps2> [output.scn.xbx]");
    return;
}

string input = Path.GetFullPath(args[0]);
string output = args.Length > 1 ? Path.GetFullPath(args[1]) : Path.ChangeExtension(input, ".scn.xbx");

if (!File.Exists(input))
{
    Console.WriteLine($"input file not found: {input}");
    return;
}

byte[] file = File.ReadAllBytes(input);
GeomPS2 geom = new GeomPS2();
using (var ms = new MemoryStream(file, writable: false))
{
    geom.ReadFrom(ms);
}

var converter = new GeomToScnConverter(geom, file);
converter.PrintAnalysis();

var scn = converter.ConvertToScn();

using (var fs = new FileStream(output, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
{
    scn.WriteTo(fs);
}

// self-verification: read the written file back and re-count everything
ScnTHUG1 roundTrip;
using (var fs = new FileStream(output, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
{
    roundTrip = new ScnTHUG1();
    roundTrip.ReadFrom(fs);
}

bool matches = roundTrip.materials.Length == scn.materials.Length &&
               roundTrip.sectors.Length == scn.sectors.Length;
Console.WriteLine($"wrote {output} ({new FileInfo(output).Length} bytes): {scn.materials.Length} materials, {scn.sectors.Length} sectors; read-back {(matches ? "OK" : "MISMATCH")}");