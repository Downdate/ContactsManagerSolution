using Microsoft.AspNetCore.Http;

namespace ContactsManager.Core.ServiceContracts
{
    /// <summary>
    /// Represents business logic for manipulating Country entity
    /// </summary>
    public interface ICountriesUploaderService
    {
       
        /// <summary>
        /// Uploads country data from the specified Excel file and returns the number of countries
        /// successfully processed.
        /// </summary>
        /// <remarks>Ensure that the provided Excel file adheres to the expected format for country data.
        /// The method may throw exceptions if the file is invalid or if errors occur during the upload
        /// process.</remarks>
        /// <param name="formFile">The Excel file containing country data to be uploaded. This parameter must not be null and should be in a
        /// valid Excel format.</param>
        /// <returns>A task that represents the operation. The task result contains the number of countries that
        /// were successfully uploaded from the Excel file.</returns>
        Task<int> UploadCountriesFromExcelFile(IFormFile formFile);
    }
}