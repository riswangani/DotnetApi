using DotnetAPI.Data;
using DotnetAPI.Dtos;
using DotnetAPI.Models;
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
        return _dapper.LoadData<DateTime>("SELECT GETDATE()").FirstOrDefault();
    }

    [HttpGet("GetUsers")]
    // public IActionResult Test()
    public IEnumerable<User> GetUsers()
    {
        string sql =
            @"
            SELECT [UserId],
                   [FirstName],
                   [LastName],
                   [Email],
                   [Gender],
                   [Active]
            FROM TutorialAppSchema.Users";

        return _dapper.LoadData<User>(sql);

        // return new string[] { "Test", "Test 2" };
    }

    [HttpGet("GetSingleUser/{userId}")]
    // public IActionResult Test()
    public User GetSingleUser(int userId)
    {
        string sql =
            @"
            SELECT [UserId],
                [FirstName],
                [LastName],
                [Email],
                [Gender],
                [Active]
            FROM TutorialAppSchema.Users
                WHERE UserId = " + userId.ToString();
        User user = _dapper.LoadDataSinggle<User>(sql, new { UserId = userId });
        return user;
    }

    [HttpPut("EditUser")]
    public IActionResult EditUser(User user)
    {
        string sql =
            @"
            UPDATE TutorialAppSchema.Users
            SET [FirstName] = @FirstName,
                [LastName] = @LastName,
                [Email] = @Email,
                [Gender] = @Gender,
                [Active] = @Active
            WHERE UserId = @UserId";

        // Console.WriteLine(sql);
        bool result = _dapper.ExecuteSql(sql, user);
        return result ? Ok() : throw new Exception("Failed updating user");
    }

    [HttpPost("AddUser")]
    public IActionResult AddUser(UserToAddDto user)
    {
        string sql =
            @"
            INSERT INTO TutorialAppSchema.Users(
                [FirstName],
                [LastName],
                [Email],
                [Gender],
                [Active]
            ) VALUES (
                @FirstName,
                @LastName,
                @Email,
                @Gender,
                @Active
            )";

        // Console.WriteLine(sql);
        if (_dapper.ExecuteSql(sql, user))
        {
            return Ok();
        }
        throw new Exception("Failed Adding user");
    }

    [HttpDelete("DeleteUser/{userId}")]
    public IActionResult DeleteUser(int userId)
    {
        string sql =
            @"
            DELETE FROM TutorialAppSchema.Users
            WHERE UserId = @UserId";

        // Console.WriteLine(sql);
        if (_dapper.ExecuteSql(sql, new { UserId = userId }))
        {
            return Ok();
        }
        throw new Exception("Failed deleting user");
    }

    // Get User Salary
    [HttpGet("GetUserSalary/{userId}")]
    // public IActionResult Test()
    public IEnumerable<UserSalary> GetUserSalary(int userId)
    {
        string sql =
            @"
        SELECT UserId, Salary 
        FROM TutorialAppSchema.UserSalary 
        WHERE UserId = @UserId";

        return _dapper.LoadData<UserSalary>(sql, new { UserId = userId });
    }

    [HttpPost("UserSalary")]
    public IActionResult PostUserSalary(UserSalary userSalaryForInsert)
    {
        string sql =
            @"INSERT INTO TutorialAppSchema.UserSalary(
                UserId,
                Salary
            ) VALUES (
                @UserId,
                @Salary
            )";

        Console.WriteLine(sql);
        if (_dapper.ExecuteSql(sql, userSalaryForInsert))
        {
            return Ok(userSalaryForInsert);
        }
        throw new Exception("Failed Adding user Salary");
    }

    [HttpPut("UserSalary")]
    public IActionResult UpdateUserSalary(UserSalary userSalaryForUpdate)
    {
        string sql =
            @"
            UPDATE TutorialAppSchema.UserSalary
            SET Salary = @Salary
            WHERE UserId = @UserId";

        Console.WriteLine(sql);
        if (_dapper.ExecuteSql(sql, userSalaryForUpdate))
        {
            return Ok(userSalaryForUpdate);
        }

        throw new Exception("Failed updating user salary");
    }

    [HttpDelete("UserSalary/{userId}")]
    public IActionResult DeleteUserSalary(int userId)
    {
        string sql =
            @"
            DELETE FROM TutorialAppSchema.UserSalary
            WHERE UserId = @UserId";

        // Console.WriteLine(sql);
        if (_dapper.ExecuteSql(sql, new { UserId = userId }))
        {
            return Ok();
        }
        throw new Exception("Failed deleting user salary");
    }
}
