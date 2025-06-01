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
    IUserRepository _userRepository;
    IMapper _mapper;

    public UserEFController(IConfiguration config, IUserRepository userRepository)
    {
        _userRepository = userRepository;

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
        IEnumerable<User> users = _userRepository.GetUsers();
        return users;
        // return new string[] { "Test", "Test 2" };
    }

    [HttpGet("GetSingleUser/{userId}")]
    // public IActionResult Test()
    public User GetSingleUser(int userId)
    {
        return _userRepository.GetSingleUser(userId);
    }

    [HttpPut("EditUser")]
    public IActionResult EditUser(User user)
    {
        User? userDb = _userRepository.GetSingleUser(user.UserId);

        if (userDb != null)
        {
            userDb.Active = user.Active;
            userDb.FirstName = user.FirstName;
            userDb.LastName = user.LastName;
            userDb.Email = user.Email;
            userDb.Gender = user.Gender;

            if (_userRepository.SaveChanges())
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

        _userRepository.AddEntity<User>(userDb);
        if (_userRepository.SaveChanges())
        {
            return Ok();
        }

        throw new Exception("Failed to Add user");
    }

    [HttpDelete("DeleteUser/{userId}")]
    public IActionResult DeleteUser(int userId)
    {
        User? userDb = _userRepository.GetSingleUser(userId);

        if (userDb != null)
        {
            _userRepository.RemoveEntity<User>(userDb);
            if (_userRepository.SaveChanges())
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
        return _userRepository.GetUserSalaries();
    }

    [HttpGet("UserSalary/{userId}")]
    // public IActionResult Test()
    public UserSalary GetUserSalary(int userId)
    {
        return _userRepository.GetSingleUserSalary(userId);
    }

    [HttpPost("AddUserSalary")]
    public IActionResult AddUserSalary(UserSalary userSalary)
    {
        _userRepository.AddEntity<UserSalary>(userSalary);
        if (_userRepository.SaveChanges())
        {
            return Ok();
        }
        throw new Exception("Failed to Add user salary");
    }

    [HttpPut("UpdateUserSalary")]
    public IActionResult UpdateUserSalary(UserSalary userForUpdate)
    {
        UserSalary? userToUpdate = _userRepository.GetSingleUserSalary(userForUpdate.UserId);

        if (userToUpdate != null)
        {
            _mapper.Map(userToUpdate, userForUpdate);
            if (_userRepository.SaveChanges())
            {
                return Ok("User salary updated successfully");
            }
        }
        return NoContent();
    }

    [HttpDelete("DeleteUserSalary/{userId}")]
    public IActionResult DeleteUserSalary(int userId)
    {
        UserSalary? userSalaryToDelete = _userRepository.GetSingleUserSalary(userId);

        if (userSalaryToDelete != null)
        {
            _userRepository.RemoveEntity<UserSalary>(userSalaryToDelete);
            if (_userRepository.SaveChanges())
            {
                return Ok();
            }
            throw new Exception("Failed deleting user salary");
        }
        throw new Exception("Failed To Get user salary");
    }
}
