using Server.Property.Models;

namespace Microsoft.EntityFrameworkCore;

public static partial class ExtendModelBuilder
{
	public static void BuildProperty(this ModelBuilder self)
	{
		var property = self.Entity<PropertyModel>();
		property
			.HasOne(m => m.Owner)
			.WithMany()
			.OnDelete(DeleteBehavior.Cascade);
		property
			.HasMany(m => m.Points)
			.WithOne()
			.HasForeignKey(m => m.PropertyId);
		property
			.Navigation(m => m.Owner)
			.UsePropertyAccessMode(PropertyAccessMode.Property);
		property
			.Navigation(m => m.Points)
			.UsePropertyAccessMode(PropertyAccessMode.Property);
	}
}
