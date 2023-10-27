using ASI.Basecode.Services.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.Interfaces
{
    public interface IBookService
    {
        public List<BookViewModel> GetBooks();
        public void AddBook(BookViewModel book, string user);
        public bool Validate(string title);
    }
}
