using NetPract2.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetPract2.Repository
{
    internal class DepositRepository
    {
        public List<Deposit> getAllDeposits()
        {
            using (DBContext ctx = new DBContext())
            {
                return ctx.Deposits.ToList();
            }
        }
        public List<Deposit> getAllDepositsWithDepositTypeId(int id)
        {
            using (DBContext ctx = new DBContext())
            {
                return (from deposit in ctx.Deposits
                        where deposit.DepositTypeId == id
                        select deposit).ToList();
            }
        }

        public List<Deposit> getAllDepositWhereClientNameContains(string clientName, bool flag) {

            using (DBContext ctx = new DBContext())
            {
                return (from deposit in ctx.Deposits
                        join client in ctx.Clients on deposit.ClientId equals client.Id
                        where client.Name.Contains(clientName)
                        where deposit.isEnded == flag
                        select deposit).ToList();
            }

        }
        
        public List<Deposit> getAllDepositSortDescending(string clientName, bool flag) {

            using (DBContext ctx = new DBContext())
            {
                return (from deposit in ctx.Deposits
                        join client in ctx.Clients on deposit.ClientId equals client.Id
                        where client.Name.Contains(clientName)
                        where deposit.isEnded == flag
                        orderby deposit.DepositedMoney descending
                        select deposit).ToList();
            }

        }

        public List<Deposit> getAllDepositSortAscending(string clientName, bool flag)
        {

            using (DBContext ctx = new DBContext())
            {
                return (from deposit in ctx.Deposits
                        join client in ctx.Clients on deposit.ClientId equals client.Id
                        where client.Name.Contains(clientName)
                        where deposit.isEnded == flag
                        orderby deposit.DepositedMoney
                        select deposit).ToList();
            }

        }

        public List<Deposit> getAllDepositsWithClientId(int id, bool flag)
        {
            using (DBContext ctx = new DBContext())
            {
                return (from deposit in ctx.Deposits
                        where deposit.ClientId == id
                        where deposit.isEnded == flag
                        select deposit).ToList();
            }
        }

        public void createDeposit(Deposit deposit) 
        {
            using (DBContext ctx = new DBContext())
            {
                ctx.Add(deposit);
                ctx.SaveChanges();
            }
        }

        public void updateDeposit(Deposit deposit)
        {
            using (DBContext ctx = new DBContext())
            {
                var entity = ctx.Deposits.FirstOrDefault(e => e.Id == deposit.Id);
                if (entity != null)
                {
                    entity.isEnded = true;
                    ctx.SaveChanges();
                }

            }
        }
        public void deleteDepositsList(List<Deposit> deposits)
        {
            using(DBContext ctx = new DBContext())
            {
                ctx.Deposits.RemoveRange(deposits);
                ctx.SaveChanges();
            }
        }

        public void deleteDepositById(int id) 
        {
            using(DBContext ctx = new DBContext())
            {
                var entity = ctx.Deposits.FirstOrDefault(e => e.Id == id);
                if (entity != null)
                {
                    ctx.Deposits.Remove(entity);
                    ctx.SaveChanges();
                }
            }
        }
    }
}
