﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessCenterManagement.WebApp
{
    public class Constants
    {
        internal const string UserIdClaimType = "UserId";
        internal const decimal MinBalance = 0.0m;

        internal const int AbonementAttendancesMinimum = 1;

        internal const decimal CoefficientMinimum = 0.0m;
        internal const decimal CoefficientMaximum = 2.0m;

        public const string TrainerRole = "Trainer";
        public const string UserRole = "User";
        public const string ManagerRole = "Manager";
        public const string DirectorRole = "Director";
        public const string MarketerRole = "Marketer";

    }
}
