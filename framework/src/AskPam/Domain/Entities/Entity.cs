using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskPam.Domain.Entities
{
    public class Entity : Entity<long>
    {
        public bool IsTransient()
        {
            return Id == default(Int64);
        }
    }

    //[Serializable]
    public abstract class Entity<TPrimaryKey> : IEntity<TPrimaryKey>
    {
        /// <summary>
        /// Unique identifier for this entity.
        /// </summary>
        public virtual TPrimaryKey Id { get; set; }

       
    }
}
