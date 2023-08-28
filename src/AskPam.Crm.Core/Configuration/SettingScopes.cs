using System;
using System.Collections.Generic;
using System.Text;

namespace AskPam.Crm.Configuration
{
    /// <summary>
    /// Defines scope of a setting.
    /// </summary>
    [Flags]
    public enum SettingScopes
    {
        /// <summary>
        /// Represents a setting that can be configured/changed for the application level.
        /// </summary>
        Application = 1,

        /// <summary>
        /// Represents a setting that can be configured/changed for each Organization.
        /// This is reserved
        /// </summary>
        Organization = 2,

        /// <summary>
        /// Represents a setting that can be configured/changed for each User.
        /// </summary>
        User = 4
    }
}
