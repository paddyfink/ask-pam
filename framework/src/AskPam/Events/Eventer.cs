using Autofac;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AskPam.Events
{
    public class DomainEvents : IDomainEvents
    {
        //Autofac context
        private readonly IComponentContext _container;
        private readonly ILogger<DomainEvents> _logger;

        public DomainEvents(IComponentContext container, ILogger<DomainEvents> logger)
        {
            _container = container;
            _logger = logger;
        }

        public async Task RaiseAsync<T>(T args) where T : IEvent
        {
            var handlers = _container.Resolve<IEnumerable<IEventHandler<T>>>();

            await RaiseAction(handlers, args);
            //var task = Task.Run(async () => RaiseAction(handlers, args));
            //return task;
        }


        //public void Raise<T>(T args) where T : IEvent
        //{
        //    var handlers = _container.Resolve<IEnumerable<IEventHandler<T>>>();

        //    RaiseAction(handlers, args);
        //}

        private  async Task RaiseAction<T>(IEnumerable<IEventHandler<T>> handlers, T args) where T : IEvent
        {
            foreach (var handler in handlers)
                try
                {
                    await handler.Handle(args);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                }
        }
    }
}

