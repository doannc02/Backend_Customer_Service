namespace IChiba.Customer.Domain.Entities;

public class CustomerEntity 
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool? isDelete { get; set; } = false;
    public DateTime? UpdatedAt { get; set; }
    public virtual ICollection<CustomerAddress> Addresses { get; set; }
}

