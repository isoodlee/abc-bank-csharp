using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace abc_bank
{
    public class Account
    {

        //public const int CHECKING = 0;
        //public const int SAVINGS = 1;
        //public const int MAXI_SAVINGS = 2;

        Mutex _lock = new Mutex();
        public enum eAccountType
        {
            CHECKING,
            SAVINGS,
            MAXI_SAVINGS,
        };

        //private readonly int accountType;
        private readonly eAccountType accountType;
        public List<Transaction> transactions;

        public Account(eAccountType accountType) 
        {
            this.accountType = accountType;
            this.transactions = new List<Transaction>();
        }


        private void AddTransaction(double amt)
        {
            if (_lock.WaitOne())
            {
                try
                {
                    transactions.Add(new Transaction(amt));
                }
                finally
                {
                    _lock.ReleaseMutex();
                }
            }
        }

        public void Deposit(double amount) 
        {
            if (amount <= 0) {
                throw new ArgumentException("amount must be greater than zero");
            } else {
                //transactions.Add(new Transaction(amount));
                AddTransaction(amount);
            }
        }

        public void Withdraw(double amount) 
        {
            if (amount <= 0) {
                throw new ArgumentException("amount must be greater than zero");
            } else {
                //transactions.Add(new Transaction(-amount));
                AddTransaction(-amount);
            }
        }

        public double InterestEarned() 
        {
            double amount = sumTransactions();
            switch(accountType){
                case eAccountType.SAVINGS:
                    if (amount <= 1000)
                        return amount * 0.001;
                    else
                        return 1 + (amount-1000) * 0.002;
    //            case SUPER_SAVINGS:
    //                if (amount <= 4000)
    //                    return 20;
                case eAccountType.MAXI_SAVINGS:
                    if (amount <= 1000)
                        return amount * 0.02;
                    if (amount <= 2000)
                        return 20 + (amount-1000) * 0.05;
                    return 70 + (amount-2000) * 0.1;
                default:
                    return amount * 0.001;
            }
        }

        public double sumTransactions() {
           return CheckIfTransactionsExist(true);
        }

        private double CheckIfTransactionsExist(bool checkAll) 
        {
            double amount = 0.0;
            foreach (Transaction t in transactions)
                amount += t.amount;
            return amount;
        }

        public eAccountType GetAccountType() 
        {
            return accountType;
        }

        public void TransferFrom(Account other, double amt)
        {
            Mutex[] lockd = {this._lock, other._lock};
            //if(WaitHandle.WaitAll(lockd))
            if(this._lock.WaitOne() && other._lock.WaitOne())
            {
                try
                {
                    other.Withdraw(amt);
                    this.Deposit(amt);
                }
                finally
                {
                    foreach(var l in lockd)
                    {
                        l.ReleaseMutex();
                    }
                }

            }

        }

    }
}
