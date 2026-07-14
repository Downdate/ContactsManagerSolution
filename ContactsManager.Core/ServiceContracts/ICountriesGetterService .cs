using Microsoft.AspNetCore.Http;
using ContactsManager.Core.DTO;

namespace ContactsManager.Core.ServiceContracts
{
    /// <summary>
    /// Represents business logic for manipulating Country entity
    /// </summary>
    public interface ICountriesGetterService
    {
       
        /// <summary>
        /// Returns all countries from the list
        /// </summary>
        /// <returns>Returns all countries from the list as a list of CountryResponse</returns>
        Task<List<CountryResponse>> GetAllCountries();

        /// <summary>
        /// Retrieves country information for the specified country identifier.
        /// </summary>
        /// <param name="countryID">The unique identifier of the country to retrieve. Specify <see langword="null"/> to indicate that no country
        /// is selected.</param>
        /// <returns>A <see cref="CountryResponse"/> containing details of the country if found; otherwise, <see
        /// langword="null"/>.</returns>
        Task<CountryResponse?> GetCountryByCountryID(Guid? countryID);

    }
}