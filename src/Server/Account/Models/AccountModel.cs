using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SampSharp.Entities.SAMP;

namespace Server.Account.Models;

public sealed class AccountModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    [MaxLength(SampLimits.MaxPlayerNameLength)]
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    [MaxLength(61)]
    public string Password { get; set; } = string.Empty;

}
