namespace Ofx.Battleships.Api.SeedWork;

public abstract record CommandQueryResponse
{
    public bool IsValid => Errors.Count == 0;
    public List<string> Errors { get; set; } = new();
}