
using System.Security.Claims;
using CrossFitWOD.Data;
using CrossFitWOD.DTOs;
using CrossFitWOD.Extensions;
using CrossFitWOD.Interfaces;
using CrossFitWOD.Models;
using CrossFitWOD.Repositories;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrossFitWOD.Controllers
{

    [ApiController]
    [Route("API/Workouts")]
    //[Authorize(Roles = "Administrator")]
    [Authorize]
    public class  WODController: ControllerBase
    {

        private readonly IUnitOfWork _UnitOfWork;
        private readonly UserManager<User> _UserManager;
        private readonly SignInManager<User> _SignInManager;

        public WODController(IUnitOfWork unitOfWork, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _UnitOfWork = unitOfWork;
            _UserManager = userManager;
            _SignInManager = signInManager;
        }

        [HttpGet("GetWODs")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<WorkoutOfTheDayDTO>>> GetWODs([FromQuery] string? userID = null)
        {

            IQueryable<WorkoutOfTheDayDTO> wods;

            // By default we get the WODS of the administrator
            if (userID is null)
            {
                var administrators = await _UserManager.GetUsersInRoleAsync("Administrator");
                var appAdministrator = administrators.FirstOrDefault();

                if (appAdministrator is null)
                    return NotFound();

                wods = _UnitOfWork.WODRepository.GetAll().Where(wod => wod.Date >= DateTime.Now.AddDays(-7) && wod.UserId == appAdministrator.Id).OrderBy(wod => wod.Date).Select(wod => wod.ToDTO());
            }
            // user is logged in get their wods
            else
            {

                var currentuser = await _UserManager.FindByIdAsync(userID);

                if (currentuser is null)
                    return NotFound();

                wods = _UnitOfWork.WODRepository.GetAll().Where(wod => wod.Date >= DateTime.Now.AddDays(-7) && wod.UserId == currentuser.Id).OrderBy(wod => wod.Date).Select(wod => wod.ToDTO());

            }

            if (wods is null)
                return NotFound();

            return wods.ToList();
        }

        [HttpGet("{id}")]
        //[AllowAnonymous]
        public async Task<ActionResult<WorkoutOfTheDayDTO>> GetWOD(Guid id)
        {
            var wod = await _UnitOfWork.WODRepository.GetByID<Guid>(id);

            if(wod is null)
                return NotFound();

            return wod.ToDTO();
        }

        [HttpPost]
        public async Task<ActionResult<WorkoutOfTheDayDTO>> CreateWOD(CreateWorkoutOfTheDayDTO newWOD)
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

                return CreatedAtAction(nameof(GetWOD), new { id = wod.Id }, wod.ToDTO());
            }

            return StatusCode(400, "User not logged in.");

        }

        [HttpPut("{id}")]
        public async Task<ActionResult<WorkoutOfTheDayDTO>> UpdateWOD(Guid id, [FromBody] UpdateWorkoutOfTheDayDTO updatedWOD)
        {
            var currentWOD = await _UnitOfWork.WODRepository.GetByID(id);

            if (currentWOD is null)
                return NotFound();

            /*WorkoutOfTheDay refreshedWOD = currentWOD with
            {
                Title = updatedWOD.Title,
                Description = updatedWOD.Description,
                CoachTip = updatedWOD.CoachTip,
                Level = updatedWOD.Level,
                Date = updatedWOD.Date,
                Results = updatedWOD.Results
            };*/


            currentWOD.Title = updatedWOD.Title;
            currentWOD.Description = updatedWOD.Description;
            currentWOD.CoachTip = updatedWOD.CoachTip;
            currentWOD.Level = updatedWOD.Level;
            currentWOD.Date = updatedWOD.Date;
            currentWOD.Results = updatedWOD.Results;

            _UnitOfWork.WODRepository.Update(currentWOD);

            return StatusCode(200, currentWOD);

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteWOD(Guid id)
        {
            var currentWOD = await _UnitOfWork.WODRepository.GetByID(id);

            if (currentWOD is null)
                return NotFound();

            await _UnitOfWork.WODRepository.Delete(id);

            return NoContent();

        }
    }
}