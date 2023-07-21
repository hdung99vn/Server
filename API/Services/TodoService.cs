using API.AppContext;
using API.IServices;
using API.Models;
using Core.Repositoty;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class TodoService : GenericRepository<Todo>, ITodoService
    {
        public TodoService(ApplicationContext context) : base(context)
        {
        }
    }
}
