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
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Persistence;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly ILogger<StudentController> logger;
        private readonly IConfiguration configuration;
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public StudentController(
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


        [EnableQuery]
        [HttpGet]
        [ProducesResponseType(typeof(Student), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CommandResult<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(CommandResult<string>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get()
        {
            var request = new StudentQueryCommand();
            var rsp = await mediator.Send(request);

            if (rsp.ErrorFlag)
                return BadRequest(rsp);
            else
                return Ok(rsp.Response);
        }


        [EnableQuery]
        [HttpGet("({key})")]
        [ProducesResponseType(typeof(Student), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CommandResult<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(CommandResult<string>), StatusCodes.Status500InternalServerError)]
        public async Task<SingleResult<StudentViewModel>> Get(
            [FromODataUri] [Required] [CustomizeValidator(Skip = true)]
            int key)
        {
            var request = new StudentQueryCommand();
            var rsp = await mediator.Send(request);
            var queryable = rsp.Response.Where(p => p.Id == key);

            return SingleResult.Create(queryable.ProjectTo<StudentViewModel>(mapper.ConfigurationProvider));
        }


        [HttpPost("create")]
        [ProducesResponseType(typeof(CommandResult<StudentViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CommandResult<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(CommandResult<string>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] StudentCreateCommand model)
        {
            logger.LogInformation("create request->" + model.ToJson());

            var rsp = await mediator.Send(model);

            logger.LogInformation("create response->" + rsp.ToJson());

            if (rsp.ErrorFlag)
                return BadRequest(rsp);
            else
                return Ok(rsp);
        }


        [HttpPost("update")]
        [ProducesResponseType(typeof(CommandResult<StudentViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CommandResult<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(CommandResult<string>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromBody] StudentUpdateCommand model)
        {
            logger.LogInformation("update request->" + model.ToJson());

            var rsp = await mediator.Send(model);

            logger.LogInformation("update response->" + rsp.ToJson());

            if (rsp.ErrorFlag)
                return BadRequest(rsp);
            else
                return Ok(rsp);
        }


        [HttpPost("delete")]
        [ProducesResponseType(typeof(CommandResult<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CommandResult<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(CommandResult<string>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete([FromBody] StudentDeleteCommand model)
        {
            logger.LogInformation("delete request->" + model.ToJson());

            var rsp = await mediator.Send(model);
            
            logger.LogInformation("delete response->" + rsp.ToJson());

            if (rsp.ErrorFlag)
                return BadRequest(rsp);
            else
                return Ok(rsp);
        }
    }
}