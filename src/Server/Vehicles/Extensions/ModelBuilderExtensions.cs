using Server.Vehicles.Models;

namespace Microsoft.EntityFrameworkCore;

public static partial class ModelBuilderExtensions
{
	public static void BuildVehicle(this ModelBuilder self)
	{
		var vehicle = self.Entity<VehicleModel>();
		vehicle
			.Property<long?>("CharacterId")
			.IsRequired(false);
		vehicle
			.HasOne(m => m.CharacterModel)
			.WithMany()
			.HasForeignKey("CharacterId")
			.OnDelete(DeleteBehavior.Cascade);
		vehicle
			.Navigation(m => m.CharacterModel)
			.UsePropertyAccessMode(PropertyAccessMode.Property);
	}
}
