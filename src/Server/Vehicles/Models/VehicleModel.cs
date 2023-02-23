using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Server.Character.Models;

namespace Server.Vehicles.Models;

public sealed class VehicleModel
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public long Id { get; set; }
	public int Model { get; set; }
	public float X { get; set; }
	public float Y { get; set; }
	public float Z { get; set; }
	public float A { get; set; }
	public int World { get; set; }
	public int Interior { get; set; }
	public float Health { get; set; } = 1000;
	public int PrimaryColor { get; set; }
	public int SecondaryColor { get; set; }

	public CharacterModel? CharacterModel { get; set; }
}
