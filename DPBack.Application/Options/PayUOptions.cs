using System.ComponentModel.DataAnnotations;

namespace DPBack.Application.Options;

public class PayUOptions
{
    public required string Secret { get; set; }
    public required string SecondKey { get; set; }
    public required string ClientId { get; set; }
    public required string NotifyUrl { get; set; }
}