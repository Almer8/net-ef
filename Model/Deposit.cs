using System.ComponentModel.DataAnnotations;

namespace NetPract2.Model
{
    class Deposit
    {
        [Key]
        public int Id { get; set; }

        public int ClientId { get; set; }
        public int DepositTypeId { get; set; }
        public double DepositedMoney { get; set; }
        public double ProfitMoney { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
    }
}
