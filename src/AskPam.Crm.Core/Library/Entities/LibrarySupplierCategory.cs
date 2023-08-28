using AskPam.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AskPam.Crm.Library
{
    public class LibrarySupplierCategory : Entity
    {
        public string Name { get; set; }
        public virtual ICollection<LibraryItem> LibraryItems { get; set; }
    }
}
