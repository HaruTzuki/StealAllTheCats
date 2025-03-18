namespace DvlDev.SATC.Application.Models;

public abstract class BaseEntity
{
	public required int Id { get; init; }
	public required DateTime CreatedOn { get; init; }
}