using CourseManager.Application.Repository;
using CourseManager.Domain.Entities;
using CourseManager.Infrastructure.Context;

namespace CourseManager.Infrastructure.Repository
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
