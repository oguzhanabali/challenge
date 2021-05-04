using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication3.Models;

namespace Challenge.Service.Services
{
    public class BookService : IBookService
    {
        private OwinAuthDbContext db = new OwinAuthDbContext();
        public List<Book> GetBooks()
        {
            return db.Book.ToList();
        }

        public Book GetBook(int id)
        {
            return db.Book.FirstOrDefault(a => a.BookId == id);
        }
        public int GetQuantity(int id)
        {
            return int.Parse(db.Book.Where(a => a.BookId == id).Select(a => a.Quantity).FirstOrDefault().ToString());
        }

        public List<BookByUser> GetUserProcess(string userId)
        {
            return db.BookByUser.Where(a => a.Users_Id == userId).ToList();
        }

        public void SaveReturned(int id, string userId)
        {
            BookByUser bookByUser = new BookByUser
            {
                BookId = id,
                ShipDate = DateTime.Now,
                Users_Id = userId
            };
            db.BookByUser.Add(bookByUser);
            db.SaveChanges();
        }
        public void SaveReserve(int id, string userId)
        {
            BookByUser bookByUser = new BookByUser
            {
                BookId = id,
                IssueDate = DateTime.Now,
                Users_Id = userId
            };
            db.BookByUser.Add(bookByUser);
            db.SaveChanges();
        }
        public void ReserveAndReturn(int bookId, bool isReserve)
        {
            using (OwinAuthDbContext db = new OwinAuthDbContext())
            {
                Book book = db.Book.Find(bookId);
                if (isReserve)
                    book.Quantity -= 1;
                else
                    book.Quantity += 1;
                db.Book.Attach(book);
                db.Entry(book).State = EntityState.Modified;
                db.SaveChanges();
            }
        }
        
        public bool IsLimitedUser(string userId)
        {
            using (OwinAuthDbContext db = new OwinAuthDbContext())
            {
                int shipQuantit = db.BookByUser.Count(a => a.Users_Id == userId && a.ShipDate == null);
                return shipQuantit > 3 ? true : false;
            }
        }
    }
}
