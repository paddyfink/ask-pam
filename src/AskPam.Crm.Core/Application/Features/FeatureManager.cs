//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading.Tasks;

//namespace AskPam.Crm.Application.Features
//{
//   public class FeatureManager
//    {
     

//        public async Task<string> GetValueAsync(Guid organizationId, string name)
//        {
//            var feature = _featureManager.Get(name);

//            var value = await FeatureValueStore.GetValueOrNullAsync(tenantId, feature);
//            if (value == null)
//            {
//                return feature.DefaultValue;
//            }

//            return value;
//        }

//        public async Task<bool> IsEnabledAsync(Guid organizationId, string featureName)
//        {
//            var value =await GetValueAsync(organizationId, featureName);
//            return bool.Parse(value);
//        }

//        public Feature Get(string name)
//        {
//            var feature = GetOrNull(name);
//            if (feature == null)
//            {
//                throw new Exception("There is no feature with name: " + name);
//            }

//            return feature;
//        }

//        public IReadOnlyList<Feature> GetAll()
//        {
//            return Features.Values.ToImmutableList();
//        }
//    }
//}
