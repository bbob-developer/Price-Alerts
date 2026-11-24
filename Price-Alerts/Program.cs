using System.Net;
using DotNetEnv;

//Setup for http requests
HttpClient client = new HttpClient();

//Allows the program to access .env
Env.TraversePath().Load();

//Load local Finnhub API key
//stings? allows varible to be null if necessary
string? FINNHUB_API_KEY = Environment.GetEnvironmentVariable("FINNHUB_API_KEY");

//Check if local Finnhub API key exists
if (String.IsNullOrWhiteSpace(FINNHUB_API_KEY))
{
    Console.WriteLine("No API key for Finnhub was found.");
    FinnhubKeyInstallHelp();
} else
{
    Console.WriteLine("Finnhub API key exists.");

    //Check if the Finnhub API key is valid
    try
    {
        //Check Apple stock to verify API key authenticity
        //TODO move to away from single request, should use normal stock check once implemented
        string response = await client.GetStringAsync($"https://finnhub.io/api/v1/quote?symbol=AAPL&token={FINNHUB_API_KEY}");
        Console.WriteLine("Finnhub authentication passed.");
    }
    catch (HttpRequestException e)
    {
        if(e.StatusCode == HttpStatusCode.Unauthorized || e.StatusCode == HttpStatusCode.Forbidden)
        {
            Console.WriteLine("Finnhub authentication failed. Try resetting your API key.");
            FinnhubKeyInstallHelp();
        }
        else
        {
            Console.WriteLine("You received an unusual error while checking your Finnhub API key, if the error continues please report it at https://github.com/bbob-developer/Price-Alerts/issues");
            Console.WriteLine($"Request error: {e.Message}");
        }
    }
}



void FinnhubKeyInstallHelp()
{
    //TODO write this function to walk user through getting finnhub api access.
}