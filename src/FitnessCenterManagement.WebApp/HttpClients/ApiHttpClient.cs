using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using FitnessCenterManagement.WebApp.HttpClients.Interfaces;
using FitnessCenterManagement.WebApp.Helpers;

namespace FitnessCenterManagement.WebApp.HttpClients
{
    public class ApiHttpClient : BaseApiHttpClient, IApiHttpClient
    {
        public ApiHttpClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
            : base(httpClient, httpContextAccessor)
        {
        }

        // USERS

        public async Task<HttpResponseMessage> GetUserCulture()
        {
            RequestHelper.SetRequestToken(Client, HttpContextAccessor);
            var result = await Client.GetAsync(SetRequestPath("Localization/"));
            return result;
        }

        // AUTH

        public async Task<HttpResponseMessage> PostUsersLogin(StringContent content)
        {
            var result = await Client.PostAsync(SetRequestPath("Auth/login"), content);
            return result;
        }

        public async Task<HttpResponseMessage> GetUsersValidateToken(string token)
        {
            using var content = JsonHelper.ObjectToStringContent(token);
            var result = await Client.PostAsync(SetRequestPath("Auth/validate"), content);
            return result;
        }

        public async Task<HttpResponseMessage> PostUsersRegister(StringContent content)
        {
            var result = await Client.PostAsync(SetRequestPath("Auth/register"), content);
            return result;
        }

        public async Task<HttpResponseMessage> PostUsersChangePassword(StringContent content)
        {
            RequestHelper.SetRequestToken(Client, HttpContextAccessor);
            var result = await Client.PostAsync(SetRequestPath("Auth/changePassword"), content);
            return result;
        }

        public async Task<HttpResponseMessage> GetUsersLogout()
        {
            var result = await Client.GetAsync(SetRequestPath("Auth/logout"));
            return result;
        }

        // PROFILE

        public async Task<HttpResponseMessage> GetProfile()
        {
            RequestHelper.SetRequestToken(Client, HttpContextAccessor);
            var result = await Client.GetAsync(SetRequestPath("Profile/"));
            return result;
        }

        public async Task<HttpResponseMessage> PutProfile(StringContent content)
        {
            RequestHelper.SetRequestToken(Client, HttpContextAccessor);
            var result = await Client.PutAsync(SetRequestPath("Profile/"), content);
            return result;
        }

        public async Task<HttpResponseMessage> PutLocalization(StringContent content)
        {
            RequestHelper.SetRequestToken(Client, HttpContextAccessor);
            var result = await Client.PutAsync(SetRequestPath("Localization/"), content);
            return result;
        }

        public async Task<HttpResponseMessage> GetProfileImage(string userId)
        {
            var result = await Client.GetAsync(SetRequestPath($"Profile/{userId}/image"));
            return result;
        }
        public async Task<HttpResponseMessage> GetProfileDefaultImage()
        {
            var result = await Client.GetAsync(SetRequestPath($"Profile/image"));
            return result;
        }

        public async Task<HttpResponseMessage> GetProfileQr(string userId)
        {
            RequestHelper.SetRequestToken(Client, HttpContextAccessor);
            var result = await Client.GetAsync(SetRequestPath($"Profile/{userId}/qr"));
            return result;
        }

        public async Task<HttpResponseMessage> Ping()
        {
            var result = await Client.GetAsync(SetRequestPath("Auth/ping"));
            return result;
        }

        public async Task<HttpResponseMessage> PutProfileBalance(StringContent content)
        {
            RequestHelper.SetRequestToken(Client, HttpContextAccessor);
            var result = await Client.PutAsync(SetRequestPath("Profile/balance"), content);
            return result;
        }

        public async Task<HttpResponseMessage> GetIsEmailFree(string email)
        {
            RequestHelper.SetRequestToken(Client, HttpContextAccessor);
            var result = await Client.GetAsync(SetRequestPath($"Auth/checkIfEmailFree/{email}"));
            return result;
        }

        public async Task<HttpResponseMessage> PutProfileImage(MultipartFormDataContent content)
        {
            RequestHelper.SetRequestToken(Client, HttpContextAccessor);
            var result = await Client.PutAsync(SetRequestPath($"Profile/avatar"), content);
            return result;
        }

