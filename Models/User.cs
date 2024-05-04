using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using System.Diagnostics.CodeAnalysis;
using ThreeFriends.Controllers;

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
        [RegularExpression(@"^[a-zA-Z][a-zA-Z][a-zA-Z]*$", ErrorMessage = "First name must be alphabets only and 2 letters and more")]
        public string First_Name { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z][a-zA-Z]*$", ErrorMessage = "Last name must be alphabets only and 2 letters and more")]
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
        [Required(ErrorMessage ="Gender is required")]
        public string Gender { get; set; }
        [Required(ErrorMessage = "Please upload a photo")]
        public string photoPath { get; set; }





        public virtual ICollection<Category> Categories { get; set; } = new List<Category>(); 
        public virtual ICollection<HistoryItem> History { get; set; } = new List<HistoryItem>();
        public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();


        public bool IsUser(string UserName, string Password)
        {
            Appdbcontxt entity = new Appdbcontxt();
            User CCur = entity.Users.FirstOrDefault(u => u.User_Name == UserName);
            if (CCur == null)
            {
                return false; 
            }
            bool flag = Hashing.ValidatePassword(Password, CCur.Password);
            return flag;
        }
        public string GetPhotoPath(string filePath)
        {
           List<string> Path = filePath.Split("\\").ToList();
           string PhotoPath = "/" + Path[Path.Count - 2] + "/" + Path[Path.Count - 1];
           return PhotoPath;
        }
    }
}
