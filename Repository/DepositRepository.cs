using NetPract2.Model;
using System;
using System.Collections.Generic;
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

        public List<Deposit> getAllDepositWhereClientNameContains(string clientName) {

            using (DBContext ctx = new DBContext())
            {
                return (from deposit in ctx.Deposits
                        join client in ctx.Clients on deposit.ClientId equals client.Id
                        where client.Name.Contains(clientName)
                        select deposit).ToList();
            }

        }

        public List<Deposit> getAllDepositsWithClientId(int id)
        {
            using (DBContext ctx = new DBContext())
            {
                return (from deposit in ctx.Deposits
                        where deposit.ClientId == id
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
