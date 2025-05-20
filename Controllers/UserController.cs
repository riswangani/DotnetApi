using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    DataContextDapper _dapper;

    public UserController(IConfiguration config)
    {
        _dapper = new DataContextDapper(config);
    }

    [HttpGet("TestConnection")]
    public DateTime TestConnection()
    {
        string sql = "SELECT GETDATE()";
        DateTime response = _dapper.LoadDataSinggle<DateTime>(sql);
        return response;
    }

    [HttpGet("GetUsers/{testValue}")]
    // public IActionResult Test()
    public string[] Test(string testValue)
    {
        string[] responseArray = new string[] { "Hello", "World", testValue };
        return responseArray;
    }
}
