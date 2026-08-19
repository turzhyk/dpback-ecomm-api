using System.Text.Json;
using DPBack.Application.Abstractions;
using DPBack.Application.Contracts;
using DPBack.Application.Mappers;
using DPBack.Application.Services;
using DPBack.Domain.Enums;
using DPBack.Domain.Models.Products;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace DPBack.Tests.Services;

public class OrderServiceTests
{
    private readonly Mock<IOrdersRepository> _mockRepository;
    private readonly Mock<ILogger<OrdersService>> _mockLogger;
    private readonly Mock<IPaymentService> _mockPaymentService;
    private readonly Mock<IPriceCalcService> _mockCalculator;
    private readonly IOrdersService _service;
    private readonly Mock<ProductConfigMapperFactory> _mockMapper;

    public OrderServiceTests()
    {
        _mockRepository = new Mock<IOrdersRepository>();
        _mockLogger = new Mock<ILogger<OrdersService>>();
        _mockCalculator = new Mock<IPriceCalcService>();
        _mockPaymentService = new Mock<IPaymentService>();
        _mockMapper = new Mock<ProductConfigMapperFactory>();
        _service = new OrdersService(_mockRepository.Object, _mockPaymentService.Object, _mockCalculator.Object,
            _mockLogger.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task CreateOrder_ShouldReturnResponseDto()
    {
        var orderDto = new CreateOrderRequestDto("Test order", Guid.NewGuid(), new List<OrderItemRequest>()
        {
            new OrderItemRequest(1, OrderItemType.Businesscard, new JsonElement())
        }, false, Guid.NewGuid());

        _mockPaymentService.Setup(x =>
            x.CreatePayment(It.IsAny<string>(), It.IsAny<decimal>())).ReturnsAsync("link");
        _mockCalculator.Setup(x =>
            x.Calculate(It.IsAny<OrderItemRequest>())).Returns(10m);

        var result = await _service.CreateOrder(Guid.NewGuid(), orderDto, CancellationToken.None);
        
        Assert.NotEmpty(result.PaymentUrl);
        Assert.NotEqual(Guid.Empty,result.OrderId);
    }
}