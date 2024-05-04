using System.ComponentModel.DataAnnotations.Schema;

namespace ThreeFriends.Models
{
	public class HistoryItem
	{
		public int Id { get; set; } // Unique identifier for the history item
		public string OperationType { get; set; } // Type of operation: Expense Addition, Category Addition, Category Deletion, Category Editing
		public string Details { get; set; } // Details specific to the operation
		public DateTime Timestamp { get; set; } // Date and time when the operation was performed
		[ForeignKey("User")]
		public int UserId { get; set; } // Foreign key to the user who performed the operation
		public virtual User User { get; set; } // Navigation property to the user who performed the operation
    }
}
