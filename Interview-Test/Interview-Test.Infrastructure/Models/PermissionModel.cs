using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Interview_Test.Models;

[Table("PermissionTb")]
public class PermissionModel
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long PermissionId { get; set; }
    [Required]
    [Column(TypeName = "varchar(200)")]
    public string Permission { get; set; }
    [ForeignKey("RoleId")]
    public RoleModel Role { get; set; }
}