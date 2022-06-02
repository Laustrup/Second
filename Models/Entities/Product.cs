using System.ComponentModel.DataAnnotations;
using Entities.Statuses;
using Microsoft.AspNetCore.Identity;

namespace Entities
{
    public class Product {

        [Key]
        public int Id {get; set;}
        
        public string UserId { get; set; }
        public IdentityUser? User { get; set; }

        [Required(ErrorMessage = "Please insert title...")]
        [MinLength(3,ErrorMessage = "Minimum length is 3!")]
        [MaxLength(100,ErrorMessage = "Maximum length is 100!")]
        private string _title {get; set;} public string Title {get{return _title;} set{_title = value;}}

        [MaxLength(500,ErrorMessage = "Maximum length is 500!")]
        private string _description {get; set;} public string Description {get{return _description;} set{_description = value;}}

        [Required(ErrorMessage = "Please insert price...")]
        private int _price {get; set;} public int Price {get{return _price;} set{_price = value;}}

        private List<Comment>? _comments {get;} public List<Comment>? Comments {get{return _comments;}}

        private ProductStatus _status {get;set;} public ProductStatus Status {get{return _status;} set{_status = value;}}

        public List<Comment> Add_Comment(Comment comment)
        {
            if (_comments == null) {
                return null;
            }
            else
            {
                _comments.Add(comment);
                return _comments;
            }
        }
    }
} 
