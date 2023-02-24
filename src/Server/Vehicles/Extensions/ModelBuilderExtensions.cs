using SampSharp.Entities.SAMP;
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
			.HasOne(m => m.Character)
			.WithMany()
			.HasForeignKey("CharacterId")
			.OnDelete(DeleteBehavior.Cascade);
		vehicle
			.Navigation(m => m.Character)
			.UsePropertyAccessMode(PropertyAccessMode.Property);
		vehicle
			.Property(m => m.Model)
			.HasConversion(v => v.ToString(), v => Enum.Parse<VehicleModelType>(v));
	}
}
