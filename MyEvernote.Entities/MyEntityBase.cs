using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.Entities
{
    public class MyEntityBase
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)] /*Primary Key olarak atadık.*/
        public int Id { get; set; }

        [Required] //Normalde Datetime lar boş geçilemez zaten ancak okunabilirlik açısından required yaptım.
        public DateTime CreatedOn { get; set; }
        [Required]
        public DateTime ModifiedOn { get; set; }
        [Required,StringLength(30)]
        public string ModifiedUsername { get; set; }
    }
}
