using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Challenge.Service.Services;
using Microsoft.AspNet.Identity;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    public class BooksController : ApiController
    {
        private readonly IBookService _bookService = new BookService();
        [AllowAnonymous]
        [HttpGet]
        //TODO:Tüm kitap listesi
        public List<Book> GetAll()
        {
            return _bookService.GetBooks();
        }
        [AllowAnonymous]
        [HttpGet]
        public Book Get(int id)
        {
            return _bookService.GetBook(id);
        }
        //TODO:Hangi kitaptan kaç adet var?
        [Authorize]
        [HttpGet]
        public HttpResponseMessage Quantity(int id)
        {
            int quantity = _bookService.GetQuantity(id);
            if (quantity > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, quantity);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Kitap numarası: " + id.ToString() + " olan bir kayıt bulunamadı.");
            }

        }
        //TODO:Belli bir kullanıcı saklı ve geri gönderdiği kitapların listesini alabilir.
        [Authorize]
        [HttpGet]
        public HttpResponseMessage GetUserProcess()
        {
            var userId = User.Identity.GetUserId();
            var userProcess = _bookService.GetUserProcess(userId);
            if (userProcess != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, userProcess);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Kayıtlı bir işleminiz bulunamadı.");
            }
        }
        [Authorize]
        [HttpPost]
        //TODO:Bir kitap bir kullanıcı tarafından reserve edildiğinde
        public HttpResponseMessage Reserve(int id)
        {
            try
            {
                if (!_bookService.IsLimitedUser(User.Identity.GetUserId()))
                {
                    _bookService.ReserveAndReturn(id, true);
                    _bookService.SaveReserve(id, User.Identity.GetUserId());

                    var message = Request.CreateResponse(HttpStatusCode.Created, "Kitap teslim edilmiştir. Kitabın iade tarihi: " + DateTime.Now.AddDays(7) + "'dir.");
                    return message;
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, "Limitiniz dolu: En fazla 3 adet kitap alabilirsiniz. ");
                }

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }
        [Authorize]
        [HttpPost]
        //TODO:Bir kitap bir kullanıcı tarafından iade edildiğinde
        public HttpResponseMessage Returned(int id)
        {
            try
            {
                _bookService.ReserveAndReturn(id, false);
                _bookService.SaveReturned(id, User.Identity.GetUserId());

                var message = Request.CreateResponse(HttpStatusCode.Created, "İadeniz yapıldı. Teşekkürler.");
                return message;
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }

    }
}