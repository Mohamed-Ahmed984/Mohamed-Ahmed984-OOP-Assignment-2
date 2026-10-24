using LibrarySystem;

Console.WriteLine("=== CITY LIBRARY ===");
DateTime borrowDate = new DateTime(2026, 10, 1);
DateTime hireDate = new DateTime(2025, 1, 1);
var student = new StudentMember("S1", "Mohamed Ahmed", "01000000001");
var premium = new PremiumMember("P1", "Sara Ali", "01000000002", 20m);
var librarian = new Librarian("L1", "Nora Hassan", "01000000003", hireDate, 5000m);
var head = new HeadLibrarian("H1", "Omar Adel", "01000000004", hireDate, 7000m);
var shelver = new Shelver("SH1", "Ali Mahmoud", "01000000005", hireDate, 4000m, "Fiction");
var book = new Book("B1", "Learning C#", 2m);
var dvd = new DVD("D1", "Science Documentary", 2m);
var magazine = new Magazine("M1", "Monthly Science", 2m);

Console.WriteLine("\nMonthly pay, through Staff:");
var staffMembers = new List<Staff> { librarian, head, shelver };
foreach (Staff employee in staffMembers)
    Console.WriteLine(employee.FullName + ": " + employee.GetMonthlyPay());
Check(librarian.GetMonthlyPay() == 5000m, "Librarian salary");
Check(head.GetMonthlyPay() == 7400m, "Head gets the fixed 400 allowance");
shelver.GiveRaise(10m);
Check(shelver.GetMonthlyPay() == 4400m, "Positive raise changes salary");
head.GiveRaise(10m);
Check(head.GetMonthlyPay() == 8100m, "Raise affects salary, allowance stays 400");
shelver.Reassign("Children");
Check(shelver.Section == "Children", "Shelver reassignment");

Console.WriteLine("\nPeriods and daily fees, through LibraryItem:");
var items = new List<LibraryItem> { book, dvd, magazine };
foreach (LibraryItem item in items)
    Console.WriteLine(item.Title + ": " + item.LoanPeriodDays + " days, daily fee " + item.GetDailyLateFee());
Check(book.LoanPeriodDays == 21 && book.GetDailyLateFee() == 2m, "Book rules");
Check(dvd.LoanPeriodDays == 7 && dvd.GetDailyLateFee() == 4m, "DVD rules");
Check(magazine.LoanPeriodDays == 3 && magazine.GetDailyLateFee() == 1m, "Magazine rules");
head.ChangeLateFee(book, 3m);
Check(book.GetDailyLateFee() == 3m, "Head changes the base late fee");
head.Withdraw(book);

Console.WriteLine("\nReject: Borrowing a withdrawn item");
bool rejected1 = false;
try
{
    student.Borrow(book, borrowDate);
}
catch (InvalidOperationException ex)
{
    rejected1 = true;
    Console.WriteLine(ex.Message);
}
Check(rejected1, "Borrowing a withdrawn item");
head.Restore(book);
Loan firstLoan = student.Borrow(book, borrowDate);
Check(book.IsOnLoan && !book.IsWithdrawn, "Restored book is borrowed");
Check(firstLoan.Item == book && firstLoan.Member == student, "Loan keeps real item and member references");
Check(firstLoan.DueDate == borrowDate.AddDays(21), "Due date is calculated");

Console.WriteLine("\nReject: Borrowing an item already on loan");
bool rejected2 = false;
try
{
    premium.Borrow(book, borrowDate);
}
catch (InvalidOperationException ex)
{
    rejected2 = true;
    Console.WriteLine(ex.Message);
}
Check(rejected2, "Borrowing an item already on loan");
student.Borrow(new Book("B2", "OOP Basics", 2m), borrowDate);
student.Borrow(new Book("B3", "SQL Basics", 2m), borrowDate);
var fourthBook = new Book("B4", "Web Basics", 2m);

