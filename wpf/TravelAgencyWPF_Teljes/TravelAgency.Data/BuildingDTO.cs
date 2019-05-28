using System;
using System.Collections.Generic;

namespace ELTE.TravelAgency.Data
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

	/// <summary>
	/// Épület típusa.
	/// </summary>
	public class BuildingDTO
    {
        /// <summary>
        /// Épület azonosítója.
        /// </summary>
        public Int32 Id { get; set; }

        /// <summary>
        /// Épület neve.
        /// </summary>
        public String Name { get; set; }

		/// <summary>
		/// Város.
		/// </summary>
		public CityDTO City { get; set; }

		/// <summary>
		/// Tengerpart távolság.
		/// </summary>
		public Int32 SeaDistance { get; set; }
        
        /// <summary>
        /// Tengerpart típusa.
        /// </summary>
        public ShoreType? ShoreId { get; set; }
        
        /// <summary>
        /// Jellemzők.
        /// </summary>
        //public Int32? Features { get; set; }
        
        /// <summary>
        /// X pozíció.
        /// </summary>
        public Double LocationX { get; set; }
        
        /// <summary>
        /// Y pozíció.
        /// </summary>
        public Double LocationY { get; set; }
        
        /// <summary>
        /// Megjegyzés.
        /// </summary>
        public String Comment { get; set; }

	    /// <summary>
	    /// Képek.
	    /// </summary>
	    public IList<ImageDTO> Images { get; set; }

        /// <summary>
        /// Új <see cref="BuildingDTO"/> objektum példányosítása.
        /// </summary>
        public BuildingDTO()
        {
            Images = new List<ImageDTO>();
        }

        /// <summary>
        /// Egyenlőségvizsgálat.
        /// </summary>
        public override Boolean Equals(Object obj)
	    {
		    return (obj is BuildingDTO dto) && Id == dto.Id;
	    }
	}
}
