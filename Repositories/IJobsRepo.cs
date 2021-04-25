using System;
using System.Collections.Generic;
using NeedsSoySauce.Data;
using NeedsSoySauce.Entities;

namespace NeedsSoySauce.Repositories
{
    public interface IJobsRepo
    {
        Job CreateJob(string goonId);

        void RemoveJob(Guid jobId);
    }
}