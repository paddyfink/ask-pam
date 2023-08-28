using AskPam.Crm.Authorization;
using AskPam.Crm.Organizations;
using System;
using System.Collections.Generic;
using System.Text;

namespace AskPam.Crm.Runtime.Session
{
    public interface ICrmSession
    {
        /// <summary>
        /// Gets current UserId or null.
        /// It can be null if no user logged in.
        /// </summary>
        string UserId { get; }


        /// <summary>
        /// Gets current User or null.
        /// </summary>
        //User User { get; }

        /// <summary>
        /// Gets current Account or null.
        /// This TenantId should be the TenantId of the <see cref="UserId"/>.
        /// It can be null if is a host user or no user logged in.
        /// </summary>
        Guid? OrganizationId { get; }


        /// <summary>
        /// Gets current Organixation or null.
        /// </summary>
        //Organization Organization { get; }

}
}
