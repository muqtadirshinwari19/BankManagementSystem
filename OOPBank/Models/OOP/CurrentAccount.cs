namespace BankSystem.Models.OOP
{
    public class CurrentAccount : BankAccount
    {
        public CurrentAccount(decimal openingBalance) : base(openingBalance)
        {
        }

        public override decimal CalculateInterest()
        {
            return 0;
        }
    }
}