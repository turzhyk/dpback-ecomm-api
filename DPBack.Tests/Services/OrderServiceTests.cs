using System.Text.Json;
using DPBack.Application.Abstractions;
using DPBack.Application.Contracts;
using DPBack.Application.Exceptions;
using DPBack.Application.Mappers;
using DPBack.Application.Services;
using DPBack.Domain.Enums;
using DPBack.Domain.Enums.Products;
using DPBack.Domain.Models;
using DPBack.Domain.Models.Products;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace DPBack.Tests.Services;

public class OrderServiceTests
{
    private readonly Mock<IOrdersRepository> _mockRepository;
    private readonly Mock<IPaymentObjectRepository> _mockPaymentRepo;
    private readonly Mock<ILogger<OrdersService>> _mockLogger;
    private readonly Mock<IPaymentService> _mockPaymentService;
    private readonly Mock<IPriceCalcService> _mockCalculator;
    private readonly IOrdersService _service;
    private readonly Mock<IProductConfigMapperResolver> _mockMapper;
    private readonly Mock<IMemoryCache> _mockCache;

    public OrderServiceTests()
    {
        _mockRepository = new Mock<IOrdersRepository>();
        _mockPaymentRepo = new Mock<IPaymentObjectRepository>();
        _mockLogger = new Mock<ILogger<OrdersService>>();
        _mockCalculator = new Mock<IPriceCalcService>();
        _mockPaymentService = new Mock<IPaymentService>();
        _mockMapper = new Mock<IProductConfigMapperResolver>();
        _mockCache = new Mock<IMemoryCache>();
        _service = new OrdersService(_mockRepository.Object, _mockPaymentRepo.Object, _mockPaymentService.Object,
            _mockCalculator.Object,
            _mockLogger.Object, _mockMapper.Object, _mockCache.Object);
    }

    [Fact]
    public async Task CreateOrder_ShouldReturnResponseDto()
    {
        var existingCustomerId = new Guid("1e7f7772-9c53-4e88-afa0-d785b3db9842");
        var idempotency = Guid.NewGuid();
        var request = new CreateOrderRequest("Test order", new List<OrderItemRequest>()
        {
            new OrderItemRequest(1, OrderItemType.Test,
                new JsonElement(), null)
        }, idempotency, false, existingCustomerId);

        _mockRepository.Setup(x => x.GetDraftByIdempotencyAsync(idempotency, CancellationToken.None))
            .ReturnsAsync(new OrderDraft { CustomerId = Guid.NewGuid(), IdempotencyKey = idempotency });

        _mockPaymentService.Setup(x =>
            x.CreatePaymentAsync(It.IsAny<string>(), It.IsAny<decimal>(), CancellationToken.None)).ReturnsAsync("link");
        _mockCalculator.Setup(x =>
            x.Calculate(It.IsAny<OrderItem>())).Returns(10m);
        _mockRepository.Setup(x => x.GetCustomerByUserIdAsync(It.IsAny<Guid>(), CancellationToken.None))
            .ReturnsAsync(new Customer{Id = existingCustomerId, Name = "1", Phone = "123456789"});
        var result = await _service.CreateAsync(Guid.NewGuid(), UserRole.Manager, request, CancellationToken.None);

        Assert.NotNull(result.PaymentUrl);
        Assert.NotEmpty(result.PaymentUrl);
        Assert.NotEqual(Guid.Empty, result.OrderId);
    }

    [Fact]
    public async Task CreateOrder_ShouldThrow_WhenItemQuantityIsZero()
    {
        var existingCustomerId = new Guid("1e7f7772-9c53-4e88-afa0-d785b3db9842");
        _mockRepository.Setup(x => x.GetCustomerByUserIdAsync(It.IsAny<Guid>(), CancellationToken.None))
            .ReturnsAsync(new Customer{Id = existingCustomerId, Name = "1", Phone = "123456789"});
        var idempotency = Guid.NewGuid();
        _mockRepository.Setup(x => x.GetDraftByIdempotencyAsync(idempotency, CancellationToken.None))
            .ReturnsAsync(new OrderDraft { CustomerId = existingCustomerId, IdempotencyKey = idempotency });
        var request = new CreateOrderRequest("Test order", new List<OrderItemRequest>()
        {
            new OrderItemRequest(0, OrderItemType.Test,
                new JsonElement(), null)
        }, idempotency, true, existingCustomerId);

        await Assert.ThrowsAsync<ArgumentException>(() =>
            _service.CreateAsync(Guid.NewGuid(), UserRole.Manager, request, CancellationToken.None));
    }

    [Fact]
    public async Task CreateOrder_ShouldReturn_CustomerDoesntExists()
    {
        var idempotency = Guid.NewGuid();
        var request = new CreateOrderRequest("Test order", new List<OrderItemRequest>()
        {
            new OrderItemRequest(1, OrderItemType.Test,
                new JsonElement(), null)
        }, idempotency, false, Guid.NewGuid());

        _mockCalculator.Setup(x =>
            x.Calculate(It.IsAny<OrderItemRequest>())).Returns(10m);
        _mockRepository.Setup(x => x.CustomerExistsAsync(It.IsAny<Guid>(), CancellationToken.None))
            .ReturnsAsync(false);

        _mockRepository.Setup(x => x.GetDraftByIdempotencyAsync(idempotency, CancellationToken.None))
            .ReturnsAsync(new OrderDraft { CustomerId = Guid.NewGuid(), IdempotencyKey = idempotency });

        await Assert.ThrowsAsync<CustomerDoesNotExistException>(() =>
            _service.CreateAsync(Guid.NewGuid(), UserRole.Manager, request, CancellationToken.None));
    }
}