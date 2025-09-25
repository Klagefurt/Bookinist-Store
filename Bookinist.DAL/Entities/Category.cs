using Bookinist.DAL.Entities.Base;

namespace Bookinist.DAL.Entities
{
    public class Category : NamedEntity 
    {
        public virtual ICollection<Book> Books { get; set; }
    }
}
