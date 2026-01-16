using ServiceRegistration;
using Xceed.Words.NET;

namespace FormPlatform.Services
{
    [RegisterAsSingleton]
    public class DocGeneratorService
    {
        public void CreateFilesAndZip(string templatePath, FormData data, string outputPath)
        {
            using (var document = DocX.Load(templatePath))
            {
                for(int i = 0; i < data.awnsers.Length; i++)
                {
                    var awnser = data.awnsers[i];
                    
                    document.ReplaceText("{Index}", (i + 1).ToString());

                    string individualOutputPath = Path.Combine(outputPath, $"Document_{i + 1}.docx");
                }
                document.SaveAs(outputPath);
            }
        }
    }
}
