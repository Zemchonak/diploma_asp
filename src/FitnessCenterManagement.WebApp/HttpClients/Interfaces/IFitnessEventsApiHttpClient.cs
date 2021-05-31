using System.Net.Http;
using System.Threading.Tasks;

namespace FitnessCenterManagement.WebApp.HttpClients.Interfaces
{
    public interface IFitnessEventsApiHttpClient
    {
        public Task<HttpResponseMessage> GetFitnessEvents(string part = "");

        public Task<HttpResponseMessage> GetFitnessEvents(int id);

        public Task<HttpResponseMessage> PutFitnessEvents(int id, StringContent content);

        public Task<HttpResponseMessage> PostFitnessEvents(StringContent content);

        public Task<HttpResponseMessage> DeleteFitnessEvents(int id);
    }
}