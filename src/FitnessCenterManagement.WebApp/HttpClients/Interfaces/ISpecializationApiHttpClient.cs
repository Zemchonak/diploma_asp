using System.Net.Http;
using System.Threading.Tasks;

namespace FitnessCenterManagement.WebApp.HttpClients.Interfaces
{
    public interface ISpecializationApiHttpClient
    {
        public Task<HttpResponseMessage> GetSpecializations(string part = "");

        public Task<HttpResponseMessage> GetSpecializations(int id);

        public Task<HttpResponseMessage> PutSpecializations(int id, StringContent content);

        public Task<HttpResponseMessage> PostSpecializations(StringContent content);

        public Task<HttpResponseMessage> DeleteSpecializations(int id);
    }
}