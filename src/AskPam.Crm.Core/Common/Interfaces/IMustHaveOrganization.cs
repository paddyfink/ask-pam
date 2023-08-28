using System;
using System.Collections.Generic;
using System.Text;

namespace AskPam.Crm.Common.Interfaces
{
    //
    // Summary:
    //     Implement this interface for an entity which must have TenantId.
    public interface IMustHaveOrganization
    {
        //
        // Summary:
        //     AccountId of this entity.
        Guid OrganizationId { get; set; }
    }
}
