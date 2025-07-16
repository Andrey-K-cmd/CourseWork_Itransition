using System.ComponentModel.DataAnnotations;

namespace Application.Models.Intagration
{
    public class SalesforceUser
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string Phone { get; set; }
        public required string City { get; set; }
        public required string Job { get; set; }
        public required string BirthYear {  get; set; }
        public required string CompanyName { get; set; }
    }
}
