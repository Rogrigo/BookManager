using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookManager.Models
{
    public class UserLibrary
    {
        [Key]
        public int Id { get; set; }
        public int UserID { get; set; }
        [ForeignKey("UserID")]
        public User User { get; set; }
        public int BookID { get; set; }
        [ForeignKey("BookID")]
        public Book Book { get; set; }
    }
}
