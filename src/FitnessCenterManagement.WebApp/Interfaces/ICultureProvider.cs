using System.Collections.Generic;
using System.Globalization;

namespace FitnessCenterManagement.WebApp.Interfaces
{
    public interface ICultureProvider
    {
        IReadOnlyCollection<CultureInfo> Cultures { get; }

        string GetFullCulture(string twoLetteredCulture);
    }
}