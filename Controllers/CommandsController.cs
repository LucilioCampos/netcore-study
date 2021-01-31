using System.Collections.Generic;
using AutoMapper;
using Commander.Data;
using Commander.Dtos;
using Commander.Models;
using Hangfire;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Commander.Controllers
{
    [Route("api/commands")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly ICommanderRepository _respository;
        private readonly IMapper _mapper;

        public CommandsController(ICommanderRepository repository, IMapper mapper)
        {
            _respository = repository;
            _mapper = mapper;
        }
        //GET api/commands
        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDto>> GetAllCommands()
        {
            var commandItems = _respository.GetAllCommands();
            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commandItems));
        }
        //GET api/commands/{id}
        [HttpGet("{id}", Name = "GetCommand")]
        public ActionResult<CommandReadDto> GetCommand(int id)
        {
            var command = _respository.GetCommandById(id);
            BackgroundJob.Enqueue(() => _respository.DeleteCommand(command));

            if (command != null)
            {
                return Ok(_mapper.Map<CommandReadDto>(command));
            }
            return NotFound();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteCommand(int id)
        {
            var command = _respository.GetCommandById(id);
             if (command == null)
            {
                return NotFound();
            }

            _respository.DeleteCommand(command);
            return NoContent();
        }

        //PUT api/commands/{id}
        [HttpPut("{id}")]
        public ActionResult<Command> UpdateCommand(int id, CommandUpdateDto commandUpdateDto)
        {
            var command = _respository.GetCommandById(id);

            if (command == null)
            {
                return NotFound();
            }

            _mapper.Map(commandUpdateDto, command);
            _respository.UpdateCommand(command);

            _respository.SaveChanges();

            var commandReadDto = _mapper.Map<CommandReadDto>(command);

            return CreatedAtRoute(nameof(GetCommand), new { Id = commandReadDto.Id }, commandReadDto);
        }

        //POST api/commands
        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommand(CommandCreateDto commandCreateDto)
        {
            var commandModel = _mapper.Map<Command>(commandCreateDto);
            _respository.CreateCommand(commandModel);
            _respository.SaveChanges();

            var commandReadDto = _mapper.Map<CommandReadDto>(commandModel);

            return CreatedAtRoute(nameof(GetCommand), new { Id = commandReadDto.Id }, commandReadDto);
        }
        //PATCH api/commands/{id}
        [HttpPatch("{id}")]
        public ActionResult PatchCommand(int id, [FromBody]JsonPatchDocument<CommandUpdateDto> patchDocument)
        {
             var repoCommand = _respository.GetCommandById(id);

            if (repoCommand == null)
            {
                return NotFound();
            }

            var commandToPatch = _mapper.Map<CommandUpdateDto>(repoCommand);
            patchDocument.ApplyTo(commandToPatch, ModelState);

            if(!TryValidateModel(commandToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(commandToPatch, repoCommand);

            _respository.UpdateCommand(repoCommand);

            _respository.SaveChanges();

            var commandReadDto = _mapper.Map<CommandReadDto>(repoCommand);

            return NoContent();

        }
    }
}