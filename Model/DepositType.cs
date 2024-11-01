using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetPract2.Model
{
    class DepositType
    {
        [Key]
        public int Id { get; set; }
        public String Name { get; set; }
        public float Percent { get; set; }
        public bool CompoundInt { get; set; }
        public int Regularity { get; set; }
        public int MinTerm { get; set; }

    }
}
