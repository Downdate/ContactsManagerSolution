using Microsoft.AspNetCore.Http;
using ContactsManager.Core.DTO;

namespace ContactsManager.Core.ServiceContracts
{
    /// <summary>
    /// Represents business logic for manipulating Country entity
    /// </summary>
    public interface ICountriesAdderService
    {
        /// <summary>
        /// Adds a country object to the list of countries
        /// </summary>
        /// <param name="countryAddRequest">Country object to be added</param>
        /// <returns>Returns the country object after adding it (including newly generated country id) </returns>
        Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest);

    }
}