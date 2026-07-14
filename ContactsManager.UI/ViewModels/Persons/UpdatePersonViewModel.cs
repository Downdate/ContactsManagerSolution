using ContactsManager.Core.DTO;


namespace CRUDContactManager.ViewModels.Persons
{
    public class UpdatePersonViewModel
    {
        public PersonUpdateRequest Person { get; set; } = new();

        public List<CountryResponse> Countries { get; set; } = new();
    }
}