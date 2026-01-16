using Microsoft.Extensions.Options;
using ServiceRegistration;

namespace Certificates_Platform.Services
{
    [RegisterAsSingleton]
    public class CertificateJobOrchestratorService
    {
        private PathCreationService _pathCreationService;

        public List<JobInfo> jobInfos;
        public Queue<int> availableIds = new Queue<int>();

        public int forceCheckUp;
        public string outputPath = "";
        public CertificateJobOrchestratorService(IOptions<GenerationSettings> options, PathCreationService serv)
        {
            //Add property from config
            _pathCreationService = serv;

            int amount = options.Value.maximumAmountOfProcesses;
            jobInfos = new List<JobInfo>();
            jobInfos.AddRange(Enumerable.Repeat(new JobInfo(), amount));
            forceCheckUp = options.Value.forceScanvanger;
            outputPath = _pathCreationService.CreatePath(options.Value.pathToOutput);
            Console.WriteLine($"Output path: {outputPath}");
            for (int i = 1; i <= amount; i++)
            {
                availableIds.Enqueue(i);
            }
        }

        public bool HasJob(int id)
        {
            return jobInfos[id].status == JobInfo.Status.NotActive;
        }

        public bool IsJobCompleted(int id)
        {
            return jobInfos[id].status == JobInfo.Status.Completed;
        }

        public int CreateFilesAndID(IFormFile pdfF, IFormFile exelF)
        {
            if(availableIds.Count == 0)
            {
                return -1;
            }

           
            int toReturn = availableIds.Dequeue();

            Directory.CreateDirectory(outputPath + Path.DirectorySeparatorChar + "id-" + toReturn.ToString());

            string pdfN = Path.Combine(outputPath, Path.Combine("id-"+toReturn.ToString(), Path.GetFileName(pdfF.FileName)));
            string exelN = Path.Combine(outputPath, Path.Combine("id-"+toReturn.ToString(),Path.GetFileName(exelF.FileName)));
            Console.WriteLine($"Creating files for job ID: {pdfN}");
            Console.WriteLine($"Creating files for job ID: {exelN}");
            pdfF.OpenReadStream().CopyTo(new FileStream($"{pdfN}", FileMode.Create));
            exelF.OpenReadStream().CopyTo(new FileStream($"{exelN}", FileMode.Create));
            AddJob(toReturn);

            return toReturn;
        }
        private void AddJob(int id)
        {
            //GET PATH AND GEN FILES

            jobInfos[id] = new JobInfo
            {
                jobID = id,
                CreatedAt = DateTime.Now,
                status = JobInfo.Status.Pending
            };
        }
    }

    public struct JobInfo
    {
        public int jobID;
        public DateTime CreatedAt;
        public Status status;

        public JobInfo()
        {
            jobID = -1;
            CreatedAt = DateTime.MinValue;
            status = Status.NotActive;
        }

        public enum Status
        {
            NotActive = 0,
            Pending,
            InProgress,
            Completed
        }
    }
}
