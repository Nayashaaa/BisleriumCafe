using QuestPDF.Fluent;
using System.Globalization;

namespace BisleriumCafe.Models.Services
{
    public static class ReportService
    {
        // Generates a PDF report for daily sales based on the selected date.
        public static void GenerateDailySalesPDF(DateTime selectedDate, string outputPath)
        {
            var transactions = TransactionService.GetTransaction();

            // Filter transactions for the selected date.
            var dailySalesData = transactions
                .Where(x => x.SalesDate.Date == selectedDate.Date)
                .ToList();

            // Generate the daily sales PDF report.
            GenerateDailyPDF(dailySalesData, outputPath, selectedDate, 0);
        }

        // Generates a PDF report for monthly sales based on the selected month and year.
        public static void GenerateMonthlySalesPDF(int selectedMonth, string outputPath, int selectedYear)
        {
            var transactions = TransactionService.GetTransaction();

            // Filter transactions for the selected month and year.
            var monthlySalesData = transactions
                .Where(x => x.SalesDate.Month == selectedMonth && x.SalesDate.Year == selectedYear) 
                .ToList();

            GenerateSalesPDF(monthlySalesData, outputPath, default(DateTime), selectedMonth);
        }

        private static void GenerateDailyPDF(List<BisleriumCafe.Models.Transaction> salesData, string outputPath, DateTime selectedDate, int selectedMonth)
        {
            var appPath = Utils.GetAppDirectoryPath();

            // Create a PDF document using QuestPDF.
            Document.Create(container =>
            {
                container.Page(page =>
                {
                    // Header section with the sales report title.
                    page.Header().AlignCenter().Text(GetSalesReportTitle(selectedDate, selectedMonth)).FontSize(20).Bold();

                    // Content section with the sales data in a table format.
                    page.Content().Padding(10)
                    .MinimalBox().Border(1).Table(table =>
                    {
                        // Table columns definition.
                        table.ColumnsDefinition(column =>
                        {
                            column.RelativeColumn();
                            column.RelativeColumn();
                            column.RelativeColumn();
                            column.RelativeColumn();
                        });
                        table.ExtendLastCellsToTableBottom();

                        // Table header cells.
                        table.Cell().AlignCenter().Padding(5).Text("S. No.").Bold();
                        table.Cell().AlignCenter().Padding(5).Text("Items").Bold();
                        table.Cell().AlignCenter().Padding(5).Text("Member ID").Bold();
                        table.Cell().AlignCenter().Padding(5).Text("Order Total").Bold();

                        // Insert into table with sales data.
                        int sn = 1;
                        float netTotal = 0;
                        foreach (var transaction in salesData)
                        {
                            // Insert into cells with transaction details.
                            table.Cell().Border(1).Padding(5).Text($"     {sn}");

                            string itemNames = string.Join(", ", transaction.Items.Select(item => item.itemName));
                            int itemCount = transaction.Items.Select(item => item.itemName).Count();
                            table.Cell().Border(1).Padding(5).Text(itemNames);
                            table.Cell().Border(1).Padding(5).Text($"        {transaction.MemberId}");
                            table.Cell().Border(1).Padding(5).Text($"    Rs. {transaction.OrderTotal}");
                            netTotal = netTotal + transaction.OrderTotal;
                            sn++;
                        }

                        // Table footer with total sales.
                        table.Cell().Text("");
                        table.Cell().Text("");
                        table.Cell().Text("");
                        table.Cell().Text("");
                        table.Cell().PaddingHorizontal(5).Text("");
                        table.Cell().PaddingHorizontal(5).Text("");
                        table.Cell().AlignRight().PaddingHorizontal(5).Text("Total Sales:").Bold().FontSize(16);
                        table.Cell().PaddingHorizontal(5).Text($"  Rs. {netTotal}").Bold().FontSize(16);
                    });

                    page.Footer().Text(text =>
                    {
                        text.Span("Page :");
                        text.CurrentPageNumber();
                    });
                });
            }).GeneratePdf(Path.Combine(appPath, outputPath));
        }

        // Generates a PDF report for monthly sales.

        private static void GenerateSalesPDF(List<BisleriumCafe.Models.Transaction> salesData, string outputPath, DateTime selectedDate, int selectedMonth)
        {
            var appPath = Utils.GetAppDirectoryPath();

            // Create a PDF document using QuestPDF.
            Document.Create(container =>
            {
                container.Page(page =>
                {
                    // Header section with the sales report title.
                    page.Header().AlignCenter().Text(GetSalesReportTitle(selectedDate, selectedMonth)).FontSize(20).Bold();
                    page.Content().Padding(10)
                    .MinimalBox().Border(1).Table(table =>
                    {
                        table.ColumnsDefinition(column =>
                        {
                            column.RelativeColumn();
                            column.RelativeColumn();
                            column.RelativeColumn();
                            column.RelativeColumn();
                            column.RelativeColumn();
                        });
                        table.ExtendLastCellsToTableBottom();
                        table.Cell().AlignCenter().Padding(5).Text("S. No.").Bold();
                        table.Cell().AlignCenter().Padding(5).Text("Items").Bold();
                        table.Cell().AlignCenter().Padding(5).Text("Member ID").Bold();
                        table.Cell().AlignCenter().Padding(5).Text("Sales Date").Bold();
                        table.Cell().AlignCenter().Padding(5).Text("Order Total").Bold();

                        int sn = 1;
                        float netTotal = 0;
                        foreach (var transaction in salesData)
                        {
                            table.Cell().Border(1).Padding(5).Text($"     {sn}");

                            string itemNames = string.Join(", ", transaction.Items.Select(item => item.itemName));
                            int itemCount = transaction.Items.Select(item => item.itemName).Count();
                            table.Cell().Border(1).Padding(5).Text(itemNames);
                            table.Cell().Border(1).Padding(5).Text($"        {transaction.MemberId}");
                            table.Cell().Border(1).Padding(5).Text(transaction.SalesDate.Date.ToString("d"));
                            table.Cell().Border(1).Padding(5).Text($"    Rs. {transaction.OrderTotal}");
                            netTotal = netTotal + transaction.OrderTotal;
                            sn++;
                        }
                        table.Cell().Text("");
                        table.Cell().Text("");
                        table.Cell().Text("");
                        table.Cell().Text("");
                        table.Cell().Text("");
                        table.Cell().PaddingHorizontal(5).Text("");
                        table.Cell().PaddingHorizontal(5).Text("");
                        table.Cell().PaddingHorizontal(5).Text("");
                        table.Cell().AlignRight().PaddingHorizontal(5).Text("Total Sales:").Bold().FontSize(16);
                        table.Cell().PaddingHorizontal(5).Text($"  Rs. {netTotal}").Bold().FontSize(16);
                    });

                    page.Footer().Text(text =>
                    {
                        text.Span("Page :");
                        text.CurrentPageNumber();
                    });
                });
            }).GeneratePdf(Path.Combine(appPath, outputPath));
        }

        // Generates the title for the sales report based on the selected date or month.
        private static string GetSalesReportTitle(DateTime selectedDate, int selectedMonth)
        {
            if (selectedDate != default(DateTime))
            {
                return $"Sales Report for {selectedDate.Date.ToString("MM/dd/yyyy")}";
            }
            else if (selectedMonth > 0 && selectedMonth <= 12)
            {
                var monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(selectedMonth);
                return $"Sales Report for {monthName}";
            }
            else
            {
                return "Sales Data";
            }
        }
    }
}
