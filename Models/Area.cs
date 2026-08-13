using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NuxibaApi.Models;

[Table("ccRIACat_Areas")]
public class Area
{
    [Key]
    public int IDArea { get; set; }
    
    [MaxLength(200)]
    public string? AreaName { get; set; }
    
    public int? StatusArea { get; set; }
    public DateTime? CreateDate { get; set; }
    public bool? Default { get; set; }
    

    public ICollection<User> Users { get; set; } = new List<User>();
}
