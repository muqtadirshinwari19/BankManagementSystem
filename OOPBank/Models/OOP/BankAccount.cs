namespace BankSystem.Models.OOP
{
    public abstract class BankAccount
    {
        // Encapsulation
        protected decimal Balance;

        // Constructor
        protected BankAccount(decimal openingBalance)
        {
            Balance = openingBalance;
        }

        // Get Current Balance
        public decimal GetBalance()
        {
            return Balance;
        }

        // Deposit (Compile Time Polymorphism later)
        public virtual void Deposit(decimal amount)
        {
            if (amount <= 0)
                throw new Exception("Deposit amount must be greater than zero.");

            Balance += amount;
        }

        // Withdraw
        public virtual bool Withdraw(decimal amount)
        {
            if (amount <= 0)
                throw new Exception("Withdrawal amount must be greater than zero.");

            if (Balance < amount)
                return false;

            Balance -= amount;

            return true;
        }

        // Transfer
        public bool Transfer(BankAccount receiver, decimal amount)
        {
            if (Withdraw(amount))
            {
                receiver.Deposit(amount);
                return true;
            }

            return false;
        }

        // Abstraction
        public abstract decimal CalculateInterest();
    }
}