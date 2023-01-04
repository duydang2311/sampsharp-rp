using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Server.Character.Models;

namespace Server.Property.Models;

public sealed class PropertyModel
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public long Id { get; set; }
	public CharacterModel? Owner { get; set; }
	[MaxLength(64)]
	public string Description { get; set; } = string.Empty;
	public List<PropertyPointModel>? Points { get; set; }
}
