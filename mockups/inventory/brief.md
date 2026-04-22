# Design Brief: Crystal Clinic — Inventory Management Module

## Objective
A production-quality, standalone HTML prototype for the **Inventory Management** module of the Crystal Clinic Service Management System — a healthcare clinic. The prototype covers all CRUD tiers in a single-page application with sidebar navigation.

## Target Audience
- Clinic pharmacists and storekeepers managing physical stock
- Clinic administrators reviewing reports and valuations
- Nurses/doctors seeing reservation status

## Aesthetic Direction
**"Clinical Precision"** — Clean, data-dense, professional healthcare administration UI.
- Dark sidebar with a light content area — high contrast, trustworthy
- Accent color: **#0EA5E9 (sky-500/teal)** — clinical, clean
- Secondary: amber/orange for warnings (expiry alerts, low stock)
- Danger: red for critical (out of stock, expired)
- Background: `#F8FAFC`, cards on white
- Fonts: Inter (sans-serif) — clean, modern
- Dense tables with subtle row hover states
- Status badges (pill-shaped, color-coded)

## What Makes It Memorable
**Rich status system**: Every row is alive — colored badges, expiry countdowns, stock-level progress bars, and reservation indicators make data instantly scannable.

## Color Palette
- Sidebar: `#0F172A` (slate-900)
- Sidebar active: `#0EA5E9`
- Sidebar text: `#94A3B8`, active: white
- Content bg: `#F8FAFC`
- Card bg: `#FFFFFF`
- Primary action: `#0EA5E9`
- Success: `#DCFCE7` / `#16A34A`
- Warning: `#FEF3C7` / `#D97706`
- Danger: `#FEE2E2` / `#DC2626`
- Info: `#DBEAFE` / `#2563EB`
- Text primary: `#0F172A`
- Text secondary: `#64748B`
- Border: `#E2E8F0`

## Screens / Navigation

### Sidebar Groups
```
[+ Crystal Clinic logo + text]

INVENTORY
  Dashboard
  Categories
  Items
  Stock / Batches

TRANSACTIONS
  Stock Movements
  Stock Take

KITS & RESERVATIONS
  Kits
  Reservations

REPORTS
  Valuation Report
  Expiring Stock
  Movement History
```

## Screen Designs

### 1. Dashboard (Default)
- **4 stat cards**: Total Items | Total Stock Value | Low Stock Items | Expiring ≤30 Days
- **Stock Level Table**: Top 5 items with inline progress bar (stock vs reorder level)
- **Recent Movements feed**: Last 8 movements (icon, item, qty +/-, timestamp)
- **Alerts panel** (right): Low stock + expiry alerts list

### 2. Categories
CRUD table: ID | Name | Description | Items Count | Status | Actions (Edit/Delete)
Top bar: search + "Add Category" button → opens modal
Sample: Medical Supplies, Medications, Surgical Equipment, Implants, Consumables

### 3. Items
CRUD table: Item Code | Name | Category | Unit | Stock Level (mini progress bar) | Reorder | Unit Cost | Status | Actions
Filter: Category dropdown + search
Stock bar colors: green (ok), yellow (low), red (critical)
"Add Item" → modal form
Sample: Titanium Implant Grade-5 (4521), Antibiotic Capsule (2015), Sterile Gloves (102), Surgical Mask (103), Sterile Field (104)

### 4. Stock / Batches
CRUD table: Stock ID | Item | Lot # | Quantity | Qty Remaining | Purchase Price | Expiry | Days Left | Status | Actions
Days Left badge: green (>90), yellow (30-90), red (<30), gray (expired)
"Receive Stock" button → modal with full form
Sample batches: TI-2025-001 (8 units), TI-2025-002 (7 units), AB-2025-045 (120 units)

### 5. Stock Movements
Table: Movement ID | Date | Type (IN/OUT/ADJUST) | Item | Batch | Qty | Reason | Reference | Cost
Type badges: green(IN), red(OUT), blue(ADJUST)
Filters: date range + type + item search
"Register Movement" → modal

