using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLib.Models;

public partial class OrrnrrContext : DbContext
{
    public OrrnrrContext()
    {
    }

    public OrrnrrContext(DbContextOptions<OrrnrrContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Candlestick> Candlesticks { get; set; }

    public virtual DbSet<CandlestickUnit> CandlestickUnits { get; set; }

    public virtual DbSet<DividendHistory> DividendHistories { get; set; }

    public virtual DbSet<DividendReceiveHistory> DividendReceiveHistories { get; set; }

    public virtual DbSet<Token> Tokens { get; set; }

    public virtual DbSet<TokenHoldingsHistory> TokenHoldingsHistories { get; set; }

    public virtual DbSet<TokenOrderHistory> TokenOrderHistories { get; set; }

    public virtual DbSet<TokenSource> TokenSources { get; set; }

    public virtual DbSet<TransactionHistory> TransactionHistories { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql(Secrets.SecretProvider.ORRNRR_CONNECTION_STRING);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Candlestick>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("candlestick_pkey");

            entity.ToTable("candlestick");

            entity.HasIndex(e => new { e.TokenId, e.BeginDate, e.CandlestickUnitId }, "unq_date_token_candlestickunit").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BeginDate).HasColumnName("begin_date");
            entity.Property(e => e.BeginPrice).HasColumnName("begin_price");
            entity.Property(e => e.CandlestickUnitId).HasColumnName("candlestick_unit_id");
            entity.Property(e => e.EndPrice).HasColumnName("end_price");
            entity.Property(e => e.MaxPrice).HasColumnName("max_price");
            entity.Property(e => e.MinPrice).HasColumnName("min_price");
            entity.Property(e => e.TokenId).HasColumnName("token_id");
            entity.Property(e => e.TransactionVolume).HasColumnName("transaction_volume");

            entity.HasOne(d => d.CandlestickUnit).WithMany(p => p.Candlesticks)
                .HasForeignKey(d => d.CandlestickUnitId)
                .HasConstraintName("fk_candlestick_unit");

            entity.HasOne(d => d.Token).WithMany(p => p.Candlesticks)
                .HasForeignKey(d => d.TokenId)
                .HasConstraintName("fk_token");
        });

        modelBuilder.Entity<CandlestickUnit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("candlestick_unit_pkey");

            entity.ToTable("candlestick_unit");

            entity.HasIndex(e => e.Name, "unq_name").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<DividendHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("dividend_history_pkey");

            entity.ToTable("dividend_history");

            entity.HasIndex(e => new { e.DividendDate, e.TokenSourceId }, "unq_tokensource_date").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DividendDate).HasColumnName("dividend_date");
            entity.Property(e => e.DividendPerUnit).HasColumnName("dividend_per_unit");
            entity.Property(e => e.TokenSourceId).HasColumnName("token_source_id");

            entity.HasOne(d => d.TokenSource).WithMany(p => p.DividendHistories)
                .HasForeignKey(d => d.TokenSourceId)
                .HasConstraintName("fk_token_source");
        });

        modelBuilder.Entity<DividendReceiveHistory>(entity =>
        {
            entity.HasKey(e => e.DividendAmount).HasName("dividend_receive_history_pkey");

            entity.ToTable("dividend_receive_history");

            entity.HasIndex(e => new { e.UserId, e.TokenId, e.ReceiveDate }, "unq_dividend_receive_history").IsUnique();

            entity.Property(e => e.DividendAmount)
                .ValueGeneratedNever()
                .HasColumnName("dividend_amount");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.ReceiveDate).HasColumnName("receive_date");
            entity.Property(e => e.TokenId).HasColumnName("token_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Token).WithMany(p => p.DividendReceiveHistories)
                .HasForeignKey(d => d.TokenId)
                .HasConstraintName("fk_token");

            entity.HasOne(d => d.User).WithMany(p => p.DividendReceiveHistories)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("fk_user");
        });

        modelBuilder.Entity<Token>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("token_pkey");

            entity.ToTable("token");

            entity.HasIndex(e => new { e.Name, e.TokenSourceId }, "unique_name_tokensource").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(2000)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .HasColumnName("name");
            entity.Property(e => e.TokenSourceId).HasColumnName("token_source_id");

            entity.HasOne(d => d.TokenSource).WithMany(p => p.Tokens)
                .HasForeignKey(d => d.TokenSourceId)
                .HasConstraintName("fk_token_source");
        });

        modelBuilder.Entity<TokenHoldingsHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("token_holdings_history_pkey");

            entity.ToTable("token_holdings_history");

            entity.HasIndex(e => new { e.UserId, e.TokenId, e.HoldDate }, "unq_token_holdings_history").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AverageBuyPrice).HasColumnName("average_buy_price");
            entity.Property(e => e.HoldDate).HasColumnName("hold_date");
            entity.Property(e => e.MaxBuyPrice).HasColumnName("max_buy_price");
            entity.Property(e => e.MinBuyPrice).HasColumnName("min_buy_price");
            entity.Property(e => e.TokenCount).HasColumnName("token_count");
            entity.Property(e => e.TokenId).HasColumnName("token_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Token).WithMany(p => p.TokenHoldingsHistories)
                .HasForeignKey(d => d.TokenId)
                .HasConstraintName("fk_token");

            entity.HasOne(d => d.User).WithMany(p => p.TokenHoldingsHistories)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("fk_user");
        });

        modelBuilder.Entity<TokenOrderHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("token_order_history_pkey");

            entity.ToTable("token_order_history");

            entity.HasIndex(e => new { e.UserId, e.TokenId, e.OrderDate }, "unq_token_order_history").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CompleteCount).HasColumnName("complete_count");
            entity.Property(e => e.IsBuyOrder).HasColumnName("is_buy_order");
            entity.Property(e => e.IsCanceled).HasColumnName("is_canceled");
            entity.Property(e => e.OrderCount).HasColumnName("order_count");
            entity.Property(e => e.OrderDate).HasColumnName("order_date");
            entity.Property(e => e.OrderPrice).HasColumnName("order_price");
            entity.Property(e => e.TokenId).HasColumnName("token_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Token).WithMany(p => p.TokenOrderHistories)
                .HasForeignKey(d => d.TokenId)
                .HasConstraintName("fk_token");

            entity.HasOne(d => d.User).WithMany(p => p.TokenOrderHistories)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("fk_user");
        });

        modelBuilder.Entity<TokenSource>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("token_source_pkey");

            entity.ToTable("token_source");

            entity.HasIndex(e => e.Name, "unique_name").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .HasColumnName("name");
            entity.Property(e => e.RequestUrl)
                .HasMaxLength(2000)
                .HasColumnName("request_url");
        });

        modelBuilder.Entity<TransactionHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("transaction_history_pkey");

            entity.ToTable("transaction_history");

            entity.HasIndex(e => new { e.BuyOrderId, e.SellOrderId }, "unq_transaction_history").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BuyOrderId).HasColumnName("buy_order_id");
            entity.Property(e => e.IsBuyTransaction).HasColumnName("is_buy_transaction");
            entity.Property(e => e.SellOrderId).HasColumnName("sell_order_id");
            entity.Property(e => e.TransactionCount).HasColumnName("transaction_count");
            entity.Property(e => e.TransactionDate).HasColumnName("transaction_date");

            entity.HasOne(d => d.BuyOrder).WithMany(p => p.TransactionHistoryBuyOrders)
                .HasForeignKey(d => d.BuyOrderId)
                .HasConstraintName("fk_buy_order");

            entity.HasOne(d => d.SellOrder).WithMany(p => p.TransactionHistorySellOrders)
                .HasForeignKey(d => d.SellOrderId)
                .HasConstraintName("fk_sell_order");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_pkey");

            entity.ToTable("user");

            entity.HasIndex(e => e.Name, "unq_username").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Balance).HasColumnName("balance");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .HasColumnName("name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
