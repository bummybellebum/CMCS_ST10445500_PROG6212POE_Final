// Services/ReportGenerator.cs
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using CMCS_ST10445500_PROG6212POE_Final.Models;

namespace CMCS_ST10445500_PROG6212POE_Final.Services
{
    public static class ReportGenerator
    {
        public static IDocument GenerateApprovedClaimsReport(List<Claim> claims)
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);

                    // === HEADER ===
                    page.Header()
                        .AlignCenter()
                        .Text("CMCS APPROVED CLAIMS REPORT")
                        .FontSize(24)
                        .Bold()
                        .FontColor("#005EB8");

                    // === MAIN CONTENT - TABLE ===
                    page.Content()
                        .PaddingVertical(20)
                        .Table(table =>
                        {
                            // Define columns
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(3); // Lecturer name
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            // Table Header (Blue background)
                            table.Header(header =>
                            {
                                header.Cell().Background("#005EB8").Padding(8).Text("Lecturer").FontColor(Colors.White).Bold();
                                header.Cell().Background("#005EB8").Padding(8).Text("Hours").FontColor(Colors.White).Bold();
                                header.Cell().Background("#005EB8").Padding(8).Text("Rate").FontColor(Colors.White).Bold();
                                header.Cell().Background("#005EB8").Padding(8).Text("Total").FontColor(Colors.White).Bold();
                                header.Cell().Background("#005EB8").Padding(8).Text("Submitted").FontColor(Colors.White).Bold();
                                header.Cell().Background("#005EB8").Padding(8).Text("Status").FontColor(Colors.White).Bold();
                            });

                            // Table Rows
                            foreach (var c in claims)
                            {
                                var total = c.HoursWorked * c.HourlyRate;

                                table.Cell().Padding(6).Text(c.Lecturer?.Name ?? "Unknown Lecturer");
                                table.Cell().Padding(6).Text(c.HoursWorked.ToString("F2"));
                                table.Cell().Padding(6).Text($"R {c.HourlyRate:F2}");
                                table.Cell().Padding(6).Text($"R {total:F2}").FontColor(Colors.Green.Darken2);
                                table.Cell().Padding(6).Text(c.SubmittedDate.ToString("yyyy-MM-dd"));
                                table.Cell().Padding(6)
                                     .Text(c.Status.ToString())
                                     .FontColor(c.Status == ClaimStatus.Approved ? Colors.Green.Darken2 : Colors.Red.Darken2);
                            }
                        });

                    // === FOOTER ===
                    page.Footer()
                        .AlignCenter()
                        .Text(text =>
                        {
                            text.Span("Generated on: ");
                            text.Span(DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                            text.Span(" | Page ");
                            text.CurrentPageNumber();
                        });
                });
            });
        }
    }
}