### 6. Stock Take
Top: focused form — Select Batch → shows current system qty → enter actual qty → reason → Submit
Bottom: Recent Stock Takes table (last 10) with difference column (+ green / - red)

### 7. Kits
Left panel (1/3): Kit cards list — Kit name, items count, status
Right panel (2/3): Selected kit detail — line items table (Item + Qty), Edit/Delete actions
"Create Kit" → modal with dynamic line item builder (add/remove rows)
Sample: Dental Surgical Kit (Kit 7)

### 8. Reservations
Table: Reservation ID | Visit ID | Service | Items | Status | Expires At | Actions
Status: Active (green), Expired (gray), Committed (blue), Released (red/muted)
Expandable rows → reserved lots detail
Actions: Commit (primary button) | Release (danger button)
"New Reservation" button

### 9. Valuation Report
Summary cards: Total Inventory Value | This Month COGS | Avg Unit Cost
Table: Item | Category | Total Qty | Unit Cost | Total Value | Method
Export CSV button + as-of date picker

### 10. Expiring Stock
Filter tabs: All | <30 Days | <90 Days | Expired
Alert banner: if expired items exist → red dismissible banner
Table: Item | Lot # | Qty | Expiry Date | Days Left | Branch | Action (Write Off button)
Days Left: strong visual countdown badges

### 11. Movement History
Same as Stock Movements but with advanced date range filters
Summary row at bottom: Total IN | Total OUT | Net Change

## Sample Data
Use realistic data from the Crystal Clinic testing guide:
- Items: Titanium Implant Grade-5, Antibiotic Capsule, Sterile Gloves, Surgical Mask, Sterile Field
- Lots: TI-2025-001, TI-2025-002, AB-2025-045, GL-2025-100, MS-2025-050
- Branch: Crystal Clinic Lahore (Branch 1)
- User: Dr. Sarah

## Typography
- Font: Inter from Google Fonts
- Table headers: 500 weight, 11px uppercase, letter-spacing 0.05em
- Body: 400, 14px
- Sidebar nav: 500, 13px
- Headings: 600

## Layout
- Sidebar: 240px fixed, dark (`#0F172A`)
- Top bar: 60px, white, breadcrumb + user avatar + bell icon
- Content: remaining width, scrollable, padded (24px)
- Mobile: sidebar hidden, hamburger toggle

## Interactions (JavaScript)
- Sidebar nav clicks: show/hide corresponding page sections
- Dashboard is default visible
- Modals: centered overlay + backdrop for Add/Edit forms
- Table row hover highlight
- Active nav item highlighted with teal accent
- Filter tabs work (show/hide rows)
- Kit selection in Kits page updates right panel

## Technical Requirements
- Single `index.html`, fully self-contained
- Use Tailwind CSS CDN (`https://cdn.tailwindcss.com`) for utility classes
- Google Fonts Inter: `https://fonts.googleapis.com/css2?family=Inter:wght@300;400;500;600;700&display=swap`
- All JS inline in `<script>` at bottom
- No external dependencies beyond CDN links
- Must look complete and professional

## Output Path
`C:\Users\pc\Desktop\Crystal_Clinic_mgm-System\mockups\inventory\index.html`

## Image Needs
- Logo: inline SVG cross/plus icon (white, 24x24)
- User avatar: CSS circle with initials "DS" (Dr. Sarah)
- Icons: use Unicode emoji or simple inline SVG — no external icon libraries needed (or use heroicons via CDN if desired)

## Key Constraints
1. All 11 screens in one HTML file — JS toggles visibility
2. Dashboard must feel data-rich immediately on load
3. Expiry warnings must be visually prominent — unmissable
4. Every table must have working Add/Edit modals with realistic form fields
5. The design must feel like a real working application, not a wireframe
