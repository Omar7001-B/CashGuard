using System.ComponentModel.DataAnnotations;

namespace ThreeFriends.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        // Type of transaction ("Expense" or "Income")
        [Required(ErrorMessage = "Transaction type is required")]
        [StringLength(10)] // Adjust the length as needed
        public string TransactionType { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title length can't be more than 100 characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Amount must be a positive number")]
        public decimal Amount { get; set; }

        [StringLength(500, ErrorMessage = "Info length can't be more than 500 characters")]
        public string Info { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.Now;

        // Foreign key for Category
        public int CategoryId { get; set; }

        // Navigation property to Category
        public virtual Category Category { get; set; }

    }
}
