using System.Net.Http;
using System.Threading.Tasks;

namespace FitnessCenterManagement.WebApp.HttpClients.Interfaces
{
    public interface IServicesApiHttpClient
    {
        public Task<HttpResponseMessage> GetServices(string part = "");

        public Task<HttpResponseMessage> GetServices(int id);

        public Task<HttpResponseMessage> PutServices(int id, StringContent content);

        public Task<HttpResponseMessage> PostServices(StringContent content);

        public Task<HttpResponseMessage> DeleteServices(int id);
    }
}
