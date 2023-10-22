using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbLibrary.DbConnector;
using DbLibrary.Models;

namespace TgBot.Bot
{
    public class BotLogic
    {
        private static FinanceBotMatveyDbContext _dbContext;

        public BotLogic()
        {
            _dbContext = new FinanceBotMatveyDbContext();
        }

        public static string GetMax(string currency)
        {
            List<BankCurrency> bankCurrencies;
            BankCurrency bankCurrency;
            int currencyId = 0;
            double maxValue = 0;
            Bank bank = new Bank();

            switch (currency)
            {
                case "EUR":
                    currencyId = _dbContext.Currencies.Where(x => x.Name == "EUR").FirstOrDefault().Id;
                    bankCurrencies = _dbContext.BankCurrencies.Where(x => x.CurrencyId == currencyId).ToList();
                    maxValue = bankCurrencies.Max(x => x.Buying);
                    bank.Id = bankCurrencies.Where(x => x.Buying == maxValue).FirstOrDefault().BankId;
                    bank.Name = _dbContext.Banks.Where(x => x.Id == bank.Id).FirstOrDefault().Name;

                    break;
                case "USD":
                    currencyId = _dbContext.Currencies.Where(x => x.Name == "USD").FirstOrDefault().Id;
                    bankCurrencies = _dbContext.BankCurrencies.Where(x => x.CurrencyId == currencyId).ToList();
                    maxValue = bankCurrencies.Max(x => x.Buying);

                    bank.Id = bankCurrencies.Where(x => x.Buying == maxValue).FirstOrDefault().BankId;
                    bank.Name = _dbContext.Banks.Where(x => x.Id == bank.Id).FirstOrDefault().Name;
                    break;
            }

            return $"Значение: {maxValue} \nБанк: {bank.Name}";
        }

        public static string GetMin(string currency)
        {
            List<BankCurrency> bankCurrencies;
            BankCurrency bankCurrency;
            int currencyId = 0;
            double maxValue = 0;
            Bank bank = new Bank();

            switch (currency)
            {
                case "EUR":
                    currencyId = _dbContext.Currencies.Where(x => x.Name == "EUR").FirstOrDefault().Id;
                    bankCurrencies = _dbContext.BankCurrencies.Where(x => x.CurrencyId == currencyId).ToList();
                    maxValue = bankCurrencies.Min(x => x.Sale);
                    bank.Id = bankCurrencies.Where(x => x.Sale == maxValue).FirstOrDefault().BankId;
                    bank.Name = _dbContext.Banks.Where(x => x.Id == bank.Id).FirstOrDefault().Name;
                    break;
                case "USD":
                    currencyId = _dbContext.Currencies.Where(x => x.Name == "USD").FirstOrDefault().Id;
                    bankCurrencies = _dbContext.BankCurrencies.Where(x => x.CurrencyId == currencyId).ToList();
                    maxValue = bankCurrencies.Min(x => x.Sale);

                    bank.Id = bankCurrencies.Where(x => x.Sale == maxValue).FirstOrDefault().BankId;
                    bank.Name = _dbContext.Banks.Where(x => x.Id == bank.Id).FirstOrDefault().Name;

                    break;
            }

            return $"Значение: {maxValue} \nБанк: {bank.Name}";
        }
    }
}