Console.WriteLine("\nReject: Student borrowing a fourth item");
bool rejected3 = false;
try
{
    student.Borrow(fourthBook, borrowDate);
}
catch (InvalidOperationException ex)
{
    rejected3 = true;
    Console.WriteLine(ex.Message);
}
Check(rejected3, "Student borrowing a fourth item");
Check(student.Loans.Count == 3 && !fourthBook.IsOnLoan, "Failed borrowing changes neither history nor item");
Loan premiumLoan = premium.Borrow(dvd, borrowDate);
librarian.ProcessReturn(premiumLoan, premiumLoan.DueDate.AddDays(5));
Console.WriteLine("\nPremium DVD returned five days late:");
Console.WriteLine("Due date: " + premiumLoan.DueDate.ToString("yyyy-MM-dd"));
Console.WriteLine("Late fee: " + premiumLoan.LateFee);
Console.WriteLine("Reading points: " + premium.ReadingPoints);
Check(premiumLoan.LateFee == 16m, "5 days x 4 daily fee x 80% = 16");
Check(premium.ReadingPoints == 5, "Five points per returned loan");
Check(premiumLoan.Status == LoanStatus.Returned && !dvd.IsOnLoan, "Returning releases the item");
Check(premiumLoan.ReturnDate == new DateTime(2026, 10, 13), "Return date is recorded");

Console.WriteLine("\nReject: Returning the same loan twice");
bool rejected4 = false;
try
{
    librarian.ProcessReturn(premiumLoan, premiumLoan.DueDate.AddDays(6));
}
catch (InvalidOperationException ex)
{
    rejected4 = true;
    Console.WriteLine(ex.Message);
}
Check(rejected4, "Returning the same loan twice");

Console.WriteLine("\nReject: Marking a returned loan lost");
bool rejected5 = false;
try
{
    librarian.MarkLost(premiumLoan);
}
catch (InvalidOperationException ex)
{
    rejected5 = true;
    Console.WriteLine(ex.Message);
}
Check(rejected5, "Marking a returned loan lost");
Check(premium.ReadingPoints == 5, "Rejected actions never add reading points");

Console.WriteLine("\nReject: Returning before the borrow date");
bool rejected6 = false;
try
{
    librarian.ProcessReturn(firstLoan, borrowDate.AddDays(-1));
}
catch (ArgumentException ex)
{
    rejected6 = true;
    Console.WriteLine(ex.Message);
}
Check(rejected6, "Returning before the borrow date");
Check(firstLoan.Status == LoanStatus.Borrowed && book.IsOnLoan, "Invalid return preserves loan state");
librarian.ProcessReturn(firstLoan, firstLoan.DueDate);
Check(firstLoan.LateFee == 0m, "Returning on the due date is free");
student.Borrow(fourthBook, borrowDate.AddDays(22));
Check(student.Loans.Count == 4, "History retains returned loans and a free slot can be reused");
Loan early = premium.Borrow(dvd, borrowDate.AddDays(20));
librarian.ProcessReturn(early, early.BorrowDate);
Check(early.LateFee == 0m && premium.ReadingPoints == 10, "Early return is free and earns five points");
Loan lostLoan = premium.Borrow(magazine, borrowDate);
librarian.MarkLost(lostLoan);
Check(lostLoan.Status == LoanStatus.Lost && magazine.IsOnLoan, "Lost item stays unavailable");
Check(premium.ReadingPoints == 10 && lostLoan.ReturnDate == null, "Lost items earn no returned-loan points");

Console.WriteLine("\nReject: Returning a lost loan");
bool rejected7 = false;
try
{
    librarian.ProcessReturn(lostLoan, borrowDate.AddDays(1));
}
catch (InvalidOperationException ex)
{
    rejected7 = true;
    Console.WriteLine(ex.Message);
}
Check(rejected7, "Returning a lost loan");

Console.WriteLine("\nReject: Marking a lost loan lost again");
bool rejected8 = false;
try
{
    librarian.MarkLost(lostLoan);
}
catch (InvalidOperationException ex)
{
    rejected8 = true;
    Console.WriteLine(ex.Message);
}
Check(rejected8, "Marking a lost loan lost again");

Console.WriteLine("\nReject: Borrowing the lost item again");
bool rejected9 = false;
try
{
    student.Borrow(magazine, borrowDate);
}
catch (InvalidOperationException ex)
{
    rejected9 = true;
    Console.WriteLine(ex.Message);
}
Check(rejected9, "Borrowing the lost item again");

Console.WriteLine("\nReject: Zero raise");
bool rejected10 = false;
try
{
    shelver.GiveRaise(0m);
}
catch (ArgumentException ex)
{
    rejected10 = true;
    Console.WriteLine(ex.Message);
}
Check(rejected10, "Zero raise");

