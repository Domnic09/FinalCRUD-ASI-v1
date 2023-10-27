using ASI.Basecode.Data;
using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.Services
{
    
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        public BookService(IBookRepository bookRepository) 
        { 
            _bookRepository = bookRepository;
        }
        
        //this can be used in bookapp
        public List<BookViewModel> GetBooks()
        {
            var url = "https://127.0.0.1:8080";
            var data = _bookRepository.GetBooks().Select(s => new BookViewModel
            {
                Title = s.Title,
                Author = s.Author,
                Description = s.Description,
                ImageUrl = Path.Combine(url, s.BookId) + ".png",

            }).ToList();
            return data;
        }
        public void AddBook(BookViewModel book, string user) // string user declared
                                                             // to retrieved the user in the controller
        {
            var coverImagesPath = PathManager.DirectoryPath.CoverImagesDirectory;

            //Create a logic to create a model that will communicate the backend or in repository
            var model = new Book(); // Why book is defined due to it is required to be placed in Db
            //Next to mapped the values base in the viewmodel
            model.BookId = Guid.NewGuid().ToString(); // to generate random strings
            model.Title = book.Title;
            model.Author = book.Author;
            model.Description = book.Description;

            //Refered in controller
            model.CreatedBy = user;
            model.UpdatedBy = user;
            model.CreatedTime = DateTime.Now;
            model.UpdatedTime = DateTime.Now;

            var coverImageFileName = Path.Combine(coverImagesPath, model.BookId) + ".png";
            using var fileStream = new FileStream(coverImageFileName, FileMode.Create);
            {
                book.ImageFile.CopyTo(fileStream);
            }

            //after setup model data, it is need to connect and pass into the repository
            _bookRepository.AddBook(model);


            //Since there is still few conflicts due to the BookId is not set into int and not automatically incremented, in order to use the BookId for other references,
            //and also to make BookId generated in the Services
            //To implement the GUID to produce random strings
            // to create Guid declare the BookID pointed to GUID

        }
        public bool Validate(string title)
        {
            //to confirm if it exist
            //The LinQ is used below, retrieving data of All books in GetBooks(),
            //then it is filtered in Where if x is same "title" parameter
            var isExist = _bookRepository.GetBooks().Where(x =>  x.Title == title).Any();
            return isExist;
        }
    }
}
