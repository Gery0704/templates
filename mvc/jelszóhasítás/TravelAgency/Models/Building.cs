using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ELTE.TravelAgency.Models
{
	/// <summary>
	///Tengerpart típusa.
	/// </summary>
	public enum ShoreType
	{
		/// <summary>
		/// Homokos
		/// </summary>
		Sandy,
		/// <summary>
		/// Kavicsos
		/// </summary>
		Gravelly,
		/// <summary>
		/// Sziklás
		/// </summary>
		Rocky
	}

	public class Building
	{
		public Building()
		{
			Apartments = new HashSet<Apartment>();
		}

		public int Id { get; set; }
		public string Name { get; set; }
		public int CityId { get; set; }
		public int SeaDistance { get; set; }

		[UIHint("ShoreTypeDisplay")] // megadjuk a megjelenítés módját
		public ShoreType ShoreId { get; set; }
		public double LocationX { get; set; }
		public double LocationY { get; set; }
		public string Comment { get; set; }
		

		public ICollection<Apartment> Apartments { get; set; }
		public City City { get; set; }
	}
}
