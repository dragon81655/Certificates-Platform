using System.IO;
using ServiceRegistration;
namespace Certificates_Platform.Services
{
    [RegisterAsSingleton]
    public class PathCreationService
    {
        public string CreatePath(string path)
        {
            string[] segments = path.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            return CreatePath(segments);
        }


        public string CreatePath(params string[] segments)
        {
            string tempPath = Path.GetTempPath();
            foreach(string segment in segments)
            {
                string t = Sanitize(segment);
                tempPath = Path.Combine(tempPath, t);
            }
            Directory.CreateDirectory(tempPath);
            return tempPath;
        }
        private static string Sanitize(string segment)
        {
            foreach (var c in Path.GetInvalidFileNameChars())
                segment = segment.Replace(c, '_');

            return segment;
        }
    }
}
