using DPBack.Application.Abstractions;
using DPBack.Application.Options;
using DPBack.Domain.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace DPBack.Infrastructure.QuestPdfGenerator;

public class QuestPdfReceiptGenerator:IReceiptGenerator
{
    public byte[] GenerateReceiptPdf(Order order, CompanyOptions companyOptions, CancellationToken cToken)
    {
       
        var doc = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(16));
        
                    page.Header()
                        .Text("Receipt")
                        .SemiBold().FontSize(20).FontColor(Colors.Blue.Medium);
                    
                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Column(x =>
                        {
                            x.Spacing(0);
                            x.Item().Text($"Creation date: {order.CreatedAt}");
                            x.Item().PaddingTop(0.5f ,Unit.Centimetre).Text($"{companyOptions.FullName}");
                            x.Item().Text($"NIP: {companyOptions.Nip}");
                            x.Item().Text($"REGON: {companyOptions.Regon}");
                            x.Item().Text($"Email: {companyOptions.Email}");
                            x.Item().Text($"Tel.: {companyOptions.PhoneNumber}");
                           
                            int rowCount = 0;
                            x.Item().PaddingTop(0.5f ,Unit.Centimetre).Text("Order");
                            foreach (var i in order.Items)
                            {
                                
                                x.Item()
                                    .Background(rowCount % 2 == 0 ? "#F2F2F2" : "#FFFFFF").Padding(3, Unit.Millimetre)
                                    .Row(row =>
                                {
                                    row.ConstantItem(300).Text(i.Type).Bold();
                                   
                                    row.RelativeItem().BorderRight(0.5f, Unit.Millimetre).Text(i.Quantity);
                                    row.RelativeItem().Text($"x{i.PricePerUnit}={i.Quantity * i.PricePerUnit} zł");
                                });
                                rowCount++;
                            }

                            x.Item().AlignRight().Text($"{order.TotalPrice} zł").FontSize(20).Bold();
                        });
                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("receipt ");
                            x.CurrentPageNumber();
                        });
                });
            });
        return doc.GeneratePdf();
    }
}