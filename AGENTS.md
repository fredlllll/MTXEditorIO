# MTXEditorIO

C# tooling for reading/writing game data files from **MTX Mototrax PRO** (Rainbow Studios, 2005). The parent workspace `C:\Projects\Mtx\` also contains a real game install (`MTX Mototrax PRO\`) and third-party binary tools (`PreTool 1.1\`, `thpsqscripted\`, `THUG2 Texture Tools by defeat0r\`) — none of those are part of this repo; only modify them if specifically asked to.

## Layout

- `MTXEditorIO/` — class library (`netstandard2.1`, `Nullable` enabled, `GeneratePackageOnBuild=true` so every build also emits a nupkg). All binary format readers/writers live in `MTXEditorIO.Raw.*`: `TexPC`, `TexPS2`, `Img`, `Pre`, `Zqb`, `Col`, `SkaPC`, `ScnTHUG1`, `GeomPS2`. Shared helpers in `MTXEditorIO.Util`: LZSS compression, CRC32, struct-based reader/writer extensions.
- Each tool is its own `net8.0` console project referencing the library; `TexEditor` is WinForms (`net8.0-windows`, Windows-only).

## Build / verification

- `dotnet build MTXEditorIO.sln` from the repo root. There are no unit tests; `Test/` is a scratch harness that reads game files from the **hardcoded path** `I:\Projects\MTX Mototrax PRO Modding\...` (edit it, don't assume the drive exists). Do not reach for `dotnet test`.

## Tools

| Tool | Direction | CLI style |
|---|---|---|
| ImgToPng / ToImg | PC `.img` ⇄ PNG | CommandLineParser |
| TexToPng / ToTex | PC `.tex` (BCn) ⇄ PNG (multi-mip) | CommandLineParser |
| Ps2TexToPng | PS2 `.tex` → PNG | raw `args[i]` |
| UnpackPre / PackPre | `.pre` archive ⇄ folder | raw `args[i]` |
| QCompile | `.Zqb` ⇄ `.qcode` script | CommandLineParser |
| GeomToScn | PS2 `.geom.ps2` → PC `.scn.xbx` (THUG1 world) | raw `args[i]` |
| TexEditor | GUI viewer/editor | WinForms |

Conventions are inconsistent on purpose: some tools use `CommandLineParser` (`--compile`, flags), others just read `args[0]`. Match whichever tool you touch.

## Gotchas

- **Vertically flipped images**: game images are stored upside down. PNG export flips while copying (`colors[(height - y - 1) * width + x]`), import flips back. Keep this pattern in new converters.
- **`.img` pixel data is BGR(A), not RGB** (raw mode writes `B, G, R, A` byte order; palettized mode is `Indexed8`).
- For `.pre`: filenames are CRC'd over the **lowercased** name (`PreItem.cs:39`); item data is LZSS-compressed+4-byte-aligned; `PackPre` always writes version `pre4`; `UnpackPre` appends `_dir` to the output folder name because Windows can't have a file and dir with the same name.
- **Zqb chunk registration is reflection-by-name**: every value in the `QbChunkCode` enum must have a matching class `MTXEditorIO.Raw.Zqb.QbChunks.<EnumValueName>`, or the static ctor of `Zqb` throws at runtime.
- **QCompile's `Compile()` (qcode → zqb) is an empty stub** — only decompile works; drag-and-drop dispatch of `.qcode` files calls the stub.
- Format structs are `[StructLayout(LayoutKind.Sequential, Pack = 1)]` and serialized with `ReadStruct<T>`/`WriteStruct<T>` (`Util/BinaryReaderExtensions.cs`, `Util/BinaryWriterExtensions.cs`) — field order *is* the on-disk layout.
- Files are opened with `FileShare.ReadWrite` so the game can hold them open.
- `ToTex` uses a random checksum (`r.Next()`) — a known TODO, not a bug.
- **`GeomPS2` (PS2 `.geom`) is read-only for now**: `WriteTo` throws `NotSupportedException` while the format is reverse engineered. Header at 0x10 holds the offset of a table of 0x50-byte records (`GeomPS2Record`) that runs to EOF; record fields (positions, type byte, data/next-offset pointers, checksum) are partially decoded — unknown fields are named `unknown*`.