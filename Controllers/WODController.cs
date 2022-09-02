
using CrossFitWOD.DTOs;
using CrossFitWOD.Extensions;
using CrossFitWOD.Models;
using CrossFitWOD.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrossFitWOD.Controllers
{

    [ApiController]
    [Route("API/Workouts")]
    public class  WODController: ControllerBase
    {
        private readonly UnitOfWork _UnitOfWork;

        public WODController(UnitOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;
        }

        [HttpGet]
        public ActionResult<IEnumerable<WorkoutOfTheDayDTO>> GetWODs()
        {
            var wods = _UnitOfWork.WODRepository.GetAll()?.Select(wod => wod.ToDTO());

            if (wods is null)
                return NotFound();

            return wods.ToList();
        }

        [HttpGet("{id}")]
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
            WorkoutOfTheDay wod = new WorkoutOfTheDay
            {
                Id = Guid.NewGuid(),
                Title = newWOD.Title,
                Description = newWOD.Description,
                CoachTip = newWOD.CoachTip,
                Level = newWOD.Level,
                Date = newWOD.Date,
                Results = newWOD.Results
            };

            await _UnitOfWork.WODRepository.Create(wod);

            return CreatedAtAction(nameof(GetWOD), new { id = wod.Id }, wod.ToDTO());

        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateWOD(Guid id, CreateWorkoutOfTheDayDTO updatedWOD)
        {
            var currentWOD = await _UnitOfWork.WODRepository.GetByID(id);

            if (currentWOD is null)
                return NotFound();

            WorkoutOfTheDay refreshedWOD = currentWOD with
            {
                Title = updatedWOD.Title,
                Description = updatedWOD.Description,
                CoachTip = updatedWOD.CoachTip,
                Level = updatedWOD.Level,
                Date = updatedWOD.Date,
                Results = updatedWOD.Results
            };

            _UnitOfWork.WODRepository.Update(refreshedWOD);

            return NoContent();

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> UpdateWOD(Guid id)
        {
            var currentWOD = await _UnitOfWork.WODRepository.GetByID(id);

            if (currentWOD is null)
                return NotFound();

            await _UnitOfWork.WODRepository.Delete(id);

            return NoContent();

        }
    }
}