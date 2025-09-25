using DBContext;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Controller
{
    public class FactoryApp
    {
        private IUserService userService;
        private AppDbContext dbContext;
        private DbContextOptions<AppDbContext> options;
        public FactoryApp(DbContextOptions<AppDbContext> options)
        {
            this.options = options;
        }
        public FactoryApp(IUserService service)
        {
            userService = service;
        }
        public FactoryApp(AppDbContext context)
        {
            dbContext = context;
        }

        public object CreateInstance(string item)
        {
            switch(item.ToLower())
                {
                case "usercontroller":
                    return new UserController(this.userService);
                case "userservice":
                    return new UserService(this.dbContext);
                case "dbconection":
                    return new AppDbContext(this.options);
                default:
                    throw new ArgumentException("Invalid item type");
            }

        }
    }
}
