using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using System.Diagnostics.CodeAnalysis;

namespace ThreeFriends.Models
{
    [Table("User")]
    public class User
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(TypeName = "int")]
        public int Id { get; set; }
        [Required] 
        public string User_Name { get; set; }
        [Required]
        public string First_Name { get; set; }
        [Required]
        public string Last_Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Phone_Number { get; set; }
        [Required]
        public string Password { get; set; }
        [AllowNull]
        public DateTime? Sign_Up_Date { get; set; }
        public string? Bank_Account_ID {  get; set; }
        // image path (image in www root)
    }
}
