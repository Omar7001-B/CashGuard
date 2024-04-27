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
        public int Id { get; set; }
        [DataType("varchar(100)")]
        [Required] 
        public string User_Name { get; set; }
        [Required]
        [DataType("varchar(100)")]
        public string First_Name { get; set; }
        [Required]
        [DataType("varchar(100)")]
        public string Last_Name { get; set; }
        [Required]
        [DataType("varchar(100)")]
        public string Email { get; set; }
        [Required]
        [DataType("varchar(100)")]
        public string Phone_Number { get; set; }
        [Required]
        [DataType("varchar(100)")]
        public string Password { get; set; }
        [Required]
        public DateTime Sign_Up_Date { get; set; }
        [Required]
        [DataType("varchar(100)")]
        public string Bank_Account_ID {  get; set; }
        [Required]
        public byte[] Photo { get; set; }

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
    }
}
