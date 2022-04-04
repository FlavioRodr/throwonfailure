namespace framework
{
    public interface IFakeService
    {
        Response GetsFaultyResponseFromApi();

        Response GetsFaltyResponse();
    }
}