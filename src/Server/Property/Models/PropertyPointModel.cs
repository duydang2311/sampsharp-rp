using Microsoft.EntityFrameworkCore;

namespace Server.Property.Models;

[PrimaryKey(nameof(PropertyId), nameof(Ordinal))]
public sealed class PropertyPointModel
{
	public long PropertyId { get; set; }
	public byte Ordinal { get; set; }
	public float X { get; set; }
	public float Y { get; set; }
	public float Z { get; set; }
	public int World { get; set; }
	public int Interior { get; set; }
}
