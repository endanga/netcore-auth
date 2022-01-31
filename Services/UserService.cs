
using dotnet_api_project.IdentityAuth;
using dotnet_api_project.Models;
using Microsoft.AspNetCore.Identity;

namespace dotnet_api_project.Services;

public interface IUserService
{
    IdentityUser GetById(int id);
    IEnumerable<IdentityUser> GetAll();
    void Register(IdentityUser identityUser, IdentityRole identityRole);
    void Update(IdentityUser identityUser, IdentityRole identityRole);
    void Delete(int id);
    
}

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;

    public UserService(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration configuration
    ){
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
    }


    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<IdentityUser> GetAll()
    {
        throw new NotImplementedException();
    }

    public IdentityUser GetById(int id)
    {
        throw new NotImplementedException();
    }

    public void Register(IdentityUser identityUser, IdentityRole identityRole)
    {
        throw new NotImplementedException();
    }

    public void Update(IdentityUser identityUser, IdentityRole identityRole)
    {
        throw new NotImplementedException();
    }
}