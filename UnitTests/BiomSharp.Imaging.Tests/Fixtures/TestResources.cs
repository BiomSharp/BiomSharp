using System.Runtime.CompilerServices;

namespace BiomSharp.Imaging.Tests.Fixtures;

internal static class TestResources
{
    static TestResources()
    {
        UnitTestsDirectory = FindUnitTestsDirectory();
        DatasetDirectory = Path.Combine(UnitTestsDirectory, _refImageFolderName);
    }

    private const string _refImageFolderName = "reference_images_v2.0_pgm";
    private static string FindUnitTestsDirectory([CallerFilePath] string sourcePath = "") =>
        Path.GetFullPath(Path.Combine(Path.GetDirectoryName(sourcePath)!, "..", ".."));

    public static readonly string DatasetDirectory;
    public static readonly string UnitTestsDirectory;
}
