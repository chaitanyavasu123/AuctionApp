using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Auction
{
    public int Id { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public DateTime StartTime { get; set; }
    [Required]
    public DateTime EndTime { get; set; }
    [Required]
    public int UserId { get; set; }
    public User? User { get; set; }

    // Navigation property for products
    public ICollection<Product>? Products { get; set; }

    // New property to hold product IDs from frontend
    [NotMapped]
    public List<int> ProductIds { get; set; }
}
