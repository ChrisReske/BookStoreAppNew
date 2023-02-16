using BookStoreApp.Blazor.Server.UI.Services.Base;

namespace BookStoreApp.Blazor.Server.UI.Services.Author;

public interface IBookService
{
    // Todo: Make Serivce(s) and methods generic (at a later stage)
    Task<Response<List<BookReadOnlyDto>>> Get();
    Task<Response<BookDetailsDto>> Get(int id);
    Task<Response<BookUpdateDto>> GetForUpdate(int id);
    Task<Response<int>> Create(BookCreateDto author);
    Task<Response<int>> Edit(int id, BookUpdateDto author);
    Task<Response<int>> Delete(int id);
}