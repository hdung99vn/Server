using API.AppContext;
using API.IServices;
using API.Models;
using AutoMapper;
using Core.Repositoty;
using Microsoft.Extensions.Caching.Distributed;

namespace API.Services
{
    public class TodoService : GenericRepository<Todo>, ITodoService
    {
        public TodoService(ApplicationContext context, IDistributedCache cache, IMapper mapper) : base(context, cache, mapper)
        {
        }
    }
}
