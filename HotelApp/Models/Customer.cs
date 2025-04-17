public class Customer
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public string Email { get; set; }
    public string City { get; set; }

    
    public string Name
    {
        get => $"{FirstName} {LastName}";
        set
        {
            
            var nameParts = value.Split(' ');
            if (nameParts.Length > 0)
                FirstName = nameParts[0];
            if (nameParts.Length > 1)
                LastName = nameParts[1];
        }
    }
}
