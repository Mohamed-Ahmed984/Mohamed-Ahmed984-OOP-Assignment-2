# Responsibilities found before refactoring

A class should have one reason to change. Sharing a domain name does not mean that
clinical rules, text, exports, or financial rules should live in the same class.
The original classes were read before the refactoring. The new types below keep
those change reasons separate, without custom interfaces.

## 1. WardBoard

Originally it stored bed assignments, calculated clinical acuity, selected pager
alerts, wrote handoff notes, and exported a census CSV. A clinical scoring change
should not require editing a nurse's document template or an export layout.

- `WardBoard`: stores and updates bed assignments and their recorded acuity.
- `AcuityScorer`: calculates the clinical score from heart rate and oxygen level.
- `WardPager`: records alerts using the pager threshold and drains its own log.
- `WardHandoffNote`: formats a nurse's handoff note.
- `WardCensusCsv`: formats the census export.

Assigning a bed still calculates its score and triggers paging. The board delegates
those calculations; it does not contain the clinical formula or pager threshold.

## 2. CheckoutBasket

Originally it stored basket choices, parsed marketing coupons, calculated totals,
set a wrapping charge, wrote gift messages, and produced a payment authorization.
A payment gateway change should not affect coupon parsing or gift-card wording.

- `CheckoutBasket`: records lines, the chosen coupon, and the wrapping choice.
- `BasketPricing`: calculates the subtotal, wrapping charge, and final payable amount.
- `CouponDiscountPolicy`: interprets coupon text and calculates its discount.
- `GiftCardFormatter`: formats the gift message.
- `PaymentAuthorizer`: produces the original demonstration authorization code.

The authorization remains a stub, not a real payment integration. Its use of
`string.GetHashCode()` is preserved, so codes can differ between processes.

## 3. SupportTicket

Originally it stored ticket messages, classified keywords, calculated SLA deadlines,
and composed public and internal messages. Support keywords, response deadlines,
and customer-facing wording can all change independently.

- `SupportTicket`: maintains ticket data and customer-message updates.
- `TicketPriorityClassifier`: determines priority from message text.
- `TicketSla`: calculates the deadline and determines whether it is breached.
- `TicketMessages`: formats public replies and internal escalation messages.

Priority is still recalculated when a customer message is appended.

## 4. LoanDesk

Originally it held an application, calculated eligibility, selected documents,
wrote a decision letter, and exported an analytics row. Compliance requirements
and letter wording should not be mixed with the risk formula.

- `LoanDesk`: stores loan application data.
- `LoanEligibility`: calculates risk and eligibility.
- `LoanDocumentRequirements`: selects the required documents.
- `LoanDecisionLetter`: formats the decision letter.
- `LoanApplicationCsv`: formats the underwriter CSV row.

## 5. CourseEnrollmentDesk

Originally it assigned seats, kept a waitlist, promoted students, wrote a welcome
packet, and created tuition invoice text. Marketing content and finance output
should not be part of the seating algorithm.

- `CourseEnrollmentDesk`: maintains seats and the waitlist, including promotion.
- `CourseWelcomePacket`: formats welcome Markdown using the enrollment status.
- `TuitionInvoice`: calculates the original VAT line and formats the tuition invoice.

The original case-handling behavior is preserved, including the case-sensitive
waitlist duplicate check and the case-insensitive position search. This exercise
does not silently change the original business behavior.

## 6. KitchenTicket

Originally it held items, recognized allergens, estimated preparation time,
printed a thermal ticket, and selected an expo lane. An allergen dictionary update
should not require editing printer layout or dispatch wording.

- `KitchenTicket`: records ordered items and normalized ingredients.
- `AllergenDetector`: recognizes and lists allergens.
- `KitchenReadyEstimate`: estimates preparation time, including the allergy delay.
- `ThermalTicketPrinter`: formats the printer output.
- `ExpoLanePolicy`: selects the dispatch lane.

## 7. SubscriptionBilling

Originally it stored subscription/payment state, prorated prices, allocated invoice
numbers, wrote collection emails, and exported ledger entries. An email formatting
change could accidentally change invoice numbering because composing mail had a
hidden side effect.

- `SubscriptionBilling`: holds subscription data and counts failed payments.
- `SubscriptionProration`: calculates the prorated amount.
- `InvoiceNumberSequence`: allocates invoice numbers across subscriptions.
- `SubscriptionDocuments`: coordinates allocating an invoice and producing a document.
- `DunningEmailFormatter`: formats collection email text from already supplied values.
- `SubscriptionLedgerCsv`: formats a ledger row from already supplied values.

The coordinator intentionally allocates one invoice per email or ledger request,
just as before. The formatter itself no longer allocates numbers. The static
sequence is needed to preserve the original application-wide invoice sequence.

## 8. WarehousePickList

Originally it stored pick needs, allocated stock, ordered a walking route, wrote
picker instructions, and exported WMS XML. Warehouse layout, shortage policy, and
an integration schema are separate reasons to change.

- `WarehousePickList`: records picking needs and stock information.
- `StockAllocation`: determines the allocated quantity.
- `PickWalkingOrder`: orders pick locations by aisle and bin.
- `PickerInstructions`: formats walking instructions and shortage notices.
- `WmsBatchXml`: formats the WMS batch message.

The route asks `StockAllocation` for quantities. Tied locations keep insertion
order. Repeated SKUs keep the original first-matching-SKU shortage behavior.

## 9. GradeBook

Originally it stored scores, calculated averages, selected letter bands and honors,
and produced transcripts and CSV. A registrar's output format should not be mixed
with academic grading policy.

- `GradeBook`: records and aggregates student scores.
- `GradePolicy`: determines the letter grade and honor-roll eligibility.
- `TranscriptFormatter`: formats a student's plain-text transcript.
- `GradeCsvExport`: formats the sorted grade CSV.

## 10. AppointmentDesk

Originally it tracked bookings, applied opening hours, searched slots, generated
calendar data, and wrote SMS reminders. Clinic hours, calendar interoperability,
and reminder wording can change independently.

- `AppointmentDesk`: maintains bookings and searches for an available slot.
- `ClinicHoursPolicy`: checks opening hours and closed weekdays.
- `AppointmentCalendar`: formats the original ICS calendar payload.
- `AppointmentSms`: formats SMS reminder text.

The original alignment, weekend policy, and calendar serialization are preserved.
Internal query properties provide collaborators with the data they need. Mutable
storage stays private; no new class is a catch-all utility.

Each of the ten original examples has its own folder under `src/SrpLab`.
The wrapping charge belongs to basket pricing; coupon interpretation remains
separate because marketing coupon rules can change independently.
