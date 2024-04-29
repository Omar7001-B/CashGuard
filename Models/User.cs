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
        [Column(TypeName = "INTEGER")]
        public int Id { get; set; }
  
        [Required(ErrorMessage = "User Name is required")]
        public string User_Name { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        public string First_Name { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        public string Last_Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone Number is required")]
        [RegularExpression(@"^\d{8,}$", ErrorMessage = "Phone Number must be at least 8 digits long and contain only digits")]
        public string Phone_Number { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$", ErrorMessage = "Password must have at least one uppercase letter, one lowercase letter, one digit, and be at least 8 characters long")]
        public string Password { get; set; }

        public DateTime Sign_Up_Date { get; set; }

        [MaxLength(50, ErrorMessage = "Bank Account ID cannot exceed 50 characters")]
        public string? Bank_Account_ID { get; set; }
        public string photoPath { get; set; }
        [NotMapped]
        private Appdbcontxt entity;
        [NotMapped]
        User CCur; 
        public bool IsUser(string UserName, string Password)
        {
            entity = new Appdbcontxt();
            CCur = entity.Users.FirstOrDefault(u => u.User_Name == UserName && u.Password == Password);
            return CCur != null;
        }
        public void SetCurUser(string UserName, string Password)
        {
            SharedValues.CurUser = IsUser(UserName, Password) ? CCur : new User();
        }
        public string GetPhotoPath(string filePath)
        {
           List<string> Path = filePath.Split("\\").ToList();
           string PhotoPath = "/" + Path[Path.Count - 2] + "/" + Path[Path.Count - 1];
           return PhotoPath;
        }
    }
}
