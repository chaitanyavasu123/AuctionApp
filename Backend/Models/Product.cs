using System.ComponentModel.DataAnnotations;

public class Product
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    public decimal StartingPrice { get; set; }

   // [Required]
   // public decimal ReservedPrice { get; set; }

    [Required]
    public string Category { get; set; }


    public int AuctionDuration { get; set; } // in hours

    // Foreign keys
    [Required]
    public int SellerId { get; set; }
    public User? Seller { get; set; }


    // Foreign key to Auction
    public int? AuctionId { get; set; }
    public Auction? Auction { get; set; }

    // New property to indicate if the product is sold
    public bool IsSold { get; set; } = false;

    //product may not sold yet
    public int? BuyerId { get; set; }
    public User? Buyer { get; set; }

    // Navigation properties
    public ICollection<Bid>? Bids { get; set; }
}
