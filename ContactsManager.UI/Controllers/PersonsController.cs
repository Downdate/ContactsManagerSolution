using ContactsManager.Core.DTO;
using ContactsManager.Core.Enums;
using ContactsManager.Core.ServiceContracts;
using CRUDContactManager.Filters.ActionFilters;
using CRUDContactManager.ViewModels.Persons;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;

namespace CRUDContactManager.Controllers
{
    [Route("[Controller]")]
    
    public class PersonsController : Controller
    {
        //private fields
        private readonly IPersonsAdderService _personsAdderService;
        private readonly IPersonsDeleterService _personsDeleterService;
        private readonly IPersonsGetterService _personsGetterService;
        private readonly IPersonsSorterService _personsSorterService;
        private readonly IPersonsUpdaterService _personsUpdaterService;
        


        private readonly ICountriesGetterService _countriesGetterService;
        private readonly ILogger<PersonsController> _logger;

        //constructor
        public PersonsController(IPersonsAdderService personsAdderService, IPersonsDeleterService personsDeleterService, IPersonsGetterService personsGetterService, IPersonsSorterService personsSorterService ,IPersonsUpdaterService personsUpdaterService , ICountriesGetterService countriesGetterService, ILogger<PersonsController> logger)
        {
            _personsAdderService = personsAdderService;
            _personsDeleterService = personsDeleterService;
            _personsGetterService = personsGetterService;
            _personsSorterService = personsSorterService;
            _personsUpdaterService = personsUpdaterService;
            _countriesGetterService = countriesGetterService;

            _logger = logger;
        }
        
        [Route("[Action]")]
        [Route("/")]
        [TypeFilter(typeof(PersonsListActionFilter))]
        [TypeFilter(typeof(ResponseHeaderActionFilter), Arguments = new Object[] { "X-Custom-Key", "Custom-Value" })]
        public async Task<IActionResult> Index(string searchBy, string searchString, string sortBy = nameof(PersonResponse.PersonName), SortOrderOptions sortOrder = SortOrderOptions.ASC)
        {
            //Log
            _logger.LogInformation("Index action method of PersonsController");

            //Search
            List<PersonResponse> persons = await _personsGetterService.GetFilteredPersons(searchBy, searchString);



            //Sorting
            ViewBag.Columns = new Dictionary<string, string>()
            {
                {nameof(PersonResponse.PersonName) , "Person Name" },
                {nameof(PersonResponse.Email) , "Email Address" },
                {nameof(PersonResponse.DateOfBirth) , "Date of Birth" },
                {nameof(PersonResponse.Country), "Country" },
                {nameof(PersonResponse.Age), "Age" },
                {nameof(PersonResponse.Gender), "Gender" },
            };
            persons = await _personsSorterService.GetSortedPersons(persons, sortBy, sortOrder);
            ViewBag.CurrentSortBy = sortBy;
            ViewBag.CurrentSortOrder = sortOrder;

            return View(persons);
        }

        //Executed when HTTP GET /persons/create
        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            //Log
            _logger.LogInformation("Create HttpGet action method of PersonsController");

            CreatePersonViewModel model = new CreatePersonViewModel();
            model.Countries = await _countriesGetterService.GetAllCountries();

            return View(model);
        }

        //Executed when HTTP POST /persons/create
        [Route("[Action]")]
        [HttpPost]
        public async Task<IActionResult> Create(CreatePersonViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Countries = await _countriesGetterService.GetAllCountries();
                return View(model);
            }

            await _personsAdderService.AddPerson(model.Person);
            return RedirectToAction("Index");
        }

        //Executed when HTTP GET /persons/Update
        [Route("[action]/{personID}")]
        [HttpGet]
        public async Task<IActionResult> Update(Guid personID)
        {
            UpdatePersonViewModel model = new UpdatePersonViewModel();
            model.Countries = await _countriesGetterService.GetAllCountries();
            PersonResponse? personResponse = await _personsGetterService.GetPersonByPersonID(personID);
            if (personResponse == null)
            {
                return RedirectToAction("Index");
            }
            model.Person = personResponse.ToPersonUpdateRequest();

            return View(model);
        }

        //Executed when HTTP POST /persons/Update
        [Route("[Action]/{personID}")]
        [HttpPost]
        public async Task<IActionResult> Update(UpdatePersonViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Countries = await _countriesGetterService.GetAllCountries();
                return View(model);
            }

            await _personsUpdaterService.UpdatePerson(model.Person);
            return RedirectToAction("Index");
        }

        //Executed when HTTP GET /persons/Delete
        [Route("[action]/{personID}")]
        [HttpGet]
        public async Task<IActionResult> Delete(Guid personID)
        {
            PersonResponse? personResponse = await _personsGetterService.GetPersonByPersonID(personID);
            if (personResponse == null)
            {
                return RedirectToAction("Index");
            }
            return View(personResponse);
        }

        //Executed when HTTP POST /persons/Delete
        [Route("[action]/{personID}")]
        [HttpPost]
        public IActionResult Delete(PersonResponse model)
        {
            _personsDeleterService.DeletePerson(model.PersonID);
            return RedirectToAction("Index");
        }

        [Route("[Action]")]
        public async Task<IActionResult> PersonsPDF()
        {
            //Get List of Persons
            List<PersonResponse> persons = await _personsGetterService.GetAllPersons();
            return new ViewAsPdf("PersonsPDF", persons, ViewData)
            {
                PageMargins = new Rotativa.AspNetCore.Options.Margins(20, 20, 20, 20),
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape,
                CustomSwitches = "--enable-local-file-access"
            };
        }

        [Route("[Action]")]
        public async Task<IActionResult> PersonsCSV()
        {
            MemoryStream memoryStream = await _personsGetterService.GetPersonsCSV();

            return File(memoryStream, "application/octet-stream", "persons.csv");
        }

        [Route("[Action]")]
        public async Task<IActionResult> PersonsExcel()
        {
            MemoryStream memoryStream = await _personsGetterService.GetPersonsExcel();

            return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "persons.xlsx");
        }
    }
}