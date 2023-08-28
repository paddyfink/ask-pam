//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace AskPam.Crm.Application.Features
//{
//    class FeatureDefinition
//    {
        

//        /// <summary>
//        /// Unique name of the feature.
//        /// </summary>
//        public string Name { get; private set; }

//        /// <summary>
//        /// Display name of the feature.
//        /// This can be used to show features on UI.
//        /// </summary>
//        public string DisplayName { get; set; }

//        /// <summary>
//        /// A brief description for this feature.
//        /// This can be used to show feature description on UI. 
//        /// </summary>
//        public string Description { get; set; }

        

//        /// <summary>
//        /// Default value of the feature.
//        /// This value is used if feature's value is not defined for current edition or tenant.
//        /// </summary>        

//        public string DefaultValue { get; set; }

//        /// <summary>
//        /// Feature's scope.
//        /// 
//        /// </summary>
//        public FeatureScopes Scope { get; set; }

//        /// <summary>
//        /// Creates a new feature.
//        /// </summary>
//        /// <param name="name">Unique name of the feature</param>
//        /// <param name="defaultValue">Default value</param>
//        /// <param name="displayName">Display name of the feature</param>
//        /// <param name="description">A brief description for this feature</param>
//        /// <param name="scope">Feature scope</param>
//        /// <param name="inputType">Input type</param>
//        public FeatureDefinition(string name, string defaultValue, string displayName = null, string description = null, FeatureScopes scope = FeatureScopes.All)
//        {
//            if (name == null)
//            {
//                throw new ArgumentNullException("name");
//            }

//            Name = name;
//            DisplayName = displayName;
//            Description = description;
//            Scope = scope;
//            DefaultValue = defaultValue;            
//        }
//    }
//}
