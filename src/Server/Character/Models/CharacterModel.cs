using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using SampSharp.Entities.SAMP;

namespace Server.Character.Models;

public sealed class CharacterModel
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public long Id { get; set; }
	[Required]
	public long AccountId { get; set; }
	[MaxLength(SampLimits.MaxPlayerNameLength)]
	[Required]
	public string Name { get; set; } = string.Empty;
	public int Age { get; set; }
	public bool Gender { get; set; }
	public float X { get; set; } = 1659.8188f;
	public float Y { get; set; } = -1680.4004f;
	public float Z { get; set; } = 21.4228f;
	public float A { get; set; } = 180.0f;
	public int World { get; set; }
	public int Interior { get; set; }
	public int Skin { get; set; }
	[DefaultValue("vi")]
	[MaxLength(11)]
	public string Locale { get; set; } = "vi";
	public float Health { get; set; } = 100;
}
