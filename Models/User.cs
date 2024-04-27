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

        [Required]
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
            SharedValues.CurUser = IsUser(UserName, Password) ? CCur : null;
        }
        public string GetPhotoPath(string filePath)
        {
           List<string> Path = filePath.Split("\\").ToList();
           string PhotoPath = "/" + Path[Path.Count - 2] + "/" + Path[Path.Count - 1];
           return PhotoPath;
        }
    }
}
