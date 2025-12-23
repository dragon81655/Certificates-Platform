using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using ServiceRegistration;

namespace Certificates_Platform.Services
{
    [RegisterAsSingleton]
    public class CertificateJobOrchestratorService
    {
        public List<JobInfo> jobInfos = new List<JobInfo>();
        public Queue<int> availableIds = new Queue<int>();

        public int forceCheckUp;

        public CertificateJobOrchestratorService(IOptions<GenerationSettings> options)
        {
            //Add property from config
            int amount = options.Value.maximumAmountOfProcesses;
            forceCheckUp = options.Value.forceScanvanger;

            for (int i = 1; i <= amount; i++)
            {
                availableIds.Enqueue(i);
            }
        }
        public int CreateFilesAndID(IFormFile pdfF, IFormFile exelF)
        {
            if(availableIds.Count == 0)
            {
                return -1;
            }

            string pdfN = Path.GetFileName(pdfF.FileName);
            string exelN = Path.GetFileName(exelF.FileName);
            int toReturn = availableIds.Dequeue();

            pdfF.OpenReadStream().CopyTo(new FileStream($"{pdfN}", FileMode.Create));

            return toReturn;
        }
        public void AddJob(int id)
        {
            //GET PATH AND GEN FILES

            jobInfos.Add(new JobInfo
            {
                jobID = id,
                CreatedAt = DateTime.Now,
                Status = "Pending"
            });
        }
    }

    public struct JobInfo
    {
        public int jobID;
        public DateTime CreatedAt;
        public string Status;
    }
}
