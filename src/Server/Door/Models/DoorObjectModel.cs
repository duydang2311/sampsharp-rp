using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Server.Door.Models;

public sealed class DoorObjectModel
{
	[Key]
	public long Id { get; set; }
	[DefaultValue(0)]
	public int Model { get; set; }
	[DefaultValue(0.0f)]
	public float X { get; set; }
	[DefaultValue(0.0f)]
	public float Y { get; set; }
	[DefaultValue(0.0f)]
	public float Z { get; set; }
	[DefaultValue(0.0f)]
	public float A { get; set; }
	[DefaultValue(0)]
	public int Interior { get; set; }
	[DefaultValue(0)]
	public int World { get; set; }
}
