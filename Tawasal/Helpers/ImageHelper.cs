
namespace Tawasal.Helpers
{
    public static class ImageHelper
    {
        public static bool FileExists(string filePath)
        {
            return File.Exists(filePath);
        }
    }
}