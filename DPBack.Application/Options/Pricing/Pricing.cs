namespace DPBack.Application.Options.Pricing;

public class Pricing
{
    public BusinesscardPricing Businesscard { get; set; } = new();
    public OpeningHoursStickerPricing OpeningHoursSticker { get; set; } = new();
}