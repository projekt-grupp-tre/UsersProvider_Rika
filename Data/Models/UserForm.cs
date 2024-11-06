

namespace Data.Models;

public class UserForm
{
    public string Id { get; set; } = null!;

    
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string PostalCode { get; set; } = null!;
    public string City { get; set; } = null!;
    public string Country { get; set; } = null!;

    

    public int? Age { get; set; }
    public string? GenderId { get; set; } = null!;
    public string? LanguageId { get; set; } = null!; //behöver inte -cookies
    public string? ImageUrl { get; set; }

}
