using BookManager.Data;
using BookManager.Interfaces;
using BookManager.Models;
using BookManager.Services;
using BookManager.ViewModels;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookManager.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookRepository _bookRepository;
        private readonly IPhotoService _photoService;
        private readonly IAuthorRepository _authorRepository;

        public BookController(IBookRepository bookRepository, IPhotoService photoService, IAuthorRepository authorRepository)
        {
            _bookRepository = bookRepository;
            _photoService = photoService;
            _authorRepository = authorRepository;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Book> books = await _bookRepository.GetAll();
            return View(books);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBookViewModel bookVM)
        {
            var author = await _authorRepository.FindAuthor(bookVM.AuthorName, bookVM.AuthorSurname);
            if (author == null && string.IsNullOrEmpty(bookVM.AuthorShortInfo))
            {
                ModelState.AddModelError("AuthorShortInfo", "Author not found please complete adding all necessary information");
                TempData["AuthorNotFound"] = true;
                return View(bookVM);
            }

            if (author == null)
            {
                TempData["AuthorNotFound"] = true;
                author = new Author
                {
                    AuthorName = bookVM.AuthorName,
                    AuthorSurname = bookVM.AuthorSurname,
                    ShortInformation = bookVM.AuthorShortInfo,
                };
                await _authorRepository.AddAsync(author);
            }

            if (ModelState.IsValid)
            { 
                var uploadPhoto = await _photoService.AddPhotoAsync(bookVM.Image);
                var newBook = new Book
                {
                    Title = bookVM.Title,
                    AuthorID = author.AuthorID,
                    BookShortInfo = bookVM.BookShortInfo,
                    Genre = bookVM.Genre,
                    Rating = bookVM.Rating,
                    BookImageUrl = uploadPhoto.Url.ToString(),
                    CloudinaryPublicId = uploadPhoto.PublicId,
                };
                _bookRepository.Add(newBook);
                return RedirectToAction("Index");
                
            }
            else
            {
                ModelState.AddModelError("", "Photo upload failed");
            }
            return View(bookVM);
        }
        public async Task<IActionResult> Detail(int id)
        {
            Book book = await _bookRepository.GetByIdAsync(id);
            if (book == null)
            {
                return RedirectToAction("Index");
            }
            return View(book);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var bookDetails = await _bookRepository.GetByIdAsync(id);
            if (bookDetails == null)
            {
                return View("Error");
            }
            return View(bookDetails);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var bookDetails = await _bookRepository.GetByIdAsync(id);
            await _photoService.DeletePhotoAsync(bookDetails.CloudinaryPublicId);
            if (bookDetails == null)
            {
                return View("Error");
            }

            _bookRepository.Delete(bookDetails);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null)
            {
                return View("Error");
            }
            var bookVM = new EditBookViewModel()
            {
                Title = book.Title,
                AuthorID = book.AuthorID,
                ShortBookInfo = book.BookShortInfo,
                Genre = book.Genre,
                URL = book.BookImageUrl,
                Rating = book.Rating,
            };
            return View(bookVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditBookViewModel bookVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit book");
                return View("Edit", bookVM);
            }

            var book = await _bookRepository.GetByIdAsyncNoTracking(id);

            if (book == null)
            {
                ModelState.AddModelError("", "Book not found");
                return View(bookVM);
            }

            if (bookVM.Image != null)
            {
                try
                {
                    await _photoService.DeletePhotoAsync(book.CloudinaryPublicId);
                    var photoResult = await _photoService.AddPhotoAsync(bookVM.Image);
                    book.BookImageUrl = photoResult.Url.ToString();
                    book.CloudinaryPublicId = photoResult.PublicId;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Could not delete or upload photo");
                    return View(bookVM);
                }
            }

            book.Title = bookVM.Title;
            book.AuthorID = bookVM.AuthorID;
            book.BookShortInfo = bookVM.ShortBookInfo;
            book.Genre = bookVM.Genre;
            book.Rating = bookVM.Rating;

            _bookRepository.Update(book);

            return RedirectToAction("Index");
        }


    }
}
