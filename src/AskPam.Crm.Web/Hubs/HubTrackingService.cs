//using Microsoft.Extensions.Caching.Memory;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace AskPam.Crm.Hubs
//{
//    public class HubTrackingService : IHubTrackingService
//    {
//        private IMemoryCache _cache;
//        public HubTrackingService(IMemoryCache memoryCache)
//        {
//            _cache = memoryCache;
        
//        }
//        public void TrackConnection(HubTrack hubTrack)
//        {
//            _cache.Set(hubTrack.UserId + hubTrack.HubName, hubTrack);
//        }

//        public void RemoveConnection(string userId, string hub)
//        {
            
//            _cache.Remove(userId + hub);

//        }

//        public HubTrack GetUserConnection(string userId, string hub)
//        {
//            HubTrack hubTrack;
//            _cache.TryGetValue(userId + hub, out hubTrack);
//            return hubTrack;
//        }
//    }
//}
