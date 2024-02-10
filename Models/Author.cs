using System.ComponentModel.DataAnnotations;

namespace BookManager.Models
{
    public class Author
    {
        [Key]
        public int AuthorID { get; set; }
        public string AuthorName { get; set; }
        public string AuthorSurname { get; set; }
        public string ShortInformation { get; set; }
    }
}
