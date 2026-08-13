using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NuxibaApi.Models;

[Table("ccloglogin")]
public class LoginRecord
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required]
    public int User_id { get; set; }
    [ForeignKey("User_id")]
    public User? User { get; set; }
    
    public int Extension { get; set; }
    

    public int TipoMov { get; set; }
    
    public DateTime fecha { get; set; }
}
