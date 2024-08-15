using System.ComponentModel.DataAnnotations;

public class User
{
    public int Id { get; set; }

    [Required]
    public string Email { get; set; }

    [Required]
    public string PasswordHash { get; set; }

    [Required]
    public string Role { get; set; } // "Admin", "User"

    // Navigation properties
    public ICollection<Product> SoldProducts { get; set; }
    public ICollection<Product> BoughtProducts { get; set; }
    public ICollection<Bid> Bids { get; set; }
    public ICollection<Auction> CreatedAuctions { get; set; }
}
