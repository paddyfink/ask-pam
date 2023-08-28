//using AskPam.Crm.Authorization;
//using AskPam.Crm.Presence;
//using AskPam.Domain.Repositories;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;
//using Z.EntityFramework.Plus;

//namespace AskPam.Crm.Hubs
//{
//    public class PresenceService
//    {
//        private const int zombiePeriodThreshold = 2;
//        private readonly ITransportHeartbeat _heartbeat;
//        private readonly TimeSpan _checkInterval = TimeSpan.FromSeconds(60);
//        private readonly int _zombieThreshold;
//        private Timer timer;
//        private IRepository<ConnectedClient, long> _connectedClientRepo;
//        private IRepository<User, string> _usersRepo;

//        public PresenceService(ITransportHeartbeat heartbeat,
//            IRepository<ConnectedClient, long> connectedClientRepo
//            , IRepository<User, string> usersRepo)
//        {
//            _heartbeat = heartbeat;
//            _zombieThreshold = (int)_checkInterval.TotalSeconds * zombiePeriodThreshold;
//            _connectedClientRepo = connectedClientRepo;
//            _usersRepo = usersRepo;
//        }

//        public void StartMonitoring()
//        {
//            if (timer == null)
//            {
//                timer = new Timer(t =>
//                {
//                    try
//                    {
//                        Check().Wait();
//                    }
//                    catch (Exception)
//                    {
//                        //Trace.TraceError(ex.Message);
//                    }
//                }, null, TimeSpan.Zero, _checkInterval);
//            }
//        }

//        //EF6 adds much better support for contains in queries http://bit.ly/1wmQ2VQ
//        private async Task Check()
//        {
//            //Update active connections
//            var activeConnections = _heartbeat.GetConnections()
//                .Where(b => b.IsAlive)
//                .Select(b => b.ConnectionId);
            
//            await _connectedClientRepo.GetAll()
//                .Where(c => activeConnections.Contains(c.ConnectionId))
//                .UpdateAsync(c => new ConnectedClient { LastActivity = DateTime.UtcNow });
            

//            //Remove zombie connections
//            var date = DateTime.UtcNow.AddSeconds(-_zombieThreshold);
//            await _connectedClientRepo.GetAll()
//                .Where(c => c.LastActivity < date)
//                .DeleteAsync();

//        }


//    }
//}
