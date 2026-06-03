using Crystal_Clinic_Mgm.Application.Accounting.Dashboards;
using Crystal_Clinic_Mgm.Domain;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Clinic_Mgm.Application.BranchStock.Dashboards
{
    #region Bar Chart: Stock Movement In vs Out

    public class StockMovementBarChartQuery : IRequest<JsonResult>
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? BranchId { get; set; }
        public int? CategoryId { get; set; }
    }

    public class StockMovementBarChartHandler(ERP_DbContext context)
        : IRequestHandler<StockMovementBarChartQuery, JsonResult>
    {
        public async Task<JsonResult> Handle(StockMovementBarChartQuery request, CancellationToken cancellationToken)
        {
            var today = DateTime.Today;
            var daysInMonth = DateTime.DaysInMonth(today.Year, today.Month);
            var startOfMonth = new DateTime(today.Year, today.Month, 1);
            var endOfMonth = new DateTime(today.Year, today.Month, daysInMonth);
            var startOfYear = new DateTime(today.Year, 1, 1);
            var endOfYear = new DateTime(today.Year, 12, 31);

            var movementsQuery = context.StockMovements
                .Include(m => m.Item)
                .AsQueryable();

            if (request.CategoryId.HasValue)
                movementsQuery = movementsQuery.Where(m => m.Item != null && m.Item.CategoryId == request.CategoryId);

            if (request.StartDate.HasValue)
                movementsQuery = movementsQuery.Where(m => m.Date >= request.StartDate.Value);

            if (request.EndDate.HasValue)
                movementsQuery = movementsQuery.Where(m => m.Date <= request.EndDate.Value);

            var dailyIn = await movementsQuery
                .Where(m => m.MovementType == MovementType.In && m.Date >= startOfMonth && m.Date <= endOfMonth)
                .GroupBy(m => m.Date.Day)
                .Select(g => new { Day = g.Key, Count = g.Sum(m => m.Quantity), Value = g.Sum(m => m.TotalCost) })
                .ToListAsync(cancellationToken);

            var dailyOut = await movementsQuery
                .Where(m => m.MovementType == MovementType.Out && m.Date >= startOfMonth && m.Date <= endOfMonth)
                .GroupBy(m => m.Date.Day)
                .Select(g => new { Day = g.Key, Count = g.Sum(m => m.Quantity), Value = g.Sum(m => m.TotalCost) })
                .ToListAsync(cancellationToken);

            var weeklyIn = await movementsQuery
                .Where(m => m.MovementType == MovementType.In && m.Date >= startOfYear && m.Date <= endOfYear)
                .GroupBy(m => (m.Date.DayOfYear - 1) / 7 + 1)
                .Select(g => new { Week = g.Key, Count = g.Sum(m => m.Quantity), Value = g.Sum(m => m.TotalCost) })
                .ToListAsync(cancellationToken);

            var weeklyOut = await movementsQuery
                .Where(m => m.MovementType == MovementType.Out && m.Date >= startOfYear && m.Date <= endOfYear)
                .GroupBy(m => (m.Date.DayOfYear - 1) / 7 + 1)
                .Select(g => new { Week = g.Key, Count = g.Sum(m => m.Quantity), Value = g.Sum(m => m.TotalCost) })
                .ToListAsync(cancellationToken);

            var monthlyIn = await movementsQuery
                .Where(m => m.MovementType == MovementType.In && m.Date >= startOfYear && m.Date <= endOfYear)
                .GroupBy(m => m.Date.Month)
                .Select(g => new { Month = g.Key, Count = g.Sum(m => m.Quantity), Value = g.Sum(m => m.TotalCost) })
                .ToListAsync(cancellationToken);

            var monthlyOut = await movementsQuery
                .Where(m => m.MovementType == MovementType.Out && m.Date >= startOfYear && m.Date <= endOfYear)
                .GroupBy(m => m.Date.Month)
                .Select(g => new { Month = g.Key, Count = g.Sum(m => m.Quantity), Value = g.Sum(m => m.TotalCost) })
                .ToListAsync(cancellationToken);

            var yearlyIn = await movementsQuery
                .Where(m => m.MovementType == MovementType.In)
                .GroupBy(m => m.Date.Year)
                .Select(g => new { Year = g.Key, Count = g.Sum(m => m.Quantity), Value = g.Sum(m => m.TotalCost) })
                .OrderBy(g => g.Year)
                .ToListAsync(cancellationToken);

            var yearlyOut = await movementsQuery
                .Where(m => m.MovementType == MovementType.Out)
                .GroupBy(m => m.Date.Year)
                .Select(g => new { Year = g.Key, Count = g.Sum(m => m.Quantity), Value = g.Sum(m => m.TotalCost) })
                .OrderBy(g => g.Year)
                .ToListAsync(cancellationToken);

            var allYears = yearlyIn.Select(y => y.Year)
                .Union(yearlyOut.Select(y => y.Year))
                .OrderBy(y => y).ToArray();

            var labels = new TimeSeriesLabelsDto
            {
                Day = Enumerable.Range(1, daysInMonth).Select(d => new DateTime(today.Year, today.Month, d).ToString("dd MMM")).ToArray(),
                Week = Enumerable.Range(1, 52).Select(w => $"Week {w}").ToArray(),
                Month = Enumerable.Range(1, 12).Select(m => new DateTime(today.Year, m, 1).ToString("MMM")).ToArray(),
                Year = allYears.Select(y => y.ToString()).ToArray()
            };

            var groups = new List<GroupedSeriesDto>
            {
                new()
                {
                    GroupName = "Day",
                    Series =
                    [
                        new ChartSeriesDto { Name = "Stock In (Qty)", Data = Enumerable.Range(1, daysInMonth).Select(d => dailyIn.FirstOrDefault(x => x.Day == d)?.Count ?? 0).ToArray() },
                        new ChartSeriesDto { Name = "Stock Out (Qty)", Data = Enumerable.Range(1, daysInMonth).Select(d => dailyOut.FirstOrDefault(x => x.Day == d)?.Count ?? 0).ToArray() },
                        new ChartSeriesDto { Name = "Stock In (Value)", Data = Enumerable.Range(1, daysInMonth).Select(d => dailyIn.FirstOrDefault(x => x.Day == d)?.Value ?? 0).ToArray() },
                        new ChartSeriesDto { Name = "Stock Out (Value)", Data = Enumerable.Range(1, daysInMonth).Select(d => dailyOut.FirstOrDefault(x => x.Day == d)?.Value ?? 0).ToArray() }
                    ]
                },
                new()
                {
                    GroupName = "Week",
                    Series =
                    [
                        new ChartSeriesDto { Name = "Stock In (Qty)", Data = Enumerable.Range(1, 52).Select(w => weeklyIn.FirstOrDefault(x => x.Week == w)?.Count ?? 0).ToArray() },
                        new ChartSeriesDto { Name = "Stock Out (Qty)", Data = Enumerable.Range(1, 52).Select(w => weeklyOut.FirstOrDefault(x => x.Week == w)?.Count ?? 0).ToArray() },
                        new ChartSeriesDto { Name = "Stock In (Value)", Data = Enumerable.Range(1, 52).Select(w => weeklyIn.FirstOrDefault(x => x.Week == w)?.Value ?? 0).ToArray() },
                        new ChartSeriesDto { Name = "Stock Out (Value)", Data = Enumerable.Range(1, 52).Select(w => weeklyOut.FirstOrDefault(x => x.Week == w)?.Value ?? 0).ToArray() }
                    ]
                },
                new()
                {
                    GroupName = "Month",
                    Series =
                    [
                        new ChartSeriesDto { Name = "Stock In (Qty)", Data = Enumerable.Range(1, 12).Select(m => monthlyIn.FirstOrDefault(x => x.Month == m)?.Count ?? 0).ToArray() },
                        new ChartSeriesDto { Name = "Stock Out (Qty)", Data = Enumerable.Range(1, 12).Select(m => monthlyOut.FirstOrDefault(x => x.Month == m)?.Count ?? 0).ToArray() },
                        new ChartSeriesDto { Name = "Stock In (Value)", Data = Enumerable.Range(1, 12).Select(m => monthlyIn.FirstOrDefault(x => x.Month == m)?.Value ?? 0).ToArray() },
                        new ChartSeriesDto { Name = "Stock Out (Value)", Data = Enumerable.Range(1, 12).Select(m => monthlyOut.FirstOrDefault(x => x.Month == m)?.Value ?? 0).ToArray() }
                    ]
                },
                new()
                {
                    GroupName = "Year",
                    Series =
                    [
                        new ChartSeriesDto { Name = "Stock In (Qty)", Data = allYears.Select(y => yearlyIn.FirstOrDefault(x => x.Year == y)?.Count ?? 0).ToArray() },
                        new ChartSeriesDto { Name = "Stock Out (Qty)", Data = allYears.Select(y => yearlyOut.FirstOrDefault(x => x.Year == y)?.Count ?? 0).ToArray() },
                        new ChartSeriesDto { Name = "Stock In (Value)", Data = allYears.Select(y => yearlyIn.FirstOrDefault(x => x.Year == y)?.Value ?? 0).ToArray() },
                        new ChartSeriesDto { Name = "Stock Out (Value)", Data = allYears.Select(y => yearlyOut.FirstOrDefault(x => x.Year == y)?.Value ?? 0).ToArray() }
                    ]
                }
            };

            return new JsonResult(new MultiSeriesChartDto { Labels = labels, Groups = groups });
        }
    }

    #endregion

    #region Line Chart: Stock Value Trend

    public class StockValueTrendLineChartQuery : IRequest<JsonResult>
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? BranchId { get; set; }
    }

    public class StockValueTrendLineChartHandler(ERP_DbContext context)
        : IRequestHandler<StockValueTrendLineChartQuery, JsonResult>
    {
        public async Task<JsonResult> Handle(StockValueTrendLineChartQuery request, CancellationToken cancellationToken)
        {
            var today = DateTime.Today;
            var daysInMonth = DateTime.DaysInMonth(today.Year, today.Month);
            var startOfMonth = new DateTime(today.Year, today.Month, 1);
            var endOfMonth = new DateTime(today.Year, today.Month, daysInMonth);
            var startOfYear = new DateTime(today.Year, 1, 1);
            var endOfYear = new DateTime(today.Year, 12, 31);

            var stockQuery = context.Stocks.AsQueryable();

            if (request.BranchId.HasValue)
                stockQuery = stockQuery.Where(s => s.BranchId == request.BranchId);

            if (request.StartDate.HasValue)
                stockQuery = stockQuery.Where(s => s.PurchaseDate >= request.StartDate.Value);

            if (request.EndDate.HasValue)
                stockQuery = stockQuery.Where(s => s.PurchaseDate <= request.EndDate.Value);

            var dailyValue = await stockQuery
                .Where(s => s.PurchaseDate >= startOfMonth && s.PurchaseDate <= endOfMonth)
                .GroupBy(s => s.PurchaseDate.Day)
                .Select(g => new { Day = g.Key, PurchaseValue = g.Sum(s => s.Quantity * s.PurchasePrice), SellValue = g.Sum(s => s.Quantity * s.SellPrice) })
                .ToListAsync(cancellationToken);

            var weeklyValue = await stockQuery
                .Where(s => s.PurchaseDate >= startOfYear && s.PurchaseDate <= endOfYear)
                .GroupBy(s => (s.PurchaseDate.DayOfYear - 1) / 7 + 1)
                .Select(g => new { Week = g.Key, PurchaseValue = g.Sum(s => s.Quantity * s.PurchasePrice), SellValue = g.Sum(s => s.Quantity * s.SellPrice) })
                .ToListAsync(cancellationToken);

            var monthlyValue = await stockQuery
                .Where(s => s.PurchaseDate >= startOfYear && s.PurchaseDate <= endOfYear)
                .GroupBy(s => s.PurchaseDate.Month)
                .Select(g => new { Month = g.Key, PurchaseValue = g.Sum(s => s.Quantity * s.PurchasePrice), SellValue = g.Sum(s => s.Quantity * s.SellPrice) })
                .ToListAsync(cancellationToken);

            var yearlyValue = await stockQuery
                .GroupBy(s => s.PurchaseDate.Year)
                .Select(g => new { Year = g.Key, PurchaseValue = g.Sum(s => s.Quantity * s.PurchasePrice), SellValue = g.Sum(s => s.Quantity * s.SellPrice) })
                .OrderBy(g => g.Year)
                .ToListAsync(cancellationToken);

            var allYears = yearlyValue.Select(y => y.Year).OrderBy(y => y).ToArray();

            var labels = new TimeSeriesLabelsDto
            {
                Day = Enumerable.Range(1, daysInMonth).Select(d => new DateTime(today.Year, today.Month, d).ToString("dd MMM")).ToArray(),
                Week = Enumerable.Range(1, 52).Select(w => $"Week {w}").ToArray(),
                Month = Enumerable.Range(1, 12).Select(m => new DateTime(today.Year, m, 1).ToString("MMM")).ToArray(),
                Year = allYears.Select(y => y.ToString()).ToArray()
            };

            var groups = new List<GroupedSeriesDto>
            {
                new()
                {
                    GroupName = "Day",
                    Series =
                    [
                        new ChartSeriesDto { Name = "Purchase Value", Data = Enumerable.Range(1, daysInMonth).Select(d => dailyValue.FirstOrDefault(x => x.Day == d)?.PurchaseValue ?? 0).ToArray() },
                        new ChartSeriesDto { Name = "Sell Value", Data = Enumerable.Range(1, daysInMonth).Select(d => dailyValue.FirstOrDefault(x => x.Day == d)?.SellValue ?? 0).ToArray() }
                    ]
                },
                new()
                {
                    GroupName = "Week",
                    Series =
                    [
                        new ChartSeriesDto { Name = "Purchase Value", Data = Enumerable.Range(1, 52).Select(w => weeklyValue.FirstOrDefault(x => x.Week == w)?.PurchaseValue ?? 0).ToArray() },
                        new ChartSeriesDto { Name = "Sell Value", Data = Enumerable.Range(1, 52).Select(w => weeklyValue.FirstOrDefault(x => x.Week == w)?.SellValue ?? 0).ToArray() }
                    ]
                },
                new()
                {
                    GroupName = "Month",
                    Series =
                    [
                        new ChartSeriesDto { Name = "Purchase Value", Data = Enumerable.Range(1, 12).Select(m => monthlyValue.FirstOrDefault(x => x.Month == m)?.PurchaseValue ?? 0).ToArray() },
                        new ChartSeriesDto { Name = "Sell Value", Data = Enumerable.Range(1, 12).Select(m => monthlyValue.FirstOrDefault(x => x.Month == m)?.SellValue ?? 0).ToArray() }
                    ]
                },
                new()
                {
                    GroupName = "Year",
                    Series =
                    [
                        new ChartSeriesDto { Name = "Purchase Value", Data = allYears.Select(y => yearlyValue.FirstOrDefault(x => x.Year == y)?.PurchaseValue ?? 0).ToArray() },
                        new ChartSeriesDto { Name = "Sell Value", Data = allYears.Select(y => yearlyValue.FirstOrDefault(x => x.Year == y)?.SellValue ?? 0).ToArray() }
                    ]
                }
            };

            return new JsonResult(new MultiSeriesChartDto { Labels = labels, Groups = groups });
        }
    }

    #endregion

    #region Pie Chart: Stock Distribution by Category

    public class StockByCategoryPieChartQuery : IRequest<JsonResult>
    {
        public int? BranchId { get; set; }
        public bool IncludeExpired { get; set; } = false;
    }

    public class StockByCategoryPieChartHandler(ERP_DbContext context)
        : IRequestHandler<StockByCategoryPieChartQuery, JsonResult>
    {
        public async Task<JsonResult> Handle(StockByCategoryPieChartQuery request, CancellationToken cancellationToken)
        {
            var stockQuery = context.Stocks
                .Include(s => s.Item)
                .ThenInclude(i => i!.Category)
                .AsQueryable();

            if (request.BranchId.HasValue)
                stockQuery = stockQuery.Where(s => s.BranchId == request.BranchId);

            if (!request.IncludeExpired)
                stockQuery = stockQuery.Where(s => !s.IsExpired);

            var byCategory = await stockQuery
                .GroupBy(s => s.Item != null && s.Item.Category != null ? s.Item.Category.Name : "Uncategorized")
                .Select(g => new PieSliceDto
                {
                    Label = g.Key,
                    Value = g.Sum(s => s.QuantityRemaining)
                })
                .OrderByDescending(s => s.Value)
                .ToListAsync(cancellationToken);

            var byValue = await stockQuery
                .GroupBy(s => s.Item != null && s.Item.Category != null ? s.Item.Category.Name : "Uncategorized")
                .Select(g => new PieSliceDto
                {
                    Label = g.Key,
                    Value = g.Sum(s => s.QuantityRemaining * s.PurchasePrice)
                })
                .OrderByDescending(s => s.Value)
                .ToListAsync(cancellationToken);

            var expirySummary = await context.Stocks
                .Where(s => (request.BranchId == null || s.BranchId == request.BranchId))
                .GroupBy(s => s.IsExpired ? "Expired" : s.ExpiryDate <= DateTime.Today.AddDays(30) ? "Expiring Soon" : "Active")
                .Select(g => new PieSliceDto
                {
                    Label = g.Key,
                    Value = g.Sum(s => s.QuantityRemaining)
                })
                .ToListAsync(cancellationToken);

            return new JsonResult(new
            {
                ByCategory = byCategory,
                ByValue = byValue,
                ByExpiryStatus = expirySummary,
                TotalQty = byCategory.Sum(c => c.Value),
                TotalValue = byValue.Sum(v => v.Value)
            });
        }
    }

    #endregion

    #region Spider Chart: Supplier Performance

    public class SupplierPerformanceSpiderChartQuery : IRequest<JsonResult>
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? BranchId { get; set; }
    }

    public class SupplierPerformanceSpiderChartHandler(ERP_DbContext context)
        : IRequestHandler<SupplierPerformanceSpiderChartQuery, JsonResult>
    {
        public async Task<JsonResult> Handle(SupplierPerformanceSpiderChartQuery request, CancellationToken cancellationToken)
        {
            var duesQuery = context.SupplierDue
                .Include(d => d.Supplier)
                .Where(d => !d.IsDeleted)
                .AsQueryable();

            if (request.StartDate.HasValue)
                duesQuery = duesQuery.Where(d => d.CreatedOn >= request.StartDate.Value);

            if (request.EndDate.HasValue)
                duesQuery = duesQuery.Where(d => d.CreatedOn <= request.EndDate.Value);

            var supplierDues = await duesQuery
                .GroupBy(d => new { d.SupplierId, SupplierName = d.Supplier != null ? d.Supplier.Name : "Unknown" })
                .Select(g => new
                {
                    g.Key.SupplierId,
                    g.Key.SupplierName,
                    TotalDue = g.Sum(d => d.DueAmount),
                    TotalPaid = g.Sum(d => d.PaidAmount),
                    TotalRemain = g.Sum(d => d.RemainAmount),
                    TotalOrders = g.Count()
                })
                .OrderByDescending(s => s.TotalDue)
                .ToListAsync(cancellationToken);

            var stockPerSupplier = await context.Stocks
                .Include(s => s.Supplier)
                .Where(s => (request.BranchId == null || s.BranchId == request.BranchId))
                .GroupBy(s => new { s.SupplierId, SupplierName = s.Supplier != null ? s.Supplier.Name : "Unknown" })
                .Select(g => new
                {
                    g.Key.SupplierId,
                    g.Key.SupplierName,
                    TotalItems = g.Sum(s => s.Quantity),
                    TotalValue = g.Sum(s => s.Quantity * s.PurchasePrice)
                })
                .ToListAsync(cancellationToken);

            var supplierNames = supplierDues.Select(s => s.SupplierName).ToArray();

            var categories = new[] { "Total Due", "Total Paid", "Remaining", "Orders", "Items Supplied", "Stock Value" };

            var series = new List<ChartSeriesDto>
            {
                new() { Name = "Total Due", Data = supplierDues.Select(s => s.TotalDue).ToArray() },
                new() { Name = "Total Paid", Data = supplierDues.Select(s => s.TotalPaid).ToArray() },
                new() { Name = "Remaining", Data = supplierDues.Select(s => s.TotalRemain).ToArray() },
                new() { Name = "Orders", Data = supplierDues.Select(s => (decimal)s.TotalOrders).ToArray() },
                new()
                {
                    Name = "Items Supplied",
                    Data = supplierDues.Select(s => (decimal)(stockPerSupplier.FirstOrDefault(sp => sp.SupplierId == s.SupplierId)?.TotalItems ?? 0)).ToArray()
                },
                new()
                {
                    Name = "Stock Value",
                    Data = supplierDues.Select(s => stockPerSupplier.FirstOrDefault(sp => sp.SupplierId == s.SupplierId)?.TotalValue ?? 0).ToArray()
                }
            };

            return new JsonResult(new
            {
                Categories = supplierNames,
                Axes = categories,
                Series = series,
                SupplierDetails = supplierDues
            });
        }
    }

    #endregion
}
