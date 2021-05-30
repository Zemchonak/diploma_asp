using System.Collections.Generic;
using System.Globalization;

namespace FitnessCenterManagement.WebApp.Interfaces
{
    public interface ICultureProviderFactory
    {
        IReadOnlyCollection<CultureInfo> GetCultures();
    }
}