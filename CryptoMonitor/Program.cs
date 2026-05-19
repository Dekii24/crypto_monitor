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
            body { background-color: #121212; color: #888888; font-family: monospace; text-align: center; margin-top: 8%; }
            h1 { font-size: 4em; color: #00ff00; margin-bottom: 1.5em}
            .coin-row { font-size: 1.8em; margin: 0.6em 0; }
            .btc-name  { color: #f7931a; font-weight: bold; }
            .eth-name  { color: #a855f7; font-weight: bold; }
            .sol-name  { color: #00c2ff; font-weight: bold; }
            .price     { color: #888888; }
        </style>
    </head>
    <body>
        <h1>Crypto exchange rates</h1>

        <div class='coin-row'>
            <span class='btc-name'>Bitcoin: </span><span class='price' id='btc'>Loading...</span>
        </div>
        <div class='coin-row'>
            <span class='eth-name'>Ethereum: </span><span class='price' id='eth'>Loading...</span>
        </div>
        <div class='coin-row'>
            <span class='sol-name'>Solana: </span><span class='price' id='sol'>Loading...</span>
        </div>
        
        <script>
            const prevPrices = { btc: null, eth: null, sol: null };

            function getColor(current, previous) {
                if (previous === null) return '#888888';
                if (current > previous) return '#00ff00';
                if (current < previous) return '#ff4444';
                return '#888888';
            }

            async function fetchPrices() {
                try {
                    const response = await fetch('/api/prices');
                    const data = await response.json();
                    
                    if (data && data.bitcoin && data.ethereum && data.solana) {
                        const btc = data.bitcoin.usd;
                        const eth = data.ethereum.usd;
                        const sol = data.solana.usd;

                        const btcEl = document.getElementById('btc');
                        const ethEl = document.getElementById('eth');
                        const solEl = document.getElementById('sol');

                        btcEl.style.color = getColor(btc, prevPrices.btc);
                        ethEl.style.color = getColor(eth, prevPrices.eth);
                        solEl.style.color = getColor(sol, prevPrices.sol);

                        btcEl.innerText = '$'  + btc;
                        ethEl.innerText = '$'  + eth;
                        solEl.innerText = '$'  + sol;

                        prevPrices.btc = btc;
                        prevPrices.eth = eth;
                        prevPrices.sol = sol;
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
