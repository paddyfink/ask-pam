using AskPam.Crm.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AskPam.Crm.Application.Features
{
    public class OrganizationFeatureSetting : FeatureSetting, IMustHaveOrganization
    {
        public Guid OrganizationId { get; set ; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TenantFeatureSetting"/> class.
        /// </summary>
        public OrganizationFeatureSetting()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrganizationFeatureSetting"/> class.
        /// </summary>
        /// <param name="organizationId">The tenant identifier.</param>
        /// <param name="name">Feature name.</param>
        /// <param name="value">Feature value.</param>
        public OrganizationFeatureSetting(Guid organizationId, string name, string value)
            :base(name, value)
        {
            OrganizationId = organizationId;
        }
    }
}
