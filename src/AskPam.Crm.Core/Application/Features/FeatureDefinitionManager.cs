//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace AskPam.Crm.Application.Features
//{
//   public class FeatureDefinitionManager
//    {
//        private readonly IDictionary<string, FeatureDefinition> _features;

//        public FeatureDefinitionManager()
//        {
//            _features = new Dictionary<string, FeatureDefinition>();

//            foreach (var feature in GetFeatureDefinitions())
//            {
//                _features[feature.Name] = feature;
//            }
//        }

//        private IEnumerable<FeatureDefinition> GetFeatureDefinitions()
//        {
//            return new[]
//            {
//                //Host settings
//                new FeatureDefinition("nname","true", "Displayname"),
//            };
//        }
//    }
//}
