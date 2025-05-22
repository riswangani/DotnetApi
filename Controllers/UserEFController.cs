using AutoMapper;
using DotnetAPI.Data;
using DotnetAPI.Dtos;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserEFController : ControllerBase
{
    DataContextEF _entityFramework;
    IMapper _mapper;

    public UserEFController(IConfiguration config)
    {
        _entityFramework = new DataContextEF(config);

        _mapper = new Mapper(
            new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserToAddDto, User>();
                cfg.CreateMap<UserSalary, UserSalary>();
            })
        );
    }

    [HttpGet("GetUsers")]
    // public IActionResult Test()
    public IEnumerable<User> GetUsers()
    {
        IEnumerable<User> users = _entityFramework.Users.ToList<User>();
        return users;
        // return new string[] { "Test", "Test 2" };
    }

    [HttpGet("GetSingleUser/{userId}")]
    // public IActionResult Test()
    public User GetSingleUser(int userId)
    {
        User? user = _entityFramework.Users.Where(u => u.UserId == userId).FirstOrDefault<User>();

        if (user != null)
        {
            return user;
        }
        throw new Exception("Failed to get user");

        // return new string[] { "Test", "Test 2", userId };
    }

    [HttpPut("EditUser")]
    public IActionResult EditUser(User user)
    {
        User? userDb = _entityFramework
            .Users.Where(u => u.UserId == user.UserId)
            .FirstOrDefault<User>();

        if (userDb != null)
        {
            userDb.Active = user.Active;
            userDb.FirstName = user.FirstName;
            userDb.LastName = user.LastName;
            userDb.Email = user.Email;
            userDb.Gender = user.Gender;

            if (_entityFramework.SaveChanges() > 0)
            {
                return Ok();
            }
        }
        throw new Exception("Failed updating user");
    }

    [HttpPost("AddUser")]
    public IActionResult AddUser(UserToAddDto user)
    {
        User userDb = _mapper.Map<User>(user);

        _entityFramework.Add(userDb);
        if (_entityFramework.SaveChanges() > 0)
        {
            return Ok();
        }

        throw new Exception("Failed to Add user");
    }

    [HttpDelete("DeleteUser/{userId}")]
    public IActionResult DeleteUser(int userId)
    {
        User? userDb = _entityFramework.Users.Where(u => u.UserId == userId).FirstOrDefault<User>();

        if (userDb != null)
        {
            _entityFramework.Users.Remove(userDb);
            if (_entityFramework.SaveChanges() > 0)
            {
                return Ok();
            }
            throw new Exception("Failed deleting user");
        }
        throw new Exception("Failed To Get user");
    }

    // UserSalary Section
    [HttpGet("GetUsersSalaries")]
    // public IActionResult Test()
    public IEnumerable<UserSalary> GetUserSalaries()
    {
        IEnumerable<UserSalary> users = _entityFramework.UserSalary.ToList();
        return users;
    }

    [HttpGet("UserSalary/{userId}")]
    // public IActionResult Test()
    public ActionResult<UserSalary> GetUserSalary(int userId)
    {
        var userSalary = _entityFramework.UserSalary.FirstOrDefault(us => us.UserId == userId);

        if (userSalary != null)
            return Ok(userSalary);

        return NotFound($"UserSalary with userId {userId} not found");
    }

    [HttpPost("AddUserSalary")]
    public IActionResult AddUserSalary(UserSalary userSalary)
    {
        _entityFramework.UserSalary.Add(userSalary);
        if (_entityFramework.SaveChanges() > 0)
        {
            return Ok();
        }
        throw new Exception("Failed to Add user salary");
    }

    [HttpPut("UpdateUserSalary")]
    public IActionResult UpdateUserSalary(UserSalary userSalary)
    {
        if (!ModelState.IsValid)
            return BadRequest("Invalid Model");

        var userSalaryDb = _entityFramework.UserSalary.FirstOrDefault(u =>
            u.UserId == userSalary.UserId
        );

        if (userSalaryDb == null)
            return NotFound($"UserSalary with userId {userSalary.UserId} not found");

        // userSalaryDb.Salary = userSalary.Salary;
        _mapper.Map(userSalary, userSalaryDb);

        if (_entityFramework.SaveChanges() > 0)
            return Ok("User salary updated successfully");

        return NoContent();
    }

    [HttpDelete("DeleteUserSalary/{userId}")]
    public IActionResult DeleteUserSalary(int userId)
    {
        UserSalary? userSalaryDb = _entityFramework
            .UserSalary.Where(u => u.UserId == userId)
            .FirstOrDefault<UserSalary>();

        if (userSalaryDb != null)
        {
            _entityFramework.UserSalary.Remove(userSalaryDb);
            if (_entityFramework.SaveChanges() > 0)
            {
                return Ok();
            }
            throw new Exception("Failed deleting user salary");
        }
        throw new Exception("Failed To Get user salary");
    }
}
