using DPBack.Application.Abstractions;
using DPBack.Domain.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace DPBack.Infrastructure.QuestPdfGenerator;

public class QuestPdfReceiptGenerator:IReceiptGenerator
{
    public byte[] GenerateReceiptPdf(Order order, CancellationToken cToken)
    {
       
        var doc = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(20));
        
                    page.Header()
                        .Text("Receipt")
                        .SemiBold().FontSize(36).FontColor(Colors.Blue.Medium);
        
                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Column(x =>
                        {
                            x.Spacing(20);
                
                            x.Item().Text($"Creation date: {order.CreatedAt}");
                            foreach (var i in order.Items)
                            {
                                x.Item().Row(row =>
                                {
                                    row.ConstantItem(26).Text(i.Type);
                                   
                                    row.RelativeItem().Text(i.Quantity);
                                    row.RelativeItem().Text($"x{i.PricePerUnit}={i.Quantity * i.PricePerUnit}");
                                });
                            }
                        });
                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Page ");
                            x.CurrentPageNumber();
                        });
                });
            });
        return doc.GeneratePdf();
    }
}