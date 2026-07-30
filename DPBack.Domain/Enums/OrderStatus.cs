namespace DPBack.Domain.Enums;

public enum OrderStatus
{
    New, // заказ создан
    InProgress, // оператор работает
    Produced, // продукция изготовлена
    Packing, // упаковка
    ReadyForShipping, // готов к отправке
    InDelivery, // у курьера
    Done, // доставлено
    Cancelled // отменено
}

public enum OrderPaymentStatus
{
    Waiting,
    Paid,
    Cancelled
}