using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Entities
{
    public class Comment {

        [Key]
        public int CommentId {get; set;}

        private Product? _product { get; set; } public Product? Product
        {
            get{return _product;}
            set { _product = value; }
        }

        public int ProductId { get; set; }

        [Required(ErrorMessage = "Please insert content...")]
        [MaxLength(500,ErrorMessage = "Maximum length is 500!")]
        private string _content {get; set;} public string Content {get{return _content;} set{_content = value;}}

        private DateTime? _timeStamp { get; set; } public DateTime? TimeStamp
        {
            get{return _timeStamp;}
            set { _timeStamp = DateTime.Now; }
        }

        public string? UserId { get; set; }
        public string? UserEmail { get; set; }
        public IdentityUser? User { get; set; }
        
   }
}