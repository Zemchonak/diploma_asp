using System;

namespace FitnessCenterManagement.WebApp.Models
{
    public class WeeklyEventModel
    {
        public int Id { get; set; }

        public string FitnessEventInfo { get; set; }

        public string Location { get; set; }

        public int FitnessEventId { get; set; }

        public int DayOfWeek { get; set; }

        public DateTimeOffset Time { get; set; }

        public int VisitorCapacity { get; set; }
    }
}
