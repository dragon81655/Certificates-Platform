using Microsoft.AspNetCore.Mvc;
using ServiceRegistration;

namespace TestFormSite.Services
{

    [RegisterAsTransient]
    public class FileSortingService
    {
        private PathCreationService pathCreationService;
        public FileSortingService(PathCreationService pathCreationService)
        {
            this.pathCreationService = pathCreationService;
        }
        public async void GenerateFiles(IEnumerable<FileData> files, string baseDir)
        {
            for (int i = 0; i < files.Count(); i++)
            {
                FileData file = files.ElementAt(i);
                pathCreationService.CreatePath(file.path);
                
                using StreamWriter writer = new StreamWriter(Path.Combine(file.path, file.fileName));
                writer.Write(file.fileData);
                writer.Close();
            }
        }

        public class FileData
        {
            public string fileName;
            public string path;
            public byte[] fileData;
        }
    }
}
