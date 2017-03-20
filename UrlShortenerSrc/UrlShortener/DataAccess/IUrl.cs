namespace UrlShortener.DataAccess
{
    public interface IUrl
    {
        string Address { get; }
        int Id { get; }
        int UseCount { get; }
        int RedirectType { get; }
    }
}