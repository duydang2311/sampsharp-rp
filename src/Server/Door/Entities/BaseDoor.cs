namespace Server.Door.Entities;

public abstract class BaseDoor : IDoor
{
	public long Id { get; set; }
	public BaseDoor(long id)
	{
		Id = id;
	}
}
