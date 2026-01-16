using System.IO;
using ServiceRegistration;
namespace TestFormSite.Services
{
    [RegisterAsSingleton]
    public class PathCreationService
    {
        public string CreateTempPath(string path)
        {
            string[] segments = { Path.GetTempPath() };
            segments.Concat(path.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));
            return CreatePath(segments);
        }
        public string CreatePath(string path)
        {
            string[] segments = path.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            return CreatePath(segments);
        }
        public string CreatePath(params string[] segments)
        {
            
            string tempPath = Path.GetTempPath();
            foreach (string segment in segments)
            {
                string t = Sanitize(segment);
                tempPath = Path.Combine(tempPath, t);
            }
            if(!Directory.Exists(tempPath))
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
