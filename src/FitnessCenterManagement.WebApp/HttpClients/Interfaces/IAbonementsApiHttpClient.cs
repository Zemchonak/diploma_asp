using System.Net.Http;
using System.Threading.Tasks;

namespace FitnessCenterManagement.WebApp.HttpClients.Interfaces
{
    public interface IAbonementsApiHttpClient
    {
        public Task<HttpResponseMessage> GetAbonements(string part = "");

        public Task<HttpResponseMessage> GetAbonements(int id);

        public Task<HttpResponseMessage> PutAbonements(int id, StringContent content);

        public Task<HttpResponseMessage> PostAbonements(StringContent content);

        public Task<HttpResponseMessage> DeleteAbonements(int id);

        public Task<HttpResponseMessage> GetAbonementsImage(int id);

        public Task<HttpResponseMessage> PutAbonementsImage(int id, MultipartFormDataContent content);
    }
}