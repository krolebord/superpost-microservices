using System.Reflection;

namespace Migrator;

public static class AssemblyHelpers
{
    public static string GetAssemblyDirectory()
    {
        return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
               ?? throw new Exception("Couldn't get DLL directory path");
    }
}
