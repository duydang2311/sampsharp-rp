using SampSharp.Entities.SAMP;

namespace Server.Vehicles.Entities;

public sealed class ServerVehicle : Vehicle
{
	private int interior;
	private int primaryColor;
	private int secondaryColor;

	public int Interior
	{
		get => interior; set
		{
			interior = value;
			GetComponent<NativeVehicle>().LinkVehicleToInterior(interior);
		}
	}

	public int PrimaryColor
	{
		get => primaryColor; set
		{
			primaryColor = value;
			GetComponent<NativeVehicle>().ChangeVehicleColor(PrimaryColor, SecondaryColor);
		}
	}

	public int SecondaryColor
	{
		get => secondaryColor; set
		{
			secondaryColor = value;
			GetComponent<NativeVehicle>().ChangeVehicleColor(PrimaryColor, SecondaryColor);
		}
	}

	public ServerVehicle() : base() { }
}
