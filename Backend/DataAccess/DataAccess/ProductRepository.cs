using Microsoft.EntityFrameworkCore;

namespace DataAccess.DataAccess
{


    public class ProductRepository : IProductRepository
    {
        private readonly AuctionDbContext _context;

        public ProductRepository(AuctionDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetProductById(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<Product> AddProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Product> UpdateProduct(int id, Product product)
        {
            var existingProduct = await _context.Products.FindAsync(id);
            if (existingProduct == null)
                return null;

            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.StartingPrice = product.StartingPrice;
            //existingProduct.ReservedPrice = product.ReservedPrice;
            existingProduct.Category = product.Category;
            existingProduct.AuctionDuration = product.AuctionDuration;
            existingProduct.SellerId = product.SellerId;
            existingProduct.IsSold = product.IsSold;

            await _context.SaveChangesAsync();
            return existingProduct;
        }

        public async Task<bool> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}
