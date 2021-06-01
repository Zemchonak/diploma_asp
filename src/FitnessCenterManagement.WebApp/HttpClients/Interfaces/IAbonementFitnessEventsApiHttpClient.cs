using System.Net.Http;
using System.Threading.Tasks;

namespace FitnessCenterManagement.WebApp.HttpClients.Interfaces
{
    public interface IAbonementFitnessEventsApiHttpClient
    {
        public Task<HttpResponseMessage> GetAbonementFitnessEventsByAbonement(int abonementId);

        public Task<HttpResponseMessage> GetAbonementFitnessEvents(int id);

        public Task<HttpResponseMessage> PutAbonementFitnessEvents(int id, StringContent content);

        public Task<HttpResponseMessage> PostAbonementFitnessEvents(StringContent content);

        public Task<HttpResponseMessage> DeleteAbonementFitnessEvents(int id);
    }
}