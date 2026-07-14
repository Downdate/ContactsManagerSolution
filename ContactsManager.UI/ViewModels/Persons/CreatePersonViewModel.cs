using ContactsManager.Core.DTO;


namespace CRUDContactManager.ViewModels.Persons
{
    public class CreatePersonViewModel
    {
        public PersonAddRequest Person { get; set; } = new();

        public List<CountryResponse> Countries { get; set; } = new();
    }
}