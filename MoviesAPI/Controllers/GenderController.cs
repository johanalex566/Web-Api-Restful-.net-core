using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MoviesAPI.DTOs;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using MoviesAPI.Entities;

namespace MoviesAPI.Controllers
{
    [ApiController]
    [Route("api/genders")]
    public class GenderController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public GenderController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<GenderDTO>>> Get()
        {
            var genders = await context.Genders.ToListAsync();
            var dtos = mapper.Map<List<GenderDTO>>(genders);
            return dtos;
        }

        [HttpGet("{id:int}", Name = "GetGender")]
        public async Task<ActionResult<GenderDTO>> Get(int id)
        {
            var gender = await context.Genders.FirstOrDefaultAsync(x => x.Id == id);
            if (gender == null)
            {
                return NotFound();
            }

            var dto = mapper.Map<GenderDTO>(gender);

            return dto;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] GenderCreationDTO genderCreationDTO)
        {

            var gender = mapper.Map<Gender>(genderCreationDTO);
            context.Add(gender);
            await context.SaveChangesAsync();
            var genderDTO = mapper.Map<GenderDTO>(gender);

            return new CreatedAtRouteResult("GetGender", new { id = genderDTO.Id }, gender);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] GenderCreationDTO genderCreationDTO)
        {
            var gender = mapper.Map<Gender>(genderCreationDTO);
            gender.Id = id;
            context.Entry(gender).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exist = await context.Genders.AnyAsync(x => x.Id == id);

            if (!exist)
            {
                return NotFound();
            }

            context.Remove(new Gender() { Id = id });
            await context.SaveChangesAsync();
            return Ok();

        }
    }
}
