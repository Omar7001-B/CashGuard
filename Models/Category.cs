using System.ComponentModel.DataAnnotations;

namespace ThreeFriends.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, ErrorMessage = "Name length can't be more than 50 characters")]
        public string Name { get; set; }

        [StringLength(100, ErrorMessage = "Description length can't be more than 100 characters")]
        public string Description { get; set; }

        [StringLength(50, ErrorMessage = "Icon length can't be more than 50 characters")]
        public string Icon { get; set; }

        // Foreign key to User
        public int UserId { get; set; }
        // Navigation property to User
        public virtual User? User { get; set; }
        // Navigation property to Transaction
        public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
