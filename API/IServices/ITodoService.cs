using API.Models;
using Core.IRepository;

namespace API.IServices
{
    public interface ITodoService:IGenericRepository<Todo>
    {
    }
}
