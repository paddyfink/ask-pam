using AskPam.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace AskPam.Crm.Application.Features
{
    public class FeatureStore
    {
        private readonly IRepository<OrganizationFeatureSetting> _organizationFeatureRepository;
    }
}
