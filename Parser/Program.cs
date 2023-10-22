using System.Net;
using System.Text;
using DbLibrary.DbConnector;
using DbLibrary.Models;
using HtmlAgilityPack;

WebClient webClient = new WebClient();

webClient.Encoding = Encoding.UTF8;
webClient.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705)");

var startTimeSpan = TimeSpan.Zero;
var periodTimeSpan = TimeSpan.FromMinutes(5);

var timer = new System.Threading.Timer((e) => { UpdateData(webClient); }, null, startTimeSpan, periodTimeSpan);

Console.ReadLine();

void UpdateData(WebClient _webClient)
{
    string htmlData = _webClient.DownloadString("https://mainfin.ru/currency");

    HtmlDocument document = new HtmlDocument();
    document.LoadHtml(htmlData);

    HtmlNode tableBanks = document.DocumentNode.SelectSingleNode("//tbody");

    HtmlNodeCollection banksData = tableBanks.SelectNodes("tr");

    FinanceBotMatveyDbContext db = new FinanceBotMatveyDbContext();

    List<Currency> currencies = db.Currencies.ToList();

    for (int i = 0; i < banksData.Count; i++)
    {
        string name = banksData[i].SelectSingleNode("td[1]/a").InnerText;
        Bank bank = db.Banks.Where(x => x.Name == name).FirstOrDefault();

        int bankId;

        if (bank == null)
        {
            db.Banks.Add(new Bank() { Name = name });
            db.SaveChanges();
        }

        bankId = db.Banks.Where(x => x.Name == name).FirstOrDefault().Id;

        for (int j = 0; j < currencies.Count; j++)
        {
            double buying = 0;
            double sale = 0;

            switch (currencies[j].Name)
            {
                case "USD":
                    buying = double.Parse(banksData[i].SelectSingleNode("td[2]/span[1]").InnerText.Replace(".", ","));
                    sale = double.Parse(banksData[i].SelectSingleNode("td[3]/span[1]").InnerText.Replace(".", ","));
                    break;

                case "EUR":
                    buying = double.Parse(banksData[i].SelectSingleNode("td[4]/span[1]").InnerText.Replace(".", ","));
                    sale = double.Parse(banksData[i].SelectSingleNode("td[5]/span[1]").InnerText.Replace(".", ","));
                    break;
            }

            BankCurrency bankCurrency = db.BankCurrencies.Where(x => x.CurrencyId == currencies[j].Id && x.BankId == bankId).FirstOrDefault();
            
            if (bankCurrency == null)
            {
                db.BankCurrencies.Add(new BankCurrency()
                    { CurrencyId = currencies[j].Id, BankId = bankId, Buying = buying, Sale = sale });
                db.SaveChanges();
            }
            else
            {
                bankCurrency.Buying = buying;
                bankCurrency.Sale = sale;
                db.BankCurrencies.Update(bankCurrency);
                db.SaveChanges();
            }
        }
    }
}