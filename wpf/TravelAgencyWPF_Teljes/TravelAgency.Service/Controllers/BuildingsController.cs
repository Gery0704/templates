using ELTE.TravelAgency.Data;
using ELTE.TravelAgency.Service.Models;
using System;
using System.Linq;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ELTE.TravelAgency.Service.Controllers
{
	/// <summary>
	/// Épületek lekérdezését és módosítását biztosító vezérlő.
	/// </summary>
	[Route("api/[controller]")]
	public class BuildingsController : Controller
    {
        private readonly TravelAgencyContext _context;
        
        /// <summary>
        /// Vezérlő példányosítása.
        /// </summary>
        /// <param name="context">Entitásmodell.</param>
        public BuildingsController(TravelAgencyContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

	        _context = context;
        }

        /// <summary>
        /// Épületek lekérdezése.
        /// </summary>
        [HttpGet]

		public IActionResult GetBuildings()
        {
            try
            {
                return Ok(_context.Buildings.Include(b => b.City).ToList().Select(building => new BuildingDTO
                {
                    Id = building.Id,
                    Name = building.Name,
					City = new CityDTO { Id = building.City.Id, Name = building.City.Name },
					SeaDistance = building.SeaDistance,
                    ShoreId = building.ShoreId,
                    LocationX = building.LocationX,
                    LocationY = building.LocationY,
                    Comment = building.Comment
                }));
            }
            catch
            {
				// Internal Server Error
	            return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Épület lekérdezése.
        /// </summary>
        /// <param name="id">Épület azonosító.</param>
        [HttpGet("{id}")]
        public IActionResult GetBuilding(Int32 id)
        {
            try
            {
                var building = _context.Buildings.Include(b => b.City).Single(b => b.Id == id);
                return Ok(new BuildingDTO
                {
                    Id = building.Id,
                    Name = building.Name,
                    City = new CityDTO { Id = building.City.Id, Name = building.City.Name },
                    SeaDistance = building.SeaDistance,
                    ShoreId = building.ShoreId,
                    LocationX = building.LocationX,
                    LocationY = building.LocationY,
                    Comment = building.Comment
                });
            }
            catch
            {
                // Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Épületek lekérdezése.
        /// </summary>
        /// <param name="id">Város azonosító.</param>
        [HttpGet("city/{id}")]
        public IActionResult GetBuildingForCity(Int32 id)
        {
            try
            {
                return Ok(_context.Buildings.Include(b => b.City).Where(b => b.CityId == id).ToList().Select(building => new BuildingDTO
                {
                    Id = building.Id,
                    Name = building.Name,
                    City = new CityDTO { Id = building.City.Id, Name = building.City.Name },
                    SeaDistance = building.SeaDistance,
                    ShoreId = building.ShoreId,
                    LocationX = building.LocationX,
                    LocationY = building.LocationY,
                    Comment = building.Comment
                }));
            }
            catch
            {
                // Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Új épület létrehozása.
        /// </summary>
        /// <param name="buildingDTO">Épület.</param>
        [HttpPost]
        [Authorize(Roles = "administrator")]
		public IActionResult PostBuilding([FromBody] BuildingDTO buildingDTO)
        {
            try
            {
                var addedBuilding = _context.Buildings.Add(new Building
                {
                    Name = buildingDTO.Name,
                    CityId = buildingDTO.City.Id,
                    SeaDistance = buildingDTO.SeaDistance,
                    ShoreId = buildingDTO.ShoreId.Value,
                    LocationX = buildingDTO.LocationX,
                    LocationY = buildingDTO.LocationY,
                    Comment = buildingDTO.Comment
                });

                _context.SaveChanges(); // elmentjük az új épületet

	            buildingDTO.Id = addedBuilding.Entity.Id;

                // visszaküldjük a létrehozott épületet
                return Created(Request.GetUri() + addedBuilding.Entity.Id.ToString(), buildingDTO);
            }
            catch
            {
				// Internal Server Error
	            return StatusCode(StatusCodes.Status500InternalServerError);
			}
        }

        /// <summary>
        /// Épület módosítása.
        /// </summary>
        /// <param name="buildingDTO">Épület.</param>
        [HttpPut]
        [Authorize(Roles = "administrator")]
		public IActionResult PutBuilding([FromBody] BuildingDTO buildingDTO)
        {
            try
            {
                Building building = _context.Buildings.FirstOrDefault(b => b.Id == buildingDTO.Id);

                if (building == null) // ha nincs ilyen azonosító, akkor hibajelzést küldünk
                    return NotFound();

                building.Name = buildingDTO.Name;
                building.CityId = buildingDTO.City.Id;
                building.SeaDistance = buildingDTO.SeaDistance;
                building.ShoreId = buildingDTO.ShoreId.Value;
                building.LocationX = buildingDTO.LocationX;
                building.LocationY = buildingDTO.LocationY;
                building.Comment = buildingDTO.Comment;

                _context.SaveChanges(); // elmentjük a módosított épületet

                return Ok();
            }
            catch
            {
				// Internal Server Error
	            return StatusCode(StatusCodes.Status500InternalServerError);
			}
        }

		/// <summary>
		/// Épület törlése.
		/// </summary>
		/// <param name="id">Épület azonosító.</param>
		[HttpDelete("{id}")]
		[Authorize(Roles = "administrator")]
		public IActionResult DeleteBuilding(Int32 id)
        {
            try
            {
                Building building = _context.Buildings.FirstOrDefault(b => b.Id == id);

                if (building == null) // ha nincs ilyen azonosító, akkor hibajelzést küldünk
                    return NotFound();

	            _context.Buildings.Remove(building);

	            _context.SaveChanges(); // elmentjük a módosított épületet

                return Ok();
            }
            catch
            {
				// Internal Server Error
	            return StatusCode(StatusCodes.Status500InternalServerError);
			}
        }
    }
}
