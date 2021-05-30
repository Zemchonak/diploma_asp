using System.Net.Http;
using System.Threading.Tasks;

namespace FitnessCenterManagement.WebApp.HttpClients.Interfaces
{
    public interface IVenuesApiHttpClient
    {
        public Task<HttpResponseMessage> GetVenues(string part = "");

        public Task<HttpResponseMessage> GetVenues(int id);

        public Task<HttpResponseMessage> PutVenues(int id, StringContent content);

        public Task<HttpResponseMessage> PostVenues(StringContent content);

        public Task<HttpResponseMessage> DeleteVenues(int id);

        public Task<HttpResponseMessage> GetVenuesImage(int id);

        public Task<HttpResponseMessage> GetVenuesQr(int id);

        public Task<HttpResponseMessage> PutVenuesImage(int id, MultipartFormDataContent content);
    }
}