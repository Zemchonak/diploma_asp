using System.Net.Http;
using System.Threading.Tasks;

namespace FitnessCenterManagement.WebApp.HttpClients.Interfaces
{
    public interface IReviewsApiHttpClient
    {
        public Task<HttpResponseMessage> GetReviews(string part = "");

        public Task<HttpResponseMessage> GetReviewByAuthorId(string authorId);

        public Task<HttpResponseMessage> GetReviews(int id);

        public Task<HttpResponseMessage> PutReviews(int id, StringContent content);

        public Task<HttpResponseMessage> PostReviews(StringContent content);

        public Task<HttpResponseMessage> DeleteReviews(int id);
    }
}