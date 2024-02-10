using BookManager.Data.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookManager.Models
{
    public class Book
    {
        [Key]
        public int BookID { get; set; }
        public string Title { get; set; }
        public int AuthorID { get; set; }
        [ForeignKey("AuthorID")]
        public  Author Author { get; set; }
        public  Genre Genre { get; set; }
        public string BookShortInfo { get; set; }
        public string BookImageUrl { get; set; }
        public double Rating { get; set; }
        public string CloudinaryPublicId { get; set; }
    }
}
