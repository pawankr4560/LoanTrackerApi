namespace WebApp.Model.Transaction
{
    public class LoanRequestModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string LoanNumber { get; set; } = string.Empty;

        public double LoanAmount { get; set; }

        public double Rate { get; set; }

        public bool Active { get; set; }
    }
}
