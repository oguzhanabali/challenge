using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication3.Models;

namespace Challenge.Service.Services
{
    public interface IBookService
    {
        List<Book> GetBooks();
        Book GetBook(int id);
        int GetQuantity(int id);
        List<BookByUser> GetUserProcess(string userId);
        void ReserveAndReturn(int bookId, bool isReserve);
        bool IsLimitedUser(string getUserId);
        void SaveReturned(int id, string userId);
        void SaveReserve(int id, string userId);
    }
}
