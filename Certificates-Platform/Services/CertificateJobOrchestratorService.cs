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

        public void AddJob(IFileProvider pdf, IFileProvider exel)
        {
            int id = availableIds.Dequeue();

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
