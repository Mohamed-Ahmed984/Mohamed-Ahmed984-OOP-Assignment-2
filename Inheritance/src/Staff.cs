namespace LibrarySystem;

public class Staff : Person
{
    private decimal _allowance;

    public DateTime HireDate { get; }
    public decimal MonthlySalary { get; private set; }

    protected Staff(string id, string name, string phone, DateTime hireDate,
        decimal salary, decimal allowance) : base(id, name, phone)
    {
        if (salary <= 0)
            throw new ArgumentException("Monthly salary must be positive.");
        if (allowance < 0)
            throw new ArgumentException("Allowance cannot be negative.");

        HireDate = hireDate.Date;
        MonthlySalary = salary;
        _allowance = allowance;
    }

    public void GiveRaise(decimal percentage)
    {
        if (percentage <= 0)
            throw new ArgumentException("Raise percentage must be greater than zero.");
        MonthlySalary += MonthlySalary * percentage / 100m;
    }

    public decimal GetMonthlyPay()
    {
        return MonthlySalary + _allowance;
    }
}
