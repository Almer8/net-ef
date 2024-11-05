using NetPract2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace NetPract2.Repository
{
    internal class ClientRepository
    {
        public List<Client> getAllClients()
        {
            using (DBContext ctx = new DBContext())
            {
                return ctx.Clients.ToList();
            }
        }
        public Client getClientByPassportID(string id) {
            using(DBContext ctx = new DBContext())
            {
                return (from client in ctx.Clients
                                where client.ClientID == id
                        select client).FirstOrDefault();
            }
        }
        public void createClient(Client client)
        {
            using (DBContext ctx = new DBContext())
            {
                ctx.Clients.Add(client);
                ctx.SaveChanges();
            }
        }
        public void updateClient(Client client)
        {
            using (DBContext ctx = new DBContext())
            {
                var entity = ctx.Clients.FirstOrDefault(e => e.Id == client.Id);
                if (entity != null)
                {
                    entity.Name = client.Name;
                    entity.ClientID = client.ClientID;
                    ctx.SaveChanges();
                }

            }
        }
        public void deleteClientById(int id)
        {
            using (DBContext ctx = new DBContext())
            {
                var entity = ctx.Clients.FirstOrDefault(e => e.Id == id);
                if (entity != null)
                {
                    ctx.Clients.Remove(entity);
                    ctx.SaveChanges();
                }
            }
        }
    }
}