        // SPECIALIZATIONS

        public async Task<HttpResponseMessage> GetSpecializations(string part)
        {
            var reqPath = "Specializations";
            if (!string.IsNullOrEmpty(part))
            {
                reqPath += $"?part={part}";
            }
            RequestHelper.SetRequestToken(Client, HttpContextAccessor);
            var result = await Client.GetAsync(SetRequestPath(reqPath));
            return result;
        }

        public async Task<HttpResponseMessage> GetSpecializations(int id)
        {
            RequestHelper.SetRequestToken(Client, HttpContextAccessor);
            var result = await Client.GetAsync(SetRequestPath($"Specializations/{id}"));
            return result;
        }

        public async Task<HttpResponseMessage> PutSpecializations(int id, StringContent content)
        {
            RequestHelper.SetRequestToken(Client, HttpContextAccessor);
            var result = await Client.PutAsync(SetRequestPath($"Specializations/{id}"), content);
            return result;
        }

        public async Task<HttpResponseMessage> PostSpecializations(StringContent content)
        {
            RequestHelper.SetRequestToken(Client, HttpContextAccessor);
            var result = await Client.PostAsync(SetRequestPath($"Specializations/"), content);
            return result;
        }

        public async Task<HttpResponseMessage> DeleteSpecializations(int id)
        {
            RequestHelper.SetRequestToken(Client, HttpContextAccessor);
            var result = await Client.DeleteAsync(SetRequestPath($"Specializations/{id}"));
            return result;
        }

        // TRAINERS

        public async Task<HttpResponseMessage> GetTrainers(string part)
        {
            var reqPath = "Trainers";
            if (!string.IsNullOrEmpty(part))
            {
                reqPath += $"?part={part}";
            }
            var result = await Client.GetAsync(SetRequestPath($"{reqPath}"));
            return result;
        }

        public async Task<HttpResponseMessage> GetTrainers(int id)
        {
            RequestHelper.SetRequestToken(Client, HttpContextAccessor);
            var result = await Client.GetAsync(SetRequestPath($"Trainers/{id}"));
            return result;
        }
        
        public async Task<HttpResponseMessage> GetTrainersBySpec(int specId)
        {
            RequestHelper.SetRequestToken(Client, HttpContextAccessor);
            var result = await Client.GetAsync(SetRequestPath($"Trainers/bySpec/{specId}"));
            return result;
        }

        public async Task<HttpResponseMessage> PutTrainers(int id, StringContent content)
        {
            RequestHelper.SetRequestToken(Client, HttpContextAccessor);
            var result = await Client.PutAsync(SetRequestPath($"Trainers/{id}"), content);
            return result;
        }

        public async Task<HttpResponseMessage> PostTrainers(StringContent content)
        {
            RequestHelper.SetRequestToken(Client, HttpContextAccessor);
            var result = await Client.PostAsync(SetRequestPath($"Trainers/"), content);
            return result;
        }

        public async Task<HttpResponseMessage> DeleteTrainers(int id)
        {
            RequestHelper.SetRequestToken(Client, HttpContextAccessor);
            var result = await Client.DeleteAsync(SetRequestPath($"Trainers/{id}"));
            return result;
        }

        public async Task<HttpResponseMessage> GetTrainersImage(int id)
        {
            var result = await Client.GetAsync(SetRequestPath($"Trainers/{id}/image"));
            return result;
        }

        public async Task<HttpResponseMessage> GetUsers(string part)
        {
            var reqPath = "Users";
            if (!string.IsNullOrEmpty(part))
            {
                reqPath += $"?part={part}";
            }
            RequestHelper.SetRequestToken(Client, HttpContextAccessor);
            var result = await Client.GetAsync(SetRequestPath($"{reqPath}"));
            return result;
        }

        public async Task<HttpResponseMessage> GetUsers(int id)
        {
            RequestHelper.SetRequestToken(Client, HttpContextAccessor);
            var result = await Client.GetAsync(SetRequestPath($"Users/{id}"));
            return result;
        }

        // VENUES

