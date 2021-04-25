using System;
using System.Collections.Generic;
using NeedsSoySauce.Entities;

namespace NeedsSoySauce.Repositories
{
    public interface IGoonsRepo
    {
        /// <summary>
        /// Returns the Goon with the given id or null if no goon with that id exists.
        /// </summary>
        Goon? GetGoon(string goonId);

        void UpdateLastSeenOnUtc(string goonId);

        /// <summary>
        /// Records this Goon if it hasn't already been recorded.
        /// </summary>
        void RecordGoon(string id);
    }
}