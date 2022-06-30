using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Models;
using System;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    
    [Route("users")]
    public class UserController : Controller
    {
        [HttpPost]
        [Route("")]
        [AllowAnonymous]
        public async Task<ActionResult<User>> Post(
            [FromServices] DataContext context,
            [FromBody] User model
            )
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            try
            {
                context.User.Add(model);
                await context.SaveChangesAsync();
                return model;
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Não foi possível criar o usuário" });
            }
        }


        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<dynamic>> Authenticate(
            [FromServices] DataContext context,
            [FromBody] User model
            )
        {
            var user = await context.User
                .AsNoTracking()
                .Where(x => x.Username == model.Username && x.Password == model.Password)   
                .FirstOrDefaultAsync();

            if (user == null)
                return NotFound(new { message = "Usuário ou senha inválida" });

            //se encontrou usuario e senha gera o token
            var token = TokenService.GenerateToken(user);
            return new
            {
                user = user,
                token = token
            };

        }

    }
}