        public async Task<HttpResponseMessage> GetVenues(string part)
        {
            var reqPath = "Venues";
            if (!string.IsNullOrEmpty(part))
            {
                reqPath += $"?part={part}";
            }
            RequestHelper.SetRequestToken(Client, HttpContextAccessor);
            var result = await Client.GetAsync(SetRequestPath($"{reqPath}"));
            return result;
        }

        public async Task<HttpResponseMessage> GetVenues(int id)
        {
            var result = await Client.GetAsync(SetRequestPath($"Venues/{id}"));
            return result;
        }

        public async Task<HttpResponseMessage> PutVenues(int id, StringContent content)
        {
            RequestHelper.SetRequestToken(Client, HttpContextAccessor);
            var result = await Client.PutAsync(SetRequestPath($"Venues/{id}"), content);
            return result;
        }

        public async Task<HttpResponseMessage> PostVenues(StringContent content)
        {
            RequestHelper.SetRequestToken(Client, HttpContextAccessor);
            var result = await Client.PostAsync(SetRequestPath($"Venues/"), content);
            return result;
        }

        public async Task<HttpResponseMessage> DeleteVenues(int id)
        {
            RequestHelper.SetRequestToken(Client, HttpContextAccessor);
            var result = await Client.DeleteAsync(SetRequestPath($"Venues/{id}"));
            return result;
        }

        public async Task<HttpResponseMessage> GetVenuesImage(int id)
        {
            var result = await Client.GetAsync(SetRequestPath($"Venues/{id}/image"));
            return result;
        }

        public async Task<HttpResponseMessage> GetVenuesQr(int id)
        {
            RequestHelper.SetRequestToken(Client, HttpContextAccessor);
            var result = await Client.GetAsync(SetRequestPath($"Venues/{id}/qr"));
            return result;
        }

        public async Task<HttpResponseMessage> PutVenuesImage(int id, MultipartFormDataContent content)
        {
            RequestHelper.SetRequestToken(Client, HttpContextAccessor);
            var result = await Client.PutAsync(SetRequestPath($"Venues/{id}/image"), content);
            return result;
        }

        // CUSTOMER CATEGORIES

        public async Task<HttpResponseMessage> GetCustomerCategories(string part)
        {
            var reqPath = "CustomerCategories";
            if (!string.IsNullOrEmpty(part))
            {
                reqPath += $"?part={part}";
            }
            var result = await Client.GetAsync(SetRequestPath(reqPath));
            return result;
        }

        public async Task<HttpResponseMessage> GetCustomerCategories(int id)
        {
            RequestHelper.SetRequestToken(Client, HttpContextAccessor);
            var result = await Client.GetAsync(SetRequestPath($"CustomerCategories/{id}"));
            return result;
        }

        public async Task<HttpResponseMessage> PutCustomerCategories(int id, StringContent content)
        {
            RequestHelper.SetRequestToken(Client, HttpContextAccessor);
            var result = await Client.PutAsync(SetRequestPath($"CustomerCategories/{id}"), content);
            return result;
        }

        public async Task<HttpResponseMessage> PostCustomerCategories(StringContent content)
        {
            RequestHelper.SetRequestToken(Client, HttpContextAccessor);
            var result = await Client.PostAsync(SetRequestPath($"CustomerCategories/"), content);
            return result;
        }

        public async Task<HttpResponseMessage> DeleteCustomerCategories(int id)
        {
            RequestHelper.SetRequestToken(Client, HttpContextAccessor);
            var result = await Client.DeleteAsync(SetRequestPath($"CustomerCategories/{id}"));
            return result;
        }

        // SERVICES

        public async Task<HttpResponseMessage> GetServices(string part)
        {
            var reqPath = "Services";
            if (!string.IsNullOrEmpty(part))
            {
                reqPath += $"?part={part}";
            }
            var result = await Client.GetAsync(SetRequestPath(reqPath));
            return result;
        }

        public async Task<HttpResponseMessage> GetServices(int id)
        {
            RequestHelper.SetRequestToken(Client, HttpContextAccessor);
            var result = await Client.GetAsync(SetRequestPath($"Services/{id}"));
            return result;
        }

