using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SampSharp.Entities.SAMP;

namespace Server.Character.Models;

public sealed class CharacterModel
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
	[Required]
	public float X { get; set; }
	[Required]
	public float Y { get; set; }
	[Required]
	public float Z { get; set; }
	[Required]
	public int World { get; set; }
	[Required]
	public int Interior { get; set; }
}
