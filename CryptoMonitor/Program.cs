using CryptoMonitor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<CryptoService>(client =>
{
    client.DefaultRequestHeaders.Add("User-Agent", "CryptoMonitorApp");
});

var app = builder.Build();

app.MapGet("/", () => Results.Content(@"
<html>
    <head>
        <title>Crypto Tracker</title>
        <style>
            body { background-color: #121212; color: #00ff00; font-family: monospace; text-align: center; margin-top: 15%; }
            h1 { font-size: 3em; }
        </style>
    </head>
    <body>
        <h1>Crypto exchange rates</h1>
        <h2 id='btc'>Bitcoin: Loading...</h2>
        <h2 id='eth'>Ethereum: Loading...</h2>
        <h2 id='sol'>Solana: Loading...</h2>
        
        <script>
            async function fetchPrices() {
                try {
                    const response = await fetch('/api/prices');
                    const data = await response.json();
                    
                    if (data && data.bitcoin && data.ethereum) {
                        document.getElementById('btc').innerText = 'Bitcoin: $' + data.bitcoin.usd;
                        document.getElementById('eth').innerText = 'Ethereum: $' + data.ethereum.usd;
                        document.getElementById('sol').innerText = 'Solana: $' + data.solana.usd;
                    }
                } catch (error) {
                    document.getElementById('btc').innerText = 'Error while updating data!';
                    document.getElementById('eth').innerText = 'Error while updating data!';
                    document.getElementById('sol').innerText = 'Error while updating data!';
                }
            }
            fetchPrices();
            setInterval(fetchPrices, 30000);
        </script>
    </body>
</html>", "text/html"));

app.MapGet("/api/prices", async (CryptoService cryptoService) =>
{
    var data = await cryptoService.GetPricesAsync();
    return data is not null ? Results.Ok(data) : Results.Problem("Failed to retrieve data.");
});

app.Run();
