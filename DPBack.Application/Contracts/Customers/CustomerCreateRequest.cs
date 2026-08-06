using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace DPBack.Application.Contracts.Customers;

public record CustomerCreateRequest( string Name, [Phone] string Phone, [EmailAddress] string Email);