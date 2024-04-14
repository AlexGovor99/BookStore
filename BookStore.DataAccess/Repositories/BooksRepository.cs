using BookStore.Core.Models;
using BookStore.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookStore.DataAccess.Repositories;

public class BooksRepository : IBooksRepository
{
    private readonly BookStoreDbContext _dbContext;
    
    public BooksRepository(BookStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Book>> Get()
    {
        var bookEntities = await _dbContext.Books
            .AsNoTracking()
            .ToListAsync();

        var books = bookEntities
            .Select(x => Book.Create(x.Id, x.Title, x.Description, x.Price).Book)
            .ToList();
        
        return books!;
    }

    public async Task<Guid> Create(Book book)
    {
        var bookEntity = new BookEntity()
        {
            Id = book.Id,
            Title = book.Title,
            Description = book.Description,
            Price = book.Price
        };

        await _dbContext.Books.AddAsync(bookEntity);
        await _dbContext.SaveChangesAsync();

        return bookEntity.Id;
    }

    public async Task<Guid> Update(Guid id, string title, string description, decimal price)
    {
        await _dbContext.Books
            .Where(x => x.Id == id)
            .ExecuteUpdateAsync(x => x
                .SetProperty(b => b.Title, b => title)
                .SetProperty(b => b.Description, b => description)
                .SetProperty(b => b.Price, b => price));

        return id;
    }

    public async Task<Guid> Delete(Guid id)
    {
        await _dbContext.Books
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync();

        return id;
    }
}