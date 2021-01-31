using System.ComponentModel.DataAnnotations;

namespace Commander.Models
{
    public class Account
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(250)]
        public string Name { get; set; }
        [Required]
        public string Document { get; set; }
       
        public enum EStatus
        {
            Active,
            Inactive
        }
         [Required]
        public EStatus staus { get; set; }

    }


}