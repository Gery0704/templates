﻿using ELTE.TravelAgency.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ELTE.TravelAgency.Controllers
{
	/// <summary>
	/// Vezérlő típusa
	/// </summary>
	public class HomeController : Controller
	{
		// a logikát egy modell osztály mögé rejtjük
		private readonly ITravelService _travelService;

		// Google konfiguráció
		private readonly IOptions<GoogleConfig> _googleConfig;

		/// <summary>
		/// Vezérlő példányosítása.
		/// </summary>
		public HomeController(ITravelService travelService, IOptions<GoogleConfig> googleConfig)
		{
			_travelService = travelService;
			_googleConfig = googleConfig;
		}

		/// <summary>
		/// Egy akció meghívása után végrehajtandó metódus.
		/// </summary>
		/// <param name="context">Az akció kontextus argumentuma.</param>
		public override void OnActionExecuted(ActionExecutedContext context)
		{
			base.OnActionExecuted(context);

			// a városok listája
			ViewBag.Cities = _travelService.Cities.ToArray();
		}

		/// <summary>
		/// Épületek listázása.
		/// </summary>
		/// <returns>Az épületek listájának nézete.</returns>
		public IActionResult Index()
		{
			return View("Index", _travelService.Buildings.ToList());
		}

		/// <summary>
		/// Épületek listázása.
		/// </summary>
		/// <param name="cityId">Város azonosítója.</param>
		/// <returns>Az épületek listájának nézete.</returns>
		public IActionResult List(Int32 cityId)
		{
			// minden lekérdezés a modellen keresztül történik
			List<Building> buildings = _travelService.GetBuildings(cityId).ToList();

			if (buildings.Count == 0) // ha nincs ilyen épület
				return RedirectToAction(nameof(Index)); // átirányítjuk a kezdőoldalra

			return View("Index", buildings);
		}

		/// <summary>
		/// Épület részleteinek nézete.
		/// </summary>
		/// <param name="buildingId">Épület azonosítója.</param>
		/// <returns>Az épület részletes nézete.</returns>
		public IActionResult Details(Int32? buildingId)
		{
			Building building = _travelService.GetBuilding(buildingId);

			if (building == null) // nem találjuk az épületet
				return RedirectToAction(nameof(Index));

			// az oldal címe
			ViewBag.Title = "Épület részletei: " + building.Name + " (" + building.City.Name + ")";
			// az épülethez tarzozó képek azonosítói
			ViewBag.Images = _travelService.GetBuildingImageIds(building.Id).ToList();
			// Google Maps API Key
			ViewBag.GoogleMapsApiKey = _googleConfig.Value.MapsApiKey;

			return View("Details", building);
		}

		/// <summary>
		/// Épület főképének lekérdezése.
		/// </summary>
		/// <param name="buildingId">Épület azonosítója.</param>
		/// <returns>Az épület képe, vagy az alapértelmezett kép.</returns>
		public FileResult ImageForBuilding(Int32? buildingId)
		{
			// lekérjük az épület első tárolt képjét (kicsiben)
			Byte[] imageContent = _travelService.GetBuildingMainImage(buildingId);

			if (imageContent == null) // amennyiben nem sikerült betölteni, egy alapértelmezett képet adunk vissza
				return File("~/images/NoImage.png", "image/png");

			return File(imageContent, "image/png");
		}

		/// <summary>
		/// Épület egyik képének lekérdezése.
		/// </summary>
		/// <param name="imageId">Kép azonosítója.</param>
		/// <param name="large">Nagy méretű kép lekérése.</param>
		/// <returns>Az épület egy képe, vagy az alapértelmezett kép.</returns>
		public FileResult Image(Int32? imageId, Boolean large = false)
		{
			// lekérjük a megadott azonosítóval rendelkező képet
			Byte[] imageContent = _travelService.GetBuildingImage(imageId, large);

			if (imageContent == null) // amennyiben nem sikerült betölteni, egy alapértelmezett képet adunk vissza
				return File("~/images/NoImage.png", "image/png");

			return File(imageContent, "image/png");
		}
	}
}