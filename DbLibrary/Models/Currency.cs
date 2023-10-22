using System;
using System.Collections.Generic;

namespace DbLibrary.Models;

public partial class Currency
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<BankCurrency> BankCurrencies { get; set; } = new List<BankCurrency>();
}
