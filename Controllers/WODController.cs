
using System.Data;
using System.Data.SqlClient;
using System.Security.Claims;
using AutoMapper;
using CrossFitWOD.Data;
using CrossFitWOD.DTOs;
using CrossFitWOD.Extensions;
using CrossFitWOD.Interfaces;
using CrossFitWOD.Models;
using CrossFitWOD.Modules;
using CrossFitWOD.Repositories;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace CrossFitWOD.Controllers
{

    [ApiController]
    [Route("API/Workouts")]
    [Authorize]
    public class  WODController: ControllerBase
    {

        private readonly IUnitOfWork _UnitOfWork;
        private readonly UserManager<User> _UserManager;
        private readonly SignInManager<User> _SignInManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public WODController(IUnitOfWork unitOfWork, UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration, IMapper  mapper)
        {
            _UnitOfWork = unitOfWork;
            _UserManager = userManager;
            _SignInManager = signInManager;
            _configuration = configuration;
            _mapper = mapper;
        }

        #region GETALL
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetWODs([FromQuery] string? userID = null)
        {
            User? user;

            if (userID is null)
            {
                var administrators = await _UserManager.GetUsersInRoleAsync("Administrator");
                user = administrators.FirstOrDefault();
            }
            else
                user = await _UserManager.FindByIdAsync(userID);

            if (user is null)
                return NotFound();

            var wods = _UnitOfWork.WODRepository.GetAll().Where(wod => wod.UserId == user.Id).OrderBy(wod => wod.Date).Select(wod => _mapper.Map<WODResponse>(wod));

            return Ok(wods);
        }
        #endregion

        #region GETLATEST
        [HttpGet("GetLatestWODs")]
        [AllowAnonymous]
        public async Task<IActionResult> GetLatestWODs([FromQuery] string? userID = null)
        {
            try
            {
                User? user;

                if (userID is null)
                {
                    var administrators = await _UserManager.GetUsersInRoleAsync("Administrator");
                    user = administrators.FirstOrDefault();
                }
                else
                    user = await _UserManager.FindByIdAsync(userID);

                if (user is null)
                    return NotFound();


                SqlParameter[] parameters = {
                    new SqlParameter("@OperationType", SqlDbType.NVarChar, 50) { Value = "GET_LATEST_WODS" },
                    new SqlParameter("@UserID", SqlDbType.NVarChar, 450) { Value = user.Id },
                };

                DataSet ds = DataHelper.ExecuteStoredProcedure(_configuration.GetConnectionString("DefaultConnection"), "[Workouts].[ManageStrengthWorkouts]", parameters);

                return Ok(JsonConvert.SerializeObject(ds.Tables[0]));
            }
            catch (Exception error)
            {
                return StatusCode(500, "Internal server error");
            }
        }
        #endregion

        #region GET
        [HttpGet("{id}")]
        public async Task<IActionResult> GetWOD(Guid id)
        {
            var wod = await _UnitOfWork.WODRepository.GetByID<Guid>(id);

            if (wod is null)
                return NotFound();

            return Ok(_mapper.Map<WODResponse>(wod));
        }
        #endregion

        #region ADD
        [HttpPost]
        public async Task<IActionResult> CreateWOD(WODRequest newWOD)
        {
            var currentIdentity = User.Identity;
            if (currentIdentity is not null && currentIdentity.IsAuthenticated)
            {
                var currentUser = await _UserManager.FindByEmailAsync(currentIdentity.Name);

                WorkoutOfTheDay wod = new WorkoutOfTheDay
                {
                    Id = Guid.NewGuid(),
                    Title = newWOD.Title,
                    Description = newWOD.Description,
                    CoachTip = newWOD.CoachTip,
                    Level = newWOD.Level,
                    Date = newWOD.Date,
                    User = currentUser,
                    Results = newWOD.Results,
                };

                await _UnitOfWork.WODRepository.Create(wod);

                return CreatedAtAction(nameof(GetWOD), new { id = wod.Id }, _mapper.Map<WODResponse>(wod));
            }

            return StatusCode(400, "User not logged in.");

        }
        #endregion

        #region UPDATE
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWOD(Guid id, [FromBody] UpdateWorkoutOfTheDayDTO updatedWOD)
        {
            var currentWOD = await _UnitOfWork.WODRepository.GetByID(id);

            if (currentWOD is null)
                return NotFound();

            currentWOD.Title = updatedWOD.Title;
            currentWOD.Description = updatedWOD.Description;
            currentWOD.CoachTip = updatedWOD.CoachTip;
            currentWOD.Level = updatedWOD.Level;
            currentWOD.Date = updatedWOD.Date;
            currentWOD.Results = updatedWOD.Results;

            _UnitOfWork.WODRepository.Update(currentWOD);

            return StatusCode(200, currentWOD);

        }
        #endregion

        #region DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWOD(Guid id)
        {
            var currentWOD = await _UnitOfWork.WODRepository.GetByID(id);

            if (currentWOD is null)
                return NotFound();

            await _UnitOfWork.WODRepository.Delete(id);

            return NoContent();

        }
        #endregion

        #region GETINRANGE
        [HttpPost("GetWODsInRange")]
        public async Task<IActionResult> GetWODsInRange(InRangeDTO range)
        {
            if (ModelState.IsValid)
            {
                var currentIdentity = User.Identity;
                if (currentIdentity is not null && currentIdentity.IsAuthenticated)
                {
                    var currentUser = await _UserManager.FindByEmailAsync(currentIdentity.Name);
                    var wods = _UnitOfWork.WODRepository.GetAll().Where(wod => wod.Date.Date >= range.From.Date && wod.Date.Date <= range.To.Date && wod.UserId == currentUser.Id).Select(wod => _mapper.Map<WODResponse>(wod));
                    var temp = wods.Count();
                    return Ok(wods);
                }
                return Unauthorized();
            }
            return BadRequest("Invalid form");
        }
        #endregion
    }
}