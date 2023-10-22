using System;
using System.Collections.Generic;
using DbLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace DbLibrary.DbConnector;

public partial class FinanceBotMatveyDbContext : DbContext
{
    public FinanceBotMatveyDbContext()
    {
    }

    public FinanceBotMatveyDbContext(DbContextOptions<FinanceBotMatveyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Bank> Banks { get; set; }

    public virtual DbSet<BankCurrency> BankCurrencies { get; set; }

    public virtual DbSet<Currency> Currencies { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=194.67.105.79:5432;Database=finance_bot_matvey_db;Username=finance_bot_matvey_user;Password=12345");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Bank>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("bank_pk");

            entity.ToTable("bank");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .HasColumnName("name");
        });

        modelBuilder.Entity<BankCurrency>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("bank_currency_pk");

            entity.ToTable("bank_currency");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.BankId).HasColumnName("bank_id");
            entity.Property(e => e.Buying).HasColumnName("buying");
            entity.Property(e => e.CurrencyId).HasColumnName("currency_id");
            entity.Property(e => e.Sale).HasColumnName("sale");

            entity.HasOne(d => d.Bank).WithMany(p => p.BankCurrencies)
                .HasForeignKey(d => d.BankId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("bank_currency_bank_id_fk");

            entity.HasOne(d => d.Currency).WithMany(p => p.BankCurrencies)
                .HasForeignKey(d => d.CurrencyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("bank_currency_currency _id_fk");
        });

        modelBuilder.Entity<Currency>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("currency _pk");

            entity.ToTable("currency ");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .HasColumnName("name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
