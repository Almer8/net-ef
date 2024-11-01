using NetPract2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetPract2.Repository
{
    internal class DepositTypeRepository
    {
        public List<DepositType> getAllDepositTypes()
        {
            using (DBContext ctx = new DBContext())
            {
                return ctx.DepositTypes.ToList();
            }
        }
        public void createDepositType(DepositType depositType)
        {
            using (DBContext ctx = new DBContext())
            {
                ctx.DepositTypes.Add(depositType);
                ctx.SaveChanges();
            }
        }

        public DepositType getDepositTypeById(int id)
        {
            using (DBContext ctx = new DBContext())
            {
                return (from depositType in ctx.DepositTypes
                        where depositType.Id == id
                        select depositType).FirstOrDefault();
            }
        }

        public DepositType getDepositTypeByName(String name)
        {
            using (DBContext ctx = new DBContext())
            {
                return (from depositType in ctx.DepositTypes
                        where depositType.Name == name
                        select depositType).FirstOrDefault();
            }
        }

        public void updateDepositType(DepositType depositType)
        {
            using (DBContext ctx = new DBContext())
            {
                var entity = ctx.DepositTypes.FirstOrDefault(e => e.Id == depositType.Id);
                if (entity != null)
                {
                    entity.MinTerm = depositType.MinTerm;
                    entity.Name = depositType.Name;
                    entity.Percent = depositType.Percent;
                    entity.Regularity = depositType.Regularity;
                    ctx.SaveChanges();
                }
                
            }
        }
        public void deleteDepositTypeById(int id)
        {
            using (DBContext ctx = new DBContext()) 
            {
                var entity = ctx.DepositTypes.FirstOrDefault(e => e.Id == id);
                if (entity != null)
                {
                    ctx.DepositTypes.Remove(entity);
                    ctx.SaveChanges();
                }
            }
        }
    }
}
