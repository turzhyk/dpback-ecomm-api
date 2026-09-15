namespace DPBack.Application.Options;

public class CompanyOptions
{
    public required string FullName { get; set; } = string.Empty;
    public required string Nip { get; set; } = string.Empty;
    public required string Regon { get; set; } = string.Empty;
    public required string PhoneNumber { get; set; } = string.Empty;
    public required string Email { get; set; } = string.Empty;
}