using System.Net.Http;
using System.Threading.Tasks;

namespace FitnessCenterManagement.WebApp.HttpClients.Interfaces
{
    public interface IApiHttpClient : 
        IServicesApiHttpClient, 
        ISpecializationApiHttpClient, 
        IVenuesApiHttpClient,
        IReviewsApiHttpClient,
        ICustomerCategoriesApiHttpClient,
        ITrainersApiHttpClient
    {
        // Users
        public Task<HttpResponseMessage> Ping();

        public Task<HttpResponseMessage> GetUserCulture();

        public Task<HttpResponseMessage> GetProfile();

        public Task<HttpResponseMessage> PutProfile(StringContent content);

        public Task<HttpResponseMessage> PostUsersLogin(StringContent content);

        public Task<HttpResponseMessage> PostUsersRegister(StringContent content);

        public Task<HttpResponseMessage> PostUsersChangePassword(StringContent content);

        public Task<HttpResponseMessage> GetUsersValidateToken(string token);

        public Task<HttpResponseMessage> GetUsersLogout();

        public Task<HttpResponseMessage> GetProfileDefaultImage();

        public Task<HttpResponseMessage> GetProfileImage(string userId);

        public Task<HttpResponseMessage> GetProfileQr(string userId);

        public Task<HttpResponseMessage> PutLocalization(StringContent content);

        public Task<HttpResponseMessage> PutProfileBalance(StringContent content);

        public Task<HttpResponseMessage> GetIsEmailFree(string email);

        public Task<HttpResponseMessage> PutProfileImage(MultipartFormDataContent content);

        public Task<HttpResponseMessage> GetUsers(string part);

        public Task<HttpResponseMessage> GetUsers(int id);
    }
}
