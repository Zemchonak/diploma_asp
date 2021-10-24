using System.Net.Http;
using System.Threading.Tasks;

namespace FitnessCenterManagement.WebApp.HttpClients.Interfaces
{
    public interface IWeeklyEventsApiHttpClient
    {
        public Task<HttpResponseMessage> GetWeeklyEvents(string part = "");

        public Task<HttpResponseMessage> GetWeeklyEvents(int id);

        public Task<HttpResponseMessage> PutWeeklyEvents(int id, StringContent content);

        public Task<HttpResponseMessage> PostWeeklyEvents(StringContent content);

        public Task<HttpResponseMessage> DeleteWeeklyEvents(int id);
    }
}