        public async Task<HttpResponseMessage> PutServices(int id, StringContent content)
        {
            RequestHelper.SetRequestToken(Client, HttpContextAccessor);
            var result = await Client.PutAsync(SetRequestPath($"Services/{id}"), content);
            return result;
        }

        public async Task<HttpResponseMessage> PostServices(StringContent content)
        {
            RequestHelper.SetRequestToken(Client, HttpContextAccessor);
            var result = await Client.PostAsync(SetRequestPath($"Services/"), content);
            return result;
        }

        public async Task<HttpResponseMessage> DeleteServices(int id)
        {
            RequestHelper.SetRequestToken(Client, HttpContextAccessor);
            var result = await Client.DeleteAsync(SetRequestPath($"Services/{id}"));
            return result;
        }

        // REVIEWS

        public async Task<HttpResponseMessage> GetReviews(string part)
        {
            RequestHelper.SetRequestToken(Client, HttpContextAccessor);
            var reqPath = "Reviews";
            if (!string.IsNullOrEmpty(part))
            {
                reqPath += $"?part={part}";
            }
            var result = await Client.GetAsync(SetRequestPath(reqPath));
            return result;
        }

        public async Task<HttpResponseMessage> GetReviews(int id)
        {
            RequestHelper.SetRequestToken(Client, HttpContextAccessor);
            var result = await Client.GetAsync(SetRequestPath($"Reviews/{id}"));
            return result;
        }

        public async Task<HttpResponseMessage> GetReviewByAuthorId(string authorId)
        {
            RequestHelper.SetRequestToken(Client, HttpContextAccessor);
            var result = await Client.GetAsync(SetRequestPath($"Reviews/byAuthor/{authorId}"));
            return result;
        }

        public async Task<HttpResponseMessage> PutReviews(int id, StringContent content)
        {
            RequestHelper.SetRequestToken(Client, HttpContextAccessor);
            var result = await Client.PutAsync(SetRequestPath($"Reviews/{id}"), content);
            return result;
        }

        public async Task<HttpResponseMessage> PostReviews(StringContent content)
        {
            RequestHelper.SetRequestToken(Client, HttpContextAccessor);
            var result = await Client.PostAsync(SetRequestPath($"Reviews/"), content);
            return result;
        }

        public async Task<HttpResponseMessage> DeleteReviews(int id)
        {
            RequestHelper.SetRequestToken(Client, HttpContextAccessor);
            var result = await Client.DeleteAsync(SetRequestPath($"Reviews/{id}"));
            return result;
        }

        // ABONEMENTS

        public async Task<HttpResponseMessage> GetAbonements(string part)
        {
            RequestHelper.SetRequestToken(Client, HttpContextAccessor);
            var reqPath = "Abonements";
            if (!string.IsNullOrEmpty(part))
            {
                reqPath += $"?part={part}";
            }
            var result = await Client.GetAsync(SetRequestPath(reqPath));
            return result;
        }

        public async Task<HttpResponseMessage> GetAbonements(int id)
        {
            RequestHelper.SetRequestToken(Client, HttpContextAccessor);
            var result = await Client.GetAsync(SetRequestPath($"Abonements/{id}"));
            return result;
        }

        public async Task<HttpResponseMessage> GetAbonementsPrice(int id)
        {
            RequestHelper.SetRequestToken(Client, HttpContextAccessor);
            var result = await Client.GetAsync(SetRequestPath($"Abonements/{id}/price"));
            return result;
        }

        public async Task<HttpResponseMessage> GetAbonementByAuthorId(string authorId)
        {
            RequestHelper.SetRequestToken(Client, HttpContextAccessor);
            var result = await Client.GetAsync(SetRequestPath($"Abonements/byAuthor/{authorId}"));
            return result;
        }

        public async Task<HttpResponseMessage> PutAbonements(int id, StringContent content)
        {
            RequestHelper.SetRequestToken(Client, HttpContextAccessor);
            var result = await Client.PutAsync(SetRequestPath($"Abonements/{id}"), content);
            return result;
        }

