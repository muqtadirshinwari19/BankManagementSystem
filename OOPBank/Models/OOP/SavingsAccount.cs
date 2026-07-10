namespace BankSystem.Models.OOP
{
    public class SavingsAccount : BankAccount
    {
        public SavingsAccount(decimal openingBalance) : base(openingBalance)
        {
        }

        public override decimal CalculateInterest()
        {
            return Balance * 0.05m;
        }
    }
}