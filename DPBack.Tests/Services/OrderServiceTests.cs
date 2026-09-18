using System.Text.Json;
using DPBack.Application.Abstractions;
using DPBack.Application.Contracts;
using DPBack.Application.Exceptions;
using DPBack.Application.Mappers;
using DPBack.Application.Services;
using DPBack.Domain.Enums;
using DPBack.Domain.Enums.Products;
using DPBack.Domain.Models.Products;
using Microsoft.Extensions.Caching.Memory;
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
    private readonly Mock<IProductConfigMapperResolver> _mockMapper;
    private readonly Mock<IMemoryCache> _mockCache;

    public OrderServiceTests()
    {
        _mockRepository = new Mock<IOrdersRepository>();
        _mockLogger = new Mock<ILogger<OrdersService>>();
        _mockCalculator = new Mock<IPriceCalcService>();
        _mockPaymentService = new Mock<IPaymentService>();
        _mockMapper = new Mock<IProductConfigMapperResolver>();
        _mockCache = new Mock<IMemoryCache>();
        _service = new OrdersService(_mockRepository.Object, _mockPaymentService.Object, _mockCalculator.Object,
            _mockLogger.Object, _mockMapper.Object, _mockCache.Object);
    }

    [Fact]
    public async Task CreateOrder_ShouldReturnResponseDto()
    {
        var existingCustomerId = new Guid("1e7f7772-9c53-4e88-afa0-d785b3db9842");
        var orderDto = new CreateOrderRequest("Test order", Guid.NewGuid(), new List<OrderItemRequest>()
        {
            new OrderItemRequest(1, OrderItemType.Test,
                new JsonElement(), null)
        }, false, existingCustomerId);

        _mockPaymentService.Setup(x =>
            x.CreatePayment(It.IsAny<string>(), It.IsAny<decimal>())).ReturnsAsync("link");
        _mockCalculator.Setup(x =>
            x.Calculate(It.IsAny<OrderItemRequest>())).Returns(10m);
        _mockRepository.Setup(x => x.CustomerExistsAsync(existingCustomerId, CancellationToken.None))
            .ReturnsAsync(true);
        var result = await _service.CreateAsync(Guid.NewGuid(), orderDto, CancellationToken.None);

        Assert.NotEmpty(result.PaymentUrl);
        Assert.NotEqual(Guid.Empty, result.OrderId);
    }

    [Fact]
    public async Task CreateOrder_ShouldReturn_CustomerDoesntExists()
    {
        var orderDto = new CreateOrderRequest("Test order", Guid.NewGuid(), new List<OrderItemRequest>()
        {
            new OrderItemRequest(1, OrderItemType.Test,
                new JsonElement(), null)
        }, false, Guid.NewGuid());

        _mockCalculator.Setup(x =>
            x.Calculate(It.IsAny<OrderItemRequest>())).Returns(10m);
        _mockRepository.Setup(x => x.CustomerExistsAsync(It.IsAny<Guid>(), CancellationToken.None))
            .ReturnsAsync(false);
        await Assert.ThrowsAsync<CustomerDoesNotExistException>(() =>
            _service.CreateAsync(Guid.NewGuid(), orderDto, CancellationToken.None));
    }
}