Console.WriteLine("\nReject: Negative raise");
bool rejected11 = false;
try
{
    shelver.GiveRaise(-10m);
}
catch (ArgumentException ex)
{
    rejected11 = true;
    Console.WriteLine(ex.Message);
}
Check(rejected11, "Negative raise");

Console.WriteLine("\nReject: Zero late fee");
bool rejected12 = false;
try
{
    head.ChangeLateFee(book, 0m);
}
catch (ArgumentException ex)
{
    rejected12 = true;
    Console.WriteLine(ex.Message);
}
Check(rejected12, "Zero late fee");

Console.WriteLine("\nReject: Negative late fee");
bool rejected13 = false;
try
{
    head.ChangeLateFee(book, -1m);
}
catch (ArgumentException ex)
{
    rejected13 = true;
    Console.WriteLine(ex.Message);
}
Check(rejected13, "Negative late fee");

Console.WriteLine("\nReject: Empty person ID");
bool rejected14 = false;
try
{
    new StudentMember(" ", "Name", "010");
}
catch (ArgumentException ex)
{
    rejected14 = true;
    Console.WriteLine(ex.Message);
}
Check(rejected14, "Empty person ID");

Console.WriteLine("\nReject: Empty full name");
bool rejected15 = false;
try
{
    new StudentMember("INVALID", "", "010");
}
catch (ArgumentException ex)
{
    rejected15 = true;
    Console.WriteLine(ex.Message);
}
Check(rejected15, "Empty full name");

Console.WriteLine("\nReject: Null phone");
bool rejected16 = false;
try
{
    new StudentMember("INVALID", "Name", null);
}
catch (ArgumentException ex)
{
    rejected16 = true;
    Console.WriteLine(ex.Message);
}
Check(rejected16, "Null phone");

Console.WriteLine("\nReject: Duplicate person ID");
bool rejected17 = false;
try
{
    new StudentMember("S1", "Name", "010");
}
catch (InvalidOperationException ex)
{
    rejected17 = true;
    Console.WriteLine(ex.Message);
}
Check(rejected17, "Duplicate person ID");

Console.WriteLine("\nReject: Invalid discount below zero");
bool rejected18 = false;
try
{
    new PremiumMember("INVALID", "Name", "010", -1m);
}
catch (ArgumentException ex)
{
    rejected18 = true;
    Console.WriteLine(ex.Message);
}
Check(rejected18, "Invalid discount below zero");

Console.WriteLine("\nReject: Invalid discount above 100");
bool rejected19 = false;
try
{
    new PremiumMember("INVALID", "Name", "010", 101m);
}
catch (ArgumentException ex)
{
    rejected19 = true;
    Console.WriteLine(ex.Message);
}
Check(rejected19, "Invalid discount above 100");

Console.WriteLine("\nReject: Nonpositive salary");
bool rejected20 = false;
try
{
    new Librarian("INVALID", "Name", "010", hireDate, 0m);
}
catch (ArgumentException ex)
{
    rejected20 = true;
    Console.WriteLine(ex.Message);
}
Check(rejected20, "Nonpositive salary");

Console.WriteLine("\nReject: Empty initial section");
bool rejected21 = false;
try
{
    new Shelver("INVALID", "Name", "010", hireDate, 1000m, " ");
}
catch (ArgumentException ex)
{
    rejected21 = true;
    Console.WriteLine(ex.Message);
}
Check(rejected21, "Empty initial section");

Console.WriteLine("\nReject: Empty reassigned section");
bool rejected22 = false;
try
{
    shelver.Reassign("");
}
catch (ArgumentException ex)
{
    rejected22 = true;
    Console.WriteLine(ex.Message);
}
Check(rejected22, "Empty reassigned section");

Console.WriteLine("\nReject: Empty catalog number");
bool rejected23 = false;
try
{
    new Book("", "Title", 1m);
}
catch (ArgumentException ex)
{
    rejected23 = true;
    Console.WriteLine(ex.Message);
}
Check(rejected23, "Empty catalog number");

Console.WriteLine("\nReject: Empty title");
bool rejected24 = false;
try
{
    new Book("INVALID", " ", 1m);
}
catch (ArgumentException ex)
{
    rejected24 = true;
    Console.WriteLine(ex.Message);
}
Check(rejected24, "Empty title");

Console.WriteLine("\nReject: Nonpositive initial fee");
bool rejected25 = false;
try
{
    new Book("INVALID", "Title", 0m);
}
catch (ArgumentException ex)
{
    rejected25 = true;
    Console.WriteLine(ex.Message);
}
Check(rejected25, "Nonpositive initial fee");

