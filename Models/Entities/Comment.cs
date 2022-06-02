using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Entities
{
    public class Comment {

        [Key]
        public int Id {get; set;}

        private Product _product {get;} public Product Product {get{return _product;}}

        [Required(ErrorMessage = "Please insert content...")]
        [MaxLength(500,ErrorMessage = "Maximum length is 500!")]
        private string _content {get; set;} public string Content {get{return _content;} set{_content = value;}}

        private DateTime _timeStamp {get;} public DateTime TimeStamp {get{return _timeStamp;}}

        public string UserId { get; set; }
        public IdentityUser? User { get; set; }

        public Comment() {}
   }
}