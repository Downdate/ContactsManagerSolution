using ContactsManager.Core.DTO;

namespace ContactsManager.Core.ServiceContracts
{
    /// <summary>
    /// Represents business logic for manipulating Perosn entity
    /// </summary>
    public interface IPersonsGetterService
    {
        /// <summary>
        /// Returns all persons
        /// </summary>
        /// <returns>Returns a list of objects of PersonResponse type</returns>
        Task<List<PersonResponse>> GetAllPersons();

        /// <summary>
        /// Returns the person object based on the given person id
        /// </summary>
        /// <param name="personID">Person id to search</param>
        /// <returns>Returns matching person object</returns>
        Task<PersonResponse?> GetPersonByPersonID(Guid? personID);

        /// <summary>
        /// Returns all person objects that matches with the given search field and search string
        /// </summary>
        /// <param name="searchBy">Search field to search</param>
        /// <param name="searchString">Search string to search</param>
        /// <returns>Returns all matching persons based on the given search field and search string</returns>
        Task<List<PersonResponse>> GetFilteredPersons(string searchBy, string? searchString);

        /// <summary>
        /// Asynchronously generates a CSV file containing data for all persons.
        /// </summary>
        /// <remarks>The caller is responsible for disposing the returned <see cref="MemoryStream"/> after
        /// use. The CSV format includes all available person records and uses UTF-8 encoding.</remarks>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="MemoryStream"/>
        /// with the CSV data for all persons. The stream's position is set to the beginning.</returns>
        Task<MemoryStream> GetPersonsCSV();

        /// <summary>
        /// Asynchronously generates an Excel file containing a list of persons.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a memory stream with the Excel
        /// file data. The caller is responsible for disposing the returned stream.</returns>
        Task<MemoryStream> GetPersonsExcel();
    }
}