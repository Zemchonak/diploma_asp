using System.Net.Http;
using System.Threading.Tasks;

namespace FitnessCenterManagement.WebApp.HttpClients.Interfaces
{
    public interface ITrainersApiHttpClient
    {
        public Task<HttpResponseMessage> GetTrainers(string part = "");

        public Task<HttpResponseMessage> GetTrainersBySpec(int specId);

        public Task<HttpResponseMessage> GetTrainers(int id);

        public Task<HttpResponseMessage> PutTrainers(int id, StringContent content);

        public Task<HttpResponseMessage> PostTrainers(StringContent content);

        public Task<HttpResponseMessage> DeleteTrainers(int id);

        public Task<HttpResponseMessage> GetTrainersImage(int id);
    }
}