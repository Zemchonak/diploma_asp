using System.Net.Http;
using System.Threading.Tasks;

namespace FitnessCenterManagement.WebApp.HttpClients.Interfaces
{
    public interface ICustomerCategoriesApiHttpClient
    {
        public Task<HttpResponseMessage> GetCustomerCategories(string part = "");

        public Task<HttpResponseMessage> GetCustomerCategories(int id);

        public Task<HttpResponseMessage> PutCustomerCategories(int id, StringContent content);

        public Task<HttpResponseMessage> PostCustomerCategories(StringContent content);

        public Task<HttpResponseMessage> DeleteCustomerCategories(int id);
    }
}