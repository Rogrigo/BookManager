using BookManager.Data;
using BookManager.Data.Enum;
using BookManager.Interfaces;
using BookManager.Models;
using BookManager.Services;
using BookManager.ViewModels.BookViewModels;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
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
        public async Task<IActionResult> Index(string searchString, Genre[] genres, int[] authorIds, string sortOrder)
        {
            var books = await _bookRepository.GetAll();
            var authors = await _authorRepository.GetAllAuthors();

            ViewData["DateSortParm"] = sortOrder == "Rating" ? "rating_desc" : "Rating";

            switch (sortOrder)
            {
                case "rating_desc":
                    books = books.OrderByDescending(s => s.Rating);
                    break;
                case "Rating":
                    books = books.OrderBy(s => s.Rating);
                    break;
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                books = books.Where(s => s.Title.Contains(searchString));
            }

            if (genres != null && genres.Length > 0)
            {
                books = books.Where(s => genres.Contains(s.Genre));
            }

            if (authorIds != null && authorIds.Length > 0)
            {
                books = books.Where(s => authorIds.Contains(s.Author.AuthorID));
            }

            var viewModel = new BooksIndexViewModel
            {
                Books = books,
                Authors = authors
            };

            return View(viewModel);
        }

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
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

        [Authorize]
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
        [Authorize]
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

        [Authorize]
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
                Author = book.Author,
                URL = book.BookImageUrl,
                Rating = book.Rating,
            };
            return View(bookVM);
        }

        [HttpPost]
        [Authorize]
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
            book.Author.AuthorName = bookVM.Author.AuthorName;
            book.Author.AuthorSurname = bookVM.Author.AuthorSurname;
            book.Author.ShortInformation = bookVM.Author.ShortInformation;
            _bookRepository.Update(book);

            return RedirectToAction("Index");
        }
    }
}
