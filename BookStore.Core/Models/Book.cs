namespace BookStore.Core.Models;

public class Book
{
    public const int MAX_TITLE_LENGTH = 250;
    
    private Book(Guid id, string title, string description, decimal price)
    {
        Id = id;
        Title = title;
        Description = description;
        Price = price;
    }
    
    public Guid Id { get; }
    
    public string Title { get; }
    
    public string Description { get; }
    
    public decimal Price { get; }

    public static (Book? Book, string Error) Create(Guid id, string title, string description, decimal price)
    {
        var error = CheckParameters(title, description, price);
        return string.IsNullOrEmpty(error) ? (new Book(id, title, description, price), error) : (null, error);
    }

    private static string CheckParameters(string title, string description, decimal price)
    {
        return string.IsNullOrEmpty(title) || title.Length > MAX_TITLE_LENGTH ? "Error in title" :
            string.IsNullOrEmpty(description) ? "Error in description" :
            price < 0 ? "Error in price" : string.Empty;
    }
}