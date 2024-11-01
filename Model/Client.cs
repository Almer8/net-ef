using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace NetPract2.Model
{
    class Client
    {
        [Key]
        public int Id { get; set; }
        public String Name { get; set; }
        public String ClientID { get; set; }
    }
}
