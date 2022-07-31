using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.CloudServices;
using MoviesAPI.DTOs;
using MoviesAPI.Entities;
using MoviesAPI.Helpers;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MoviesAPI.Controllers
{
    [ApiController]
    [Route("api/actors")]
    public class ActorsController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IFileStorage fileStorage;
        private readonly string container = "actors";

        public ActorsController(ApplicationDbContext context, IMapper mapper, IFileStorage fileStorage)
        {
            this.context = context;
            this.mapper = mapper;
            this.fileStorage = fileStorage;
        }

        [HttpGet]
        public async Task<ActionResult<List<ActorDTO>>> Get([FromQuery] PaginationDTO paginationDTO)
        {
            var queryable = context.Actors.AsQueryable();
            await HttpContext.InsertRecordParameter(queryable, paginationDTO.NumbersOfRecordsPerPage);

            var actors = await queryable.Paginate(paginationDTO).ToListAsync();
            var dtos = mapper.Map<List<ActorDTO>>(actors);
            return dtos;
        }

        [HttpGet("{id}", Name = "GetActor")]
        public async Task<ActionResult<ActorDTO>> Get(int id)
        {
            var actor = await context.Actors.FirstOrDefaultAsync(x => x.Id == id);

            if (actor == null)
            {
                return NotFound();
            }

            return mapper.Map<ActorDTO>(actor);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm] ActorCreationDTO actorCreationDTO)
        {
            var actor = mapper.Map<Actor>(actorCreationDTO);

            if (actorCreationDTO.photoFile != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await actorCreationDTO.photoFile.CopyToAsync(memoryStream);
                    var content = memoryStream.ToArray();
                    var extension = Path.GetExtension(actorCreationDTO.photoFile.FileName);
                    actor.Photo = await fileStorage.SaveFile(content, extension, container, actorCreationDTO.photoFile.ContentType);
                }
            }

            context.Add(actor);
            await context.SaveChangesAsync();
            var actorDto = mapper.Map<ActorDTO>(actor);
            return new CreatedAtRouteResult("GetActor", new { id = actor.Id }, actorDto);

        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromForm] ActorCreationDTO actorCreationDTO)
        {
            var actorDB = await context.Actors.FirstOrDefaultAsync(x => x.Id == id);

            if (actorDB == null)
            {
                return NotFound();
            }

            actorDB = mapper.Map(actorCreationDTO, actorDB);

            if (actorCreationDTO.photoFile != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await actorCreationDTO.photoFile.CopyToAsync(memoryStream);
                    var content = memoryStream.ToArray();
                    var extension = Path.GetExtension(actorCreationDTO.photoFile.FileName);
                    actorDB.Photo = await fileStorage.EditFile(content, extension, container, actorDB.Photo, actorCreationDTO.photoFile.ContentType);
                }
            }

            await context.SaveChangesAsync();
            return Ok();

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exist = await context.Actors.AnyAsync(x => x.Id == id);

            if (!exist)
            {
                return NotFound();
            }

            context.Remove(new Actor() { Id = id });
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<ActorPatchDTO> patchDocument )
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }

            var entityDB = await context.Actors.FirstOrDefaultAsync(x=>x.Id ==id);

            if (entityDB == null)
            {
                return NotFound();
            }

            var entityDTO = mapper.Map<ActorPatchDTO>(entityDB);

            patchDocument.ApplyTo(entityDTO, ModelState);

            var isValid = TryValidateModel(entityDTO);

            if (!isValid)
            {
                return BadRequest(ModelState);
            }

            mapper.Map(entityDTO, entityDB);

            await context.SaveChangesAsync();

            return NoContent();

        }
    }
}