namespace IChiba.Customer.Domain.Entities;

public class CustomerAddress
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid CustomerId { get; set; }
    public string City { get; set; } = string.Empty;
    public string District { get; set; } = string.Empty;
    public string Ward { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public virtual CustomerEntity? Customer { get; set; }
    public DateTime CreateAt { get; set; } = DateTime.UtcNow;
    public Guid? CreateBy { get; set; }
    public DateTime? UpdateAt { get; set; }
    public Guid? UpdateBy { get; set; }
    public DateTime? DeleteAt { get; set; }
    public Guid? DeleteBy { get; set; }
    public bool IsDefaultAddress { get; set; } = false;
    public string? Country { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
}

