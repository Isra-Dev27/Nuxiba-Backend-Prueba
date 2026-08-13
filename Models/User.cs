using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NuxibaApi.Models;

[Table("ccUsers")]
public class User
{
    [Key]
    public int User_id { get; set; }
    
    [MaxLength(100)]
    public string? Login { get; set; }
    
    [MaxLength(100)]
    public string? Nombres { get; set; }
    
    [MaxLength(100)]
    public string? ApellidoPaterno { get; set; }
    
    [MaxLength(100)]
    public string? ApellidoMaterno { get; set; }
    
    [MaxLength(200)]
    public string? Password { get; set; }
    
    public int? TipoUser_id { get; set; }
    public int? Status { get; set; }
    public DateTime? fCreate { get; set; }
    
    public int? IDArea { get; set; }
    [ForeignKey("IDArea")]
    public Area? Area { get; set; }
    
    public DateTime? LastLoginAttempt { get; set; }
    

    public ICollection<LoginRecord> LoginRecords { get; set; } = new List<LoginRecord>();
}
