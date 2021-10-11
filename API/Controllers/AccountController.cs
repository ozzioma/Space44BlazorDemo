using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Application.UseCases.Auth;
using Application.UseCases.Student;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Persistence;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<StudentController> logger;
        private readonly IConfiguration configuration;
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public AccountController(
            IMediator _mediator,
            ILogger<StudentController> _logger,
            IMapper _mapper,
            IConfiguration _configuration)
        {
            mediator = _mediator;
            configuration = _configuration;
            logger = _logger;
            mapper = _mapper;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] AccountRegisterCommand model)
        {
            var rsp = await mediator.Send(model);
            logger.LogInformation("register response->" + rsp.ToJson());

            if (rsp.ErrorFlag)
                return BadRequest(rsp);
            else
                return Ok(rsp);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginCommand model)
        {
            var rsp = await mediator.Send(model);
            logger.LogInformation("login response->" + rsp.ToJson());

            if (rsp.ErrorFlag)
                return Unauthorized(rsp);
            else
                return Ok(rsp);
        }
    }
}