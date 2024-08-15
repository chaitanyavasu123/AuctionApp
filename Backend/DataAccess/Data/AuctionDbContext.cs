using Microsoft.EntityFrameworkCore;

public class AuctionDbContext : DbContext
{
    public AuctionDbContext(DbContextOptions<AuctionDbContext> options)
           : base(options)
    {
    }
    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Bid> Bids { get; set; }
    public DbSet<Auction> Auctions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure the one-to-many relationship between User and Product (SoldProducts)
        modelBuilder.Entity<User>()
            .HasMany(u => u.SoldProducts)
            .WithOne(p => p.Seller)
            .HasForeignKey(p => p.SellerId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure the one-to-many relationship between User and Product (BoughtProducts)
        modelBuilder.Entity<User>()
            .HasMany(u => u.BoughtProducts)
            .WithOne(p => p.Buyer)
            .HasForeignKey(p => p.BuyerId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure the one-to-many relationship between User and Bids
        modelBuilder.Entity<User>()
            .HasMany(u => u.Bids)
            .WithOne(b => b.User)
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure the one-to-many relationship between Product and Bids
        modelBuilder.Entity<Product>()
            .HasMany(p => p.Bids)
            .WithOne(b => b.Product)
            .HasForeignKey(b => b.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure the one-to-many relationship between User and Auctions
        modelBuilder.Entity<User>()
            .HasMany(u => u.CreatedAuctions)
            .WithOne(a => a.User)
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure the one-to-many relationship between Auction and Products
        modelBuilder.Entity<Auction>()
            .HasMany(a => a.Products)
            .WithOne(p => p.Auction)
            .HasForeignKey(p => p.AuctionId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Product>()
            .Property(p => p.AuctionId)
            .IsRequired(false);

        // Configure the default value for IsSold
        modelBuilder.Entity<Product>()
            .Property(p => p.IsSold)
            .HasDefaultValue(false);

        modelBuilder.Entity<Product>()
        .Property(p => p.BuyerId)
        .IsRequired(false);

        modelBuilder.Entity<Product>()
            .Property(p => p.BuyerId)
            .HasDefaultValue(null);
        modelBuilder.Entity<Bid>()
        .Property(b => b.Amount)
        .HasColumnType("decimal(18, 2)");

        modelBuilder.Entity<Product>()
            .Property(p => p.StartingPrice)
            .HasColumnType("decimal(18, 2)");

       // modelBuilder.Entity<Product>()
        //    .Property(p => p.ReservedPrice)
         //   .HasColumnType("decimal(18, 2)");

        base.OnModelCreating(modelBuilder);

        // Seed Users
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 99,
                Email = "admin@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("AdminPassword"),
                Role = "Admin"
            },
            new User
            {
                Id = 1,
                Email = "user1@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("User1Password"),
                Role = "User"
            },
            new User
            {
                Id = 2,
                Email = "user2@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("User2Password"),
                Role = "User"
            }
        );
        // Seed Auctions
        modelBuilder.Entity<Auction>().HasData(
    new Auction
    {
        Id = 1,
        Title = "Electronics Bonanza",
        Description = "Auction for various electronics items",
        StartTime = new DateTime(2024, 8, 1, 9, 0, 0),
        EndTime = new DateTime(2024, 8, 15, 18, 0, 0),
        UserId = 2  // Assuming this is the seller with Id 2
    },
    new Auction
    {
        Id = 2,
        Title = "Bike Auction",
        Description = "Auction for high-quality mountain bikes",
        StartTime = new DateTime(2024, 8, 1, 10, 0, 0),
        EndTime = new DateTime(2024, 8, 10, 17, 0, 0),
        UserId = 1  // Assuming this is the seller with Id 1
    }
);


        // Seed Products
        modelBuilder.Entity<Product>().HasData(
            new Product
            {
                Id = 1,
                Name = "Laptop",
                Description = "A high-performance laptop",
                StartingPrice = 500,
               // ReservedPrice = 1000,
                Category = "Electronics",
                AuctionId= 1,
                SellerId = 2,
                IsSold = false
            },
            new Product
            {
                Id = 2,
                Name = "Smartphone",
                Description = "A latest model smartphone",
                StartingPrice = 300,
                //ReservedPrice = 700,
                Category = "Electronics",
                AuctionId = 1,
                SellerId = 2,
                IsSold = false
            },
            new Product
            {
                Id = 3,
                Name = "Bike",
                Description = "A mountain bike",
                StartingPrice = 150,
               // ReservedPrice = 500,
                Category = "Vehicles",
                AuctionId = 2,
                SellerId = 1,
                IsSold = false
            }
        );

        base.OnModelCreating(modelBuilder);
    }
}


