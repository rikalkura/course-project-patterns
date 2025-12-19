namespace OnlineStore.Application.Features.Products.DTOs;

public class UpdateProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? PhotoPath { get; set; }
    public decimal Price { get; set; }
    public int CategoryId { get; set; }
    public int StockQuantity { get; set; }
}




