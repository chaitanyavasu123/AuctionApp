using System.ComponentModel.DataAnnotations;

public class Bid
{
    public int Id { get; set; }

    [Required]
    public decimal Amount { get; set; }

    [Required]
    public int ProductId { get; set; }
    public Product? Product { get; set; }

    [Required]
    public int UserId { get; set; }
    public User? User { get; set; }
}
