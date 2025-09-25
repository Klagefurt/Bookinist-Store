using Bookinist.DAL.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookinist.DAL.Entities
{
    public class Book : NamedEntity
    {
        public virtual Category Category { get; set; }
    }
}
