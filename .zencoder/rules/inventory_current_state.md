---
description: Current Inventory System State and Workflow
alwaysApply: true
---

# Current Inventory System State and Workflow

## Overview
The system has two inventory-related components: a financial asset management system (MainAccount, ExpenseTracking, etc.) for monetary tracking, and a basic physical inventory system (Item, ItemCategory, Stock, StockMovement) for tracking physical items. However, the physical inventory system is limited in functionality and integration.

## Current Database Structure

### MainAccount Table
- **Purpose**: User financial accounts with balances
- **Key Fields**: BalanceAmount, OwnerUserId, CurrencyTypeId
- **Usage**: Stores account balances for clinic staff/users

### ExpenseTracking Table
- **Purpose**: Recording financial expenses
- **Key Fields**: Amount, ExpenseTypeId, InvoiceNumber, AttachmentPath
- **Usage**: Tracks money spent on various expense categories

### WithdrawalTracking Table
- **Purpose**: Deposits and withdrawals from accounts
- **Key Fields**: WithdrawalAmount, DepositAmount, MainAccountId
- **Usage**: Records cash movements in/out of accounts

### TradeTracking Table
- **Purpose**: Trading activities (if applicable)
- **Key Fields**: TradeAmount, ProfitAmount, LossAmount
- **Usage**: Tracks investment/trading performance

### AccountTracking Table
- **Purpose**: Transaction audit trail
- **Key Fields**: DebitAmount, trackType, transactionStatus
- **Usage**: Logs all financial transactions with approval workflow

### Physical Inventory Tables

#### Item Table
- **Purpose**: Item master data
- **Key Fields**: ItemId, Name, Description, BaseUnit, CurrentStock, ReorderLevel
- **Usage**: Defines inventory items with basic stock levels

#### ItemCategory Table
- **Purpose**: Item categorization
- **Key Fields**: categoryId, Name, Description
- **Usage**: Groups items into categories

#### Stock Table
- **Purpose**: Detailed stock tracking per batch/purchase
- **Key Fields**: stockId, quantity, itemId, purchasePrice, batchNumber, expiryDate
- **Usage**: Tracks individual stock entries with pricing and expiration

#### StockMovement Table
- **Purpose**: Stock movement history
- **Key Fields**: StockMovementId, Date, MovementType, Quantity, ItemId
- **Usage**: Records stock in/out movements with reasons

## Current Workflow

### 1. Account Setup
- Administrator creates MainAccount for each user/staff
- Sets initial balance and currency
- Assigns to specific branch

### 2. Expense Recording
- User selects expense type and account
- Uploads invoice attachment
- System validates account balance
- Updates account balance automatically
- Stores expense record with audit trail

### 3. Transfer Process
- User initiates transfer between accounts
- Creates pending transaction in AccountTracking
- Requires approval from account owner
- Upon approval, updates balances
- Records transaction history

### 4. Withdrawal/Deposit
- User records cash movements
- Validates account balance for withdrawals
- Updates account balance
- Stores transaction record

### 5. Reporting
- Dashboard shows real-time balances
- Charts for expense analytics
- Branch-wise asset overview
- Transaction history

## Key Characteristics

### Strengths
- **Financial Tracking**: Comprehensive monetary transaction tracking
- **Approval Workflows**: Transfer approvals prevent unauthorized spending
- **Audit Trail**: Complete transaction history with user tracking
- **Multi-Currency**: Supports different currencies
- **Attachment Support**: Invoice uploads for expense verification
- **Branch-Level**: Segregated by clinic branches

### Financial System Limitations
- **Account-Centric**: Focused on money management, not asset lifecycle
- **Manual Balance Updates**: Balances updated transactionally but no reconciliation
- **Limited Reporting**: Basic charts, no advanced analytics

### Physical Inventory System Limitations
- **Basic Functionality**: Has item master, categories, stock tracking, and movements but lacks advanced features
- **No Expiration Alerts**: Tracks expiry dates but no automated alerts for expiring items
- **Limited Supplier Integration**: Stock table links to suppliers but no comprehensive supplier management for inventory
- **No Barcode Integration**: Has barcode field but no scanning or automated processing
- **No Valuation**: No inventory valuation or cost accounting integration
- **Manual Stock Updates**: Stock movements exist but may require manual entry
- **No Low-Stock Alerts**: ReorderLevel exists but no automated notifications

## Current Usage in Clinic Context

### For Clinic Operations
- **Staff Advances**: Tracks money given to staff for expenses
- **Petty Cash**: Manages clinic petty cash accounts
- **Expense Management**: Records clinic operational expenses
- **Financial Transfers**: Handles money movements between departments

### Gaps for Clinic Needs
- **Medication Inventory**: No tracking of drug stock levels
- **Medical Supplies**: No management of bandages, syringes, etc.
- **Equipment Tracking**: No asset register for medical devices
- **Supplier Management**: No vendor database or purchase orders
- **Stock Alerts**: No notifications for low inventory
- **Expiration Tracking**: No expiry date monitoring for medications

## Conclusion
The current system is a financial asset management platform suitable for monetary tracking but inadequate for physical inventory management required in a healthcare setting. It provides good financial controls but lacks the operational inventory features needed for efficient clinic supply chain management.</content>
</xai:function_call"> 

<xai:function_call name="Write">
<parameter name="file_path">d:\Projects\CrystalClinic\Crystal_Clinic-Service-MGM-System-master\.zencoder\rules\inventory_enhancement_plan.md