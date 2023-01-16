using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Door.Models;

public sealed class DoorModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    [DefaultValue(0.0f)]
    public float EntranceX { get; set; }
    [DefaultValue(0.0f)]
    public float EntranceY { get; set; }
    [DefaultValue(0.0f)]
    public float EntranceZ { get; set; }
    [DefaultValue(0.0f)]
    public float EntranceA { get; set; }
    [DefaultValue(0)]
    public int EntranceInterior { get; set; }
    [DefaultValue(0)]
    public int EntranceWorld { get; set; }
    [DefaultValue(0.0f)]
    public float ExitX { get; set; }
    [DefaultValue(0.0f)]
    public float ExitY { get; set; }
    [DefaultValue(0.0f)]
    public float ExitZ { get; set; }
    [DefaultValue(0.0f)]
    public float ExitA { get; set; }
    [DefaultValue(0)]
    public int ExitInterior { get; set; }
    [DefaultValue(0)]
    public int ExitWorld { get; set; }
}