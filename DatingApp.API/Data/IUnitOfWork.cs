using System.Threading.Tasks;

namespace DatingApp.API.Data
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IMessageRepository MessageRepository { get; }
        ILikesRepository LikesRepository { get; }
        Task<bool> CompleteAsync();
        bool HasChanges();
    }
}