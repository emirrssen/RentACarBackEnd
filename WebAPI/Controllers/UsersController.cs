﻿using Business.Abstract;
using Entities.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("update")]
        public IActionResult UpdateProfile(UserForUpdateDto userForUpdate)
        {
            var result = _userService.UpdateProfile(userForUpdate);

            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("updatepassword")]
        public IActionResult UpdatePassword(UserForUpdateDto userForUpdateDto, string oldPassword, string newPassword)
        {
            var result = _userService.UpdatePassword(oldPassword, newPassword, userForUpdateDto);

            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);  
        }

        [HttpGet("getall")]
        public IActionResult GetAllUsers()
        {
            var result = _userService.GetAllUsers();

            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }
    }
}
