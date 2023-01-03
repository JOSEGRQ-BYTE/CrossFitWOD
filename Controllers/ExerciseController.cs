using System;
using AutoMapper;
using CrossFitWOD.DTOs;
using CrossFitWOD.Interfaces;
using CrossFitWOD.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CrossFitWOD.Controllers
{
    [ApiController]
    [Route("API/[controller]")]
    [Authorize]
    public class ExerciseController : ControllerBase
	{

        private readonly IUnitOfWork _context;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public ExerciseController(IUnitOfWork context, UserManager<User> userManager, IMapper mapper, IConfiguration configuration)
		{
            _context = context;
            _configuration = configuration;
            _userManager = userManager;
            _mapper = mapper;
		}

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetAll()
        {
            return Ok(_context.ExerciseRepository.GetAll().ToList());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            if (String.IsNullOrEmpty(id))
                return BadRequest("Invalid id");

            var exercise = await _context.ExerciseRepository.GetByID<string>(id);

            if (exercise is null)
                return BadRequest("Exercise not found");

            return Ok(_mapper.Map<ExerciseResponse>(exercise));
        }

        [HttpPost]
        public async Task<IActionResult> Add(ExerciseRequest exercise)
        {
            var currentIdentity = User.Identity;
            if (currentIdentity is not null && currentIdentity.IsAuthenticated)
            {

                var currentUser = await _userManager.FindByEmailAsync(currentIdentity.Name);

                Exercise newExercise = _mapper.Map<Exercise>(exercise);
                newExercise.Id = Guid.NewGuid().ToString();
                newExercise.Created = DateTime.Now;
                newExercise.LastUpdated = DateTime.Now;

                await _context.ExerciseRepository.Create(newExercise);

                return CreatedAtAction(nameof(Get), new { id = newExercise.Id }, _mapper.Map<ExerciseResponse>(newExercise));
            }

            return StatusCode(400, "User not logged in");

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var existingExercise = await _context.ExerciseRepository.GetByID<string>(id);

            if (existingExercise is null)
                return BadRequest("Exercise not found");

            try
            {
                await _context.ExerciseRepository.Delete(id);
            }
            catch (Exception)
            {
                StatusCode(500, "Internal server error");
            }

            return NoContent();

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] ExerciseRequest exercise)
        {
            if (String.IsNullOrEmpty(id))
                return BadRequest("Invalid id");

            try
            {
                var existingExercise = await _context.ExerciseRepository.GetByID<string>(id);
                if (existingExercise is null)
                    return BadRequest("Exercise not found");

                existingExercise.ExerciseName = exercise.ExerciseName;
                existingExercise.Description = exercise.Description;
                existingExercise.LastUpdated = DateTime.Now;

                _context.ExerciseRepository.Update(existingExercise);

                return StatusCode(201, _mapper.Map<ExerciseResponse>(existingExercise));
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}