Console.WriteLine("\nReject: Duplicate catalog number");
bool rejected26 = false;
try
{
    new DVD("B1", "Duplicate", 1m);
}
catch (InvalidOperationException ex)
{
    rejected26 = true;
    Console.WriteLine(ex.Message);
}
Check(rejected26, "Duplicate catalog number");

Console.WriteLine("\nReject: Borrowing a null item");
bool rejected27 = false;
try
{
    premium.Borrow(null, borrowDate);
}
catch (ArgumentNullException ex)
{
    rejected27 = true;
    Console.WriteLine(ex.Message);
}
Check(rejected27, "Borrowing a null item");

Console.WriteLine("\nReject: Returning a null loan");
bool rejected28 = false;
try
{
    librarian.ProcessReturn(null, borrowDate);
}
catch (ArgumentNullException ex)
{
    rejected28 = true;
    Console.WriteLine(ex.Message);
}
Check(rejected28, "Returning a null loan");

Console.WriteLine("\nReject: Marking a null loan lost");
bool rejected29 = false;
try
{
    librarian.MarkLost(null);
}
catch (ArgumentNullException ex)
{
    rejected29 = true;
    Console.WriteLine(ex.Message);
}
Check(rejected29, "Marking a null loan lost");

Console.WriteLine("\nReject: Pricing a null item");
bool rejected30 = false;
try
{
    head.ChangeLateFee(null, 1m);
}
catch (ArgumentNullException ex)
{
    rejected30 = true;
    Console.WriteLine(ex.Message);
}
Check(rejected30, "Pricing a null item");

Console.WriteLine("\nReject: Withdrawing a null item");
bool rejected31 = false;
try
{
    head.Withdraw(null);
}
catch (ArgumentNullException ex)
{
    rejected31 = true;
    Console.WriteLine(ex.Message);
}
Check(rejected31, "Withdrawing a null item");

Console.WriteLine("\nReject: Restoring a null item");
bool rejected32 = false;
try
{
    head.Restore(null);
}
catch (ArgumentNullException ex)
{
    rejected32 = true;
    Console.WriteLine(ex.Message);
}
Check(rejected32, "Restoring a null item");

Console.WriteLine("\nPremium active-loan limit:");
for (int i = 1; i <= 10; i++)
    premium.Borrow(new Book("LIMIT" + i, "Book " + i, 1m), borrowDate);
var eleventh = new Book("LIMIT11", "Eleventh", 1m);

Console.WriteLine("\nReject: Premium borrowing an eleventh active item");
bool rejected33 = false;
try
{
    premium.Borrow(eleventh, borrowDate);
}
catch (InvalidOperationException ex)
{
    rejected33 = true;
    Console.WriteLine(ex.Message);
}
Check(rejected33, "Premium borrowing an eleventh active item");
Check(!eleventh.IsOnLoan, "Failed premium borrowing leaves the item available");

Check(premium.Loans.Count == 13, "Complete premium loan history is preserved");
var fullDiscount = new PremiumMember("P100", "Full Discount", "010100", 100m);
Loan freeLoan = fullDiscount.Borrow(new Book("FREE1", "Discount Example", 2m), borrowDate);
librarian.ProcessReturn(freeLoan, freeLoan.DueDate.AddDays(2));
Check(freeLoan.LateFee == 0m, "100 percent discount gives zero late fee");
var noDiscount = new PremiumMember("P0", "No Discount", "010101", 0m);
Loan paidLoan = noDiscount.Borrow(new Magazine("PAID1", "No Discount Example", 2m), borrowDate);
librarian.ProcessReturn(paidLoan, paidLoan.DueDate.AddDays(2));
Check(paidLoan.LateFee == 2m, "Zero discount preserves the full magazine late fee");
var lateStudent = new StudentMember("SLATE", "Late Student", "010102");
Loan studentLateLoan = lateStudent.Borrow(new Book("SLATE1", "Student Late Example", 3m), borrowDate);
librarian.ProcessReturn(studentLateLoan, studentLateLoan.DueDate.AddDays(2));
Check(studentLateLoan.LateFee == 6m, "Students receive no late fee discount");
Console.WriteLine("All library checks passed.");

static void Check(bool condition, string message)
{
    if (!condition)
        throw new Exception("FAIL: " + message);
    Console.WriteLine("PASS: " + message);
}
