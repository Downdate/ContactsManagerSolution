using ContactsManager.Core.DTO;
using CRUDContactManager.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDContactManager.Filters.ActionFilters
{
    public class PersonsListActionFilter : IActionFilter
    {
        private readonly ILogger<PersonsListActionFilter> _logger;

        public PersonsListActionFilter(ILogger<PersonsListActionFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // To do: add after action execution logic here
            _logger.LogInformation("PersonsListActionFilter: OnActionExecuted method is executing");

            PersonsController personsController = (PersonsController)context.Controller;

            IDictionary<string, object?>? parameters = (IDictionary<string, object?>?)context.HttpContext.Items["arguments"];

            if (parameters != null) 
            {
                if (parameters.ContainsKey("Currentarguments")) 
                {
                    personsController.ViewData["CurrentsearchBy"] = Convert.ToString(parameters["CurrentsearchBy"]);
                }

                if (parameters.ContainsKey("CurrentsearchString")) 
                {
                    personsController.ViewData["CurrentsearchString"] = Convert.ToString(parameters["CurrentsearchString"]);
                }

                if (parameters.ContainsKey("CurrentSortBy")) 
                {
                    personsController.ViewData["CurrentSortBy"] = Convert.ToString(parameters["CurrentSortBy"]);
                }

                if (parameters.ContainsKey("CurrentSortOrder")) 
                {
                    personsController.ViewData["CurrentSortOrder"] = Convert.ToString(parameters["CurrentsortOrder"]);
                }
            }

            personsController.ViewBag.SearchFields = new Dictionary<string, string>()
            {
                {nameof(PersonResponse.PersonName) , "Person Name" },
                {nameof(PersonResponse.Email) , "Email Address" },
                {nameof(PersonResponse.DateOfBirth) , "Date of Birth" },
                {nameof(PersonResponse.Country), "Country" },
                {nameof(PersonResponse.Age), "Age" },
                {nameof(PersonResponse.Gender),"Gender" }
            };
            


        }
        
        public void OnActionExecuting(ActionExecutingContext context)
        {
            context.HttpContext.Items["arguments"] = context.ActionArguments;

            // To do: add before action execution logic here 
            _logger.LogInformation("PersonsListActionFilter: OnActionExecuting method is executing");

            if (context.ActionArguments.ContainsKey("searchBy")) 
            {
                string? searchBy = Convert.ToString(context.ActionArguments["searchBy"]);

                if (!string.IsNullOrEmpty(searchBy))
                {
                    var searchByOptions = new List<string>() 
                    {
                        nameof(PersonResponse.PersonName),
                        nameof(PersonResponse.Email),
                        nameof(PersonResponse.DateOfBirth),
                        nameof(PersonResponse.Gender),
                        nameof(PersonResponse.CountryID),
                        nameof(PersonResponse.Address),

                    };
                    
                    if (searchByOptions.Any(temp => temp == searchBy) == false)
                    {
                        _logger.LogInformation("searchBy actual value {searchBy}, searchBy");
                        context.ActionArguments["searchBy"] = nameof(PersonResponse.PersonName);
                    }
                       

                }
            }
        }
    }
}
