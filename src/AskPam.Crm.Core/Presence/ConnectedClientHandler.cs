using AskPam.Events;
using AskPam.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Z.EntityFramework.Plus;

namespace AskPam.Crm.Presence
{
    public class ConnectedClientHandler : IEventHandler<UserConnected>, IEventHandler<UserDisconnected>//, IEventHandler<UserReconnected>
    {
        private readonly IRepository<ConnectedClient, long> _connectedClientRepo;

        public ConnectedClientHandler(IRepository<ConnectedClient, long> connectedClientRepo)
        {
            _connectedClientRepo = connectedClientRepo;
        }

        //public async Task Handle(UserReconnected args)
        //{
        //    var client = args.Client;

        //    if (!_connectedClientRepo.GetAll().Any(c => c.ConnectionId == client.ConnectionId))
        //    {
        //        _connectedClientRepo.Insert(client);
        //        _connectedClientRepo.SaveChanges();
        //    }
        //}

        public async Task Handle(UserDisconnected args)
        {
           await _connectedClientRepo.GetAll()
                  .Where(b => b.ConnectionId == args.connectionID)
                  .DeleteAsync();
        }

        public async Task Handle(UserConnected args)
        {
            var client = args.Client;

            if (!await _connectedClientRepo.GetAll().AnyAsync(c => c.ConnectionId == client.ConnectionId))
            {
                await _connectedClientRepo.InsertAsync(client);
                //await _connectedClientRepo.SaveChangesAsync();
            }
        }
    }
}
