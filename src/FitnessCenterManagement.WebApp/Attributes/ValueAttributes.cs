using System.ComponentModel.DataAnnotations;

namespace FitnessCenterManagement.WebApp.Attributes
{
    /// <summary>
    /// Checks if not less than Constants.CoefficientMinimum
    /// </summary>
    public class MinCoefficientValueAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is null || (decimal) value < Constants.CoefficientMinimum)
            {
                return false;
            }
            return true;
        }
    }

    /// <summary>
    /// Checks if not greater than Constants.CoefficientMaximum
    /// </summary>
    public class MaxCoefficientValueAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is null || (decimal) value > Constants.CoefficientMaximum)
            {
                return false;
            }
            return true;
        }
    }

    /// <summary>
    /// Checks if not greater than Constants.AbonementAttendancesMinimum
    /// </summary>
    public class MinAttendencesValueAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is null || (int) value < Constants.AbonementAttendancesMinimum)
            {
                return false;
            }
            return true;
        }
    }
}
