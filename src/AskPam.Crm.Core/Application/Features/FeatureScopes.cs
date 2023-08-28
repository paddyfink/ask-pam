using System;
using System.Collections.Generic;
using System.Text;

namespace AskPam.Crm.Application.Features
{
    /// <summary>
    /// Scopes of a <see cref="Feature"/>.
    /// </summary>
    [Flags]
    public enum FeatureScopes
    {
        /// <summary>
        /// This <see cref="Feature"/> can be enabled/disabled per edition.
        /// </summary>
        Plan = 1,

        /// <summary>
        /// This Feature<see cref="Feature"/> can be enabled/disabled per tenant.
        /// </summary>
        Organization = 2,

        /// <summary>
        /// This <see cref="Feature"/> can be enabled/disabled per tenant and edition.
        /// </summary>
        All = 3
    }
}
