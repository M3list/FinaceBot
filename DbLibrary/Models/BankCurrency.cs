using System;
using System.Collections.Generic;

namespace DbLibrary.Models;

public partial class BankCurrency
{
    public int Id { get; set; }

    public int BankId { get; set; }

    public int CurrencyId { get; set; }

    public double Buying { get; set; }

    public double Sale { get; set; }

    public virtual Bank Bank { get; set; } = null!;

    public virtual Currency Currency { get; set; } = null!;
}
