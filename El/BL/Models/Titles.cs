namespace El.BL.Models
{
    public record class Title(int Id, string Name);

    public record class PlayQuote(int Id, string Title, string Quote);
}
