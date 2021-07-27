using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.API.Dtos;
using DatingApp.API.Helpers;
using DatingApp.API.Models;

namespace DatingApp.API.Data
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUserById(int id);        
        Task<User> GetUserByUsername(string username);        
        Task<string> GetUserGender(string username);        
        Task<PagedList<MemberDto>> GetMembers(UserParams userParams);
        Task<MemberDto> GetMember(string username);
        void Update(User user);
    }
}