using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using FitnessCenterManagement.WebApp.Interfaces;

namespace FitnessCenterManagement.WebApp.Helpers
{
    internal class CultureProvider : ICultureProvider
    {
        public CultureProvider(ICultureProviderFactory factory)
        {
            Cultures = factory.GetCultures();
        }

        public IReadOnlyCollection<CultureInfo> Cultures { get; set; }

        public string GetFullCulture(string twoLetteredCulture)
        {
            var value = Cultures.FirstOrDefault(one => one.Name
                .Contains(twoLetteredCulture, System.StringComparison.InvariantCultureIgnoreCase));
            return (value is null) ? twoLetteredCulture : value.Name;
        }
    }
}