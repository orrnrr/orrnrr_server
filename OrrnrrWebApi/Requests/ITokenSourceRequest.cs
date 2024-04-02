namespace OrrnrrWebApi.Requests
{
    internal interface ITokenSourceRequest
    {
        string RequestUrl { get; }
        string Name { get; }
    }
}
