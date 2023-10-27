using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using ASI.Basecode.Services.Services;
using ASI.Basecode.AdminApp.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ASI.Basecode.AdminApp.Controllers
{
    public class BookController : ControllerBase<BookController>
    {
        private readonly IBookService _bookService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        /// <param name="loggerFactory"></param>
        /// <param name="configuration"></param>
        /// <param name="localizer"></param>
        /// <param name="mapper"></param>
        public BookController(IBookService bookService,
                              IHttpContextAccessor httpContextAccessor,
                              ILoggerFactory loggerFactory,
                              IConfiguration configuration,
                              IMapper mapper = null) : base(httpContextAccessor, loggerFactory, configuration, mapper)
        {
            _bookService = bookService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var data = _bookService.GetBooks();
            return View("Index", data);
        }

        // purpose is to display add book screen
        [HttpGet]
        public IActionResult AddBook()
        {
            return View();
        }

        // the parameter of this post is to retrieved the bookview model
        [HttpPost]
        public IActionResult AddBook(BookViewModel book)
        {
            //Before added, invoke the validation declared in the BookService Validate()
            var isExist = _bookService.Validate(book.Title);
            
            if(isExist)
            {
                //create a condition to avoid duplication
                //AddModelError needs a "key" and "error message", key is the container of the message error
                //in this part is "Title", then error message viewed by the user
                ModelState.AddModelError("Title", "Title already exist.");
                //here, below why book is returned,
                //in order to avoid wiping out the data of other fields inputted along from the user
                return View(book);
            }
            _bookService.AddBook(book, this.UserId);
            return RedirectToAction("Index");
        }
    }
}
