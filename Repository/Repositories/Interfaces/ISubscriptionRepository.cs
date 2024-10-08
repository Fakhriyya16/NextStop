﻿using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories.Interfaces
{
    public interface ISubscriptionRepository : IBaseRepository<Subscription>
    {
        Task<Subscription> GetByUserId(string userId);
        Task DeleteFromDatabase(Subscription subscription);
    }
}