        public async Task<HttpResponseMessage> PostAbonements(StringContent content)
        {
            RequestHelper.SetRequestToken(Client, HttpContextAccessor);
            var result = await Client.PostAsync(SetRequestPath($"Abonements/"), content);
            return result;
        }

        public async Task<HttpResponseMessage> DeleteAbonements(int id)
        {
            RequestHelper.SetRequestToken(Client, HttpContextAccessor);
            var result = await Client.DeleteAsync(SetRequestPath($"Abonements/{id}"));
            return result;
        }

        public async Task<HttpResponseMessage> GetAbonementsImage(int id)
        {
            var result = await Client.GetAsync(SetRequestPath($"Abonements/{id}/image"));
            return result;
        }
        public async Task<HttpResponseMessage> PutAbonementsImage(int id, MultipartFormDataContent content)
        {
            RequestHelper.SetRequestToken(Client, HttpContextAccessor);
            var result = await Client.PutAsync(SetRequestPath($"Abonements/{id}/image"), content);
            return result;
        }

        // FITNESS EVENTS

        public async Task<HttpResponseMessage> GetFitnessEvents(string part)
        {
            var reqPath = "FitnessEvents";
            if (!string.IsNullOrEmpty(part))
            {
                reqPath += $"?part={part}";
            }
            RequestHelper.SetRequestToken(Client, HttpContextAccessor);
            var result = await Client.GetAsync(SetRequestPath(reqPath));
            return result;
        }

        public async Task<HttpResponseMessage> GetFitnessEvents(int id)
        {
            RequestHelper.SetRequestToken(Client, HttpContextAccessor);
            var result = await Client.GetAsync(SetRequestPath($"FitnessEvents/{id}"));
            return result;
        }

        public async Task<HttpResponseMessage> PutFitnessEvents(int id, StringContent content)
        {
            RequestHelper.SetRequestToken(Client, HttpContextAccessor);
            var result = await Client.PutAsync(SetRequestPath($"FitnessEvents/{id}"), content);
            return result;
        }

        public async Task<HttpResponseMessage> PostFitnessEvents(StringContent content)
        {
            RequestHelper.SetRequestToken(Client, HttpContextAccessor);
            var result = await Client.PostAsync(SetRequestPath($"FitnessEvents/"), content);
            return result;
        }

        public async Task<HttpResponseMessage> DeleteFitnessEvents(int id)
        {
            RequestHelper.SetRequestToken(Client, HttpContextAccessor);
            var result = await Client.DeleteAsync(SetRequestPath($"FitnessEvents/{id}"));
            return result;
        }

        // ABONEMENT FITNESS EVENTS

        public async Task<HttpResponseMessage> GetAbonementFitnessEventsByAbonement(int abonementId)
        {
            var reqPath = "AbonementFitnessEvents";
            if (abonementId > 0)
            {
                reqPath += $"?abonementId={abonementId}";
            }
            RequestHelper.SetRequestToken(Client, HttpContextAccessor);
            var result = await Client.GetAsync(SetRequestPath(reqPath));
            return result;
        }

        public async Task<HttpResponseMessage> GetAbonementFitnessEvents(int id)
        {
            RequestHelper.SetRequestToken(Client, HttpContextAccessor);
            var result = await Client.GetAsync(SetRequestPath($"AbonementFitnessEvents/{id}"));
            return result;
        }

        public async Task<HttpResponseMessage> PutAbonementFitnessEvents(int id, StringContent content)
        {
            RequestHelper.SetRequestToken(Client, HttpContextAccessor);
            var result = await Client.PutAsync(SetRequestPath($"AbonementFitnessEvents/{id}"), content);
            return result;
        }

        public async Task<HttpResponseMessage> PostAbonementFitnessEvents(StringContent content)
        {
            RequestHelper.SetRequestToken(Client, HttpContextAccessor);
            var result = await Client.PostAsync(SetRequestPath($"AbonementFitnessEvents"), content);
            return result;
        }

        public async Task<HttpResponseMessage> DeleteAbonementFitnessEvents(int id)
        {
            RequestHelper.SetRequestToken(Client, HttpContextAccessor);
            var result = await Client.DeleteAsync(SetRequestPath($"AbonementFitnessEvents/{id}"));
            return result;
        }
    }
}
