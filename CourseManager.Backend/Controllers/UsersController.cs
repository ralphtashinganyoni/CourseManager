using CourseManager.API.Controllers;
using CourseManager.Application.Repository;
using CourseManager.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CourseManager.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : GenericController<User>
    {
        public UsersController(IBaseRepository<User> repository) : base(repository)
        {
        }
    }
}