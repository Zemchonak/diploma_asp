using FitnessCenterManagement.WebApp.Interfaces;
using System.Collections.Generic;
using System.Globalization;

namespace FitnessCenterManagement.WebApp.Helpers
{
    public class CultureProviderFactory : ICultureProviderFactory
    {
        public IReadOnlyCollection<CultureInfo> GetCultures()
        {
            return new List<CultureInfo>
            {
                new CultureInfo("en-US"),
                new CultureInfo("ru-RU"),
                new CultureInfo("be-BY"),
            }.AsReadOnly();
        }
    }
}