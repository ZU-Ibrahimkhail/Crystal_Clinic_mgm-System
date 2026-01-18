using Crystal_Clinic_Mgm.Application.Accounting;
using Crystal_Clinic_Mgm.Application.Accounting.Services;
using Crystal_Clinic_Mgm.Domain;
using Crystal_Clinic_Mgm.Application.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Crystal_Clinic_Mgm.Application.BranchStock
{
    public class FinancialBridge :
        INotificationHandler<InventoryStockAdjustedEvent>,
        INotificationHandler<InventoryKitConsumedEvent>,
        INotificationHandler<InventoryStockExpiredEvent>
    {
        private readonly IFinancialService _financialService;
        private readonly ILogger<FinancialBridge> _logger;

        public FinancialBridge(
            IFinancialService financialService,
            ILogger<FinancialBridge> logger)
        {
            _financialService = financialService;
            _logger = logger;
        }

        public async Task Handle(InventoryStockAdjustedEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Processing inventory stock adjustment for item {ItemId}, movement type {MovementType}",
                    notification.ItemId, notification.MovementType);

                // Create journal entry based on movement type
                if (notification.MovementType == MovementType.In)
                {
                    // Debit Inventory, Credit Cash/Bank or Accounts Payable
                    await CreateInventoryIncreaseEntry(notification, cancellationToken);
                }
                else if (notification.MovementType == MovementType.Out)
                {
                    // Debit COGS, Credit Inventory
                    await CreateInventoryDecreaseEntry(notification, cancellationToken);
                }
                else if (notification.MovementType == MovementType.Adjustment)
                {
                    // Handle stock adjustments (could be increase or decrease)
                    await CreateInventoryAdjustmentEntry(notification, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing inventory stock adjustment event for item {ItemId}",
                    notification.ItemId);
                // Don't rethrow - we don't want inventory operations to fail due to financial issues
            }
        }

        public async Task Handle(InventoryKitConsumedEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Processing kit consumption for kit {KitName}, total cost {TotalCost}",
                    notification.KitName, notification.TotalCost);

                // Create journal entry for kit consumption
                // Debit COGS, Credit Inventory (for each consumed item)
                await CreateKitConsumptionEntry(notification, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing kit consumption event for kit {KitId}",
                    notification.KitId);
            }
        }

        public async Task Handle(InventoryStockExpiredEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Processing stock expiry for item {ItemName}, lot {LotNumber}",
                    notification.ItemName, notification.LotNumber);

                // Create journal entry for expired stock
                // Debit Loss on Expired Inventory, Credit Inventory
                await CreateExpiredStockEntry(notification, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing stock expiry event for stock {StockId}",
                    notification.StockId);
            }
        }

        private async Task CreateInventoryIncreaseEntry(InventoryStockAdjustedEvent notification, CancellationToken cancellationToken)
        {
            // For incoming stock: Debit Inventory, Credit Accounts Payable (or Cash if paid)
            // This is a simplified version - in practice, we'd need to determine the source

            var journalEntry = new JournalEntryRequest
            {
                EntryDate = DateTime.UtcNow,
                Description = $"Inventory increase - Item {notification.ItemId}, Ref: {notification.ReferenceId}",
                ReferenceNumber = notification.ReferenceId,
                ReferenceType = "INVENTORY_IN",
                Lines = new List<JournalEntryLineRequest>
                {
                    new JournalEntryLineRequest
                    {
                        AccountCode = "1103", // Inventory account
                        Description = $"Inventory increase - Item {notification.ItemId}",
                        DebitAmount = notification.TotalCost,
                        CreditAmount = 0
                    },
                    new JournalEntryLineRequest
                    {
                        AccountCode = "2101", // Accounts Payable (assuming vendor payment)
                        Description = $"Accounts payable - Item {notification.ItemId}",
                        DebitAmount = 0,
                        CreditAmount = notification.TotalCost
                    }
                }
            };

            await _financialService.CreateJournalEntryAsync(journalEntry, cancellationToken);
        }

        private async Task CreateInventoryDecreaseEntry(InventoryStockAdjustedEvent notification, CancellationToken cancellationToken)
        {
            // For outgoing stock: Debit COGS, Credit Inventory

            var journalEntry = new JournalEntryRequest
            {
                EntryDate = DateTime.UtcNow,
                Description = $"Inventory decrease - Item {notification.ItemId}, Ref: {notification.ReferenceId}",
                ReferenceNumber = notification.ReferenceId,
                ReferenceType = "INVENTORY_OUT",
                Lines = new List<JournalEntryLineRequest>
                {
                    new JournalEntryLineRequest
                    {
                        AccountCode = "5102", // Cost of Goods Sold
                        Description = $"COGS - Item {notification.ItemId}",
                        DebitAmount = notification.TotalCost,
                        CreditAmount = 0
                    },
                    new JournalEntryLineRequest
                    {
                        AccountCode = "1103", // Inventory account
                        Description = $"Inventory decrease - Item {notification.ItemId}",
                        DebitAmount = 0,
                        CreditAmount = notification.TotalCost
                    }
                }
            };

            await _financialService.CreateJournalEntryAsync(journalEntry, cancellationToken);
        }

        private async Task CreateInventoryAdjustmentEntry(InventoryStockAdjustedEvent notification, CancellationToken cancellationToken)
        {
            // For adjustments, determine if it's an increase or decrease
            var isIncrease = notification.Quantity > 0;
            var absQuantity = Math.Abs(notification.Quantity);
            var absCost = Math.Abs(notification.TotalCost);

            var journalEntry = new JournalEntryRequest
            {
                EntryDate = DateTime.UtcNow,
                Description = $"Inventory adjustment - Item {notification.ItemId}, Ref: {notification.ReferenceId}",
                ReferenceNumber = notification.ReferenceId,
                ReferenceType = "INVENTORY_ADJ",
                Lines = new List<JournalEntryLineRequest>
                {
                    new JournalEntryLineRequest
                    {
                        AccountCode = isIncrease ? "1103" : "5102", // Inventory or COGS
                        Description = $"Inventory adjustment - Item {notification.ItemId}",
                        DebitAmount = isIncrease ? absCost : 0,
                        CreditAmount = isIncrease ? 0 : absCost
                    },
                    new JournalEntryLineRequest
                    {
                        AccountCode = isIncrease ? "5102" : "1103", // Opposite side
                        Description = $"Inventory adjustment offset - Item {notification.ItemId}",
                        DebitAmount = isIncrease ? 0 : absCost,
                        CreditAmount = isIncrease ? absCost : 0
                    }
                }
            };

            await _financialService.CreateJournalEntryAsync(journalEntry, cancellationToken);
        }

        private async Task CreateKitConsumptionEntry(InventoryKitConsumedEvent notification, CancellationToken cancellationToken)
        {
            // For kit consumption: Debit COGS, Credit Inventory (aggregated)

            var journalEntry = new JournalEntryRequest
            {
                EntryDate = DateTime.UtcNow,
                Description = $"Kit consumption - {notification.KitName}, Ref: {notification.ReferenceId}",
                ReferenceNumber = notification.ReferenceId,
                ReferenceType = "KIT_CONSUMPTION",
                Lines = new List<JournalEntryLineRequest>
                {
                    new JournalEntryLineRequest
                    {
                        AccountCode = "5102", // Cost of Goods Sold
                        Description = $"COGS - Kit {notification.KitName}",
                        DebitAmount = notification.TotalCost,
                        CreditAmount = 0
                    },
                    new JournalEntryLineRequest
                    {
                        AccountCode = "1103", // Inventory account
                        Description = $"Inventory decrease - Kit {notification.KitName}",
                        DebitAmount = 0,
                        CreditAmount = notification.TotalCost
                    }
                }
            };

            await _financialService.CreateJournalEntryAsync(journalEntry, cancellationToken);
        }

        private async Task CreateExpiredStockEntry(InventoryStockExpiredEvent notification, CancellationToken cancellationToken)
        {
            // For expired stock: Debit Loss on Expired Inventory, Credit Inventory

            var journalEntry = new JournalEntryRequest
            {
                EntryDate = DateTime.UtcNow,
                Description = $"Expired stock - {notification.ItemName}, Lot: {notification.LotNumber}",
                ReferenceNumber = $"EXP-{notification.StockId}",
                ReferenceType = "EXPIRED_STOCK",
                Lines = new List<JournalEntryLineRequest>
                {
                    new JournalEntryLineRequest
                    {
                        AccountCode = "5103", // Financial Expense (or create specific expired inventory loss account)
                        Description = $"Loss on expired inventory - {notification.ItemName}",
                        DebitAmount = notification.TotalValue,
                        CreditAmount = 0
                    },
                    new JournalEntryLineRequest
                    {
                        AccountCode = "1103", // Inventory account
                        Description = $"Expired inventory write-off - {notification.ItemName}",
                        DebitAmount = 0,
                        CreditAmount = notification.TotalValue
                    }
                }
            };

            await _financialService.CreateJournalEntryAsync(journalEntry, cancellationToken);
        }
    }
}