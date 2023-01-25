using Server.Door.Models;

namespace Microsoft.EntityFrameworkCore;

public static partial class ModelBuilderExtensions
{
	public static void BuildDoor(this ModelBuilder self)
	{
		var door = self.Entity<DoorModel>();
		_ = door
			.HasOne(m => m.ObjectModel)
			.WithOne()
			.HasForeignKey<DoorObjectModel>(m => m.Id)
			.IsRequired(false)
			.OnDelete(DeleteBehavior.Cascade);
		_ = door
			.Navigation(m => m.ObjectModel)
			.UsePropertyAccessMode(PropertyAccessMode.Property);
	}
}
