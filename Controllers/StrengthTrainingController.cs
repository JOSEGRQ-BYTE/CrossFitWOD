using System;
using System.Data;
using System.Data.SqlClient;
using AutoMapper;
using CrossFitWOD.DTOs;
using CrossFitWOD.Extensions;
using CrossFitWOD.Interfaces;
using CrossFitWOD.Models;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CrossFitWOD.Modules;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace CrossFitWOD.Controllers
{
    [ApiController]
    [Route("API/[controller]")]
    [Authorize]
    public class StrengthTrainingController : ControllerBase
	{

        private readonly IUnitOfWork _context;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public StrengthTrainingController(IUnitOfWork unitOfWork, UserManager<User> userManager, IMapper mapper, IConfiguration configuration)
		{
            _context = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;
            _configuration = configuration;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetWorkouts()
        {

            return Ok(_context.StrengthTrainingRepository.GetAll().ToList());

        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            if (String.IsNullOrEmpty(id))
                return BadRequest("Invalid id");

            var workout = await _context.StrengthTrainingRepository.GetByID<string>(id);

            if (workout is null)
                return BadRequest("Workout not found");

            return Ok(_mapper.Map<StrengthTrainingResponse>(workout));
        }

        
        [HttpPost]
        public async Task<IActionResult> Add(StrengthTrainingRequest workout)
        {
            var currentIdentity = User.Identity;
            if (currentIdentity is not null && currentIdentity.IsAuthenticated)
            {

                var currentUser = await _userManager.FindByEmailAsync(currentIdentity.Name);

                StrengthTraining newWorkout = _mapper.Map<StrengthTraining>(workout);
                newWorkout.Id = Guid.NewGuid().ToString();
                newWorkout.Created = DateTime.Now;
                newWorkout.LastUpdated = DateTime.Now;
                newWorkout.UserId = currentUser.Id;

                await _context.StrengthTrainingRepository.Create(newWorkout);

                return CreatedAtAction(nameof(Get), new { id = newWorkout.Id }, _mapper.Map<StrengthTrainingResponse>(newWorkout));
            }

            return StatusCode(400, "User not logged in");

        }

        [HttpGet("GetWorkoutStatistics")]
        [AllowAnonymous]
        public async Task<IActionResult> GetStrengthWorkouts()
        {
            var administrators = await _userManager.GetUsersInRoleAsync("Administrator");
            var appAdministrator = administrators.FirstOrDefault();

            if (appAdministrator is not null)
            {

                //var currentUser = await _userManager.FindByEmailAsync(appAdministrator.Email);
                SqlParameter[] parameters = {
                  new SqlParameter("@OperationType", SqlDbType.NVarChar, 50) { Value = "GET_STRENGTH_STATS" },
                  new SqlParameter("@UserID", SqlDbType.NVarChar, 450) { Value = appAdministrator.Id },
                };


                DataSet ds = DataHelper.ExecuteStoredProcedure(_configuration.GetConnectionString("DefaultConnection"), "[Workouts].[ManageStrengthWorkouts]", parameters);

                return Ok(JsonConvert.SerializeObject(ds.Tables[0]));
            }
            return BadRequest();
        }


        [HttpGet("GetDetailedWorkouts")]
        [AllowAnonymous]
        public async Task<IActionResult> GetDetailedWorkouts()
        {
            var administrators = await _userManager.GetUsersInRoleAsync("Administrator");
            var appAdministrator = administrators.FirstOrDefault();

            if (appAdministrator is not null)
            {
                SqlParameter[] parameters = {
                  new SqlParameter("@OperationType", SqlDbType.NVarChar, 50) { Value = "GET_DETAILED_WORKOUTS" },
                  new SqlParameter("@UserID", SqlDbType.NVarChar, 450) { Value = appAdministrator.Id },
                };

                DataSet ds = DataHelper.ExecuteStoredProcedure(_configuration.GetConnectionString("DefaultConnection"), "[Workouts].[ManageStrengthWorkouts]", parameters);

                return Ok(JsonConvert.SerializeObject(ds.Tables[0]));
            }
            return BadRequest();
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWOD(string id, [FromBody] StrengthTrainingRequest updatedExercise)
        {
            var currentExercise = await _context.StrengthTrainingRepository.GetByID(id);

            if (currentExercise is null)
                return BadRequest("Workout not found");

            currentExercise.ExerciseId = updatedExercise.ExerciseId;
            currentExercise.Weight = updatedExercise.Weight;
            currentExercise.IsBodyweight = updatedExercise.IsBodyweight;
            currentExercise.Reps = updatedExercise.Reps;
            currentExercise.Sets = updatedExercise.Sets;

            try
            {
                _context.StrengthTrainingRepository.Update(currentExercise);
                return StatusCode(200, _mapper.Map<StrengthTrainingResponse>(currentExercise));
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var existingWorkout = await _context.StrengthTrainingRepository.GetByID<string>(id);

            if (existingWorkout is null)
                return BadRequest("Workout not found");

            try
            {
                await _context.StrengthTrainingRepository.Delete(id);
            }
            catch (Exception)
            {
                StatusCode(500, "Internal server error");
            }

            return NoContent();

        }
    }
}

