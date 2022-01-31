using dotnet_api_project.IdentityAuth;
using Microsoft.AspNetCore.Identity;

namespace dotnet_api_project.Services;


public interface IRoleService
{
    IdentityRole GetById(int id);
    IEnumerable<IdentityRole> GetAll();
    void Register(IdentityRole identityRole);
    void Update(IdentityRole identityRole);
    void Delete(int id);
}

public class RoleService : IRoleService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;

    public RoleService
    (
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration configuration
    ) 
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<IdentityRole> GetAll()
    {
        throw new NotImplementedException();
    }

    public IdentityRole GetById(int id)
    {
        throw new NotImplementedException();
    }

    public void Register(IdentityRole identityRole)
    {
        throw new NotImplementedException();
    }

    public void Update(IdentityRole identityRole)
    {
        throw new NotImplementedException();
    }
}