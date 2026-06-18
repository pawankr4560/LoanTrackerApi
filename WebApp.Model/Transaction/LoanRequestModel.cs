namespace WebApp.Model.Transaction
{
    public class LoanRequestModel
    {
        public int Id { get; set; }

        public string UserId { get; set; } = string.Empty;

        public string LoanNumber { get; set; } = string.Empty;

        public double LoanAmount { get; set; }
        public double EMI { get; set; }

        public double Rate { get; set; }
        public float Tenure { get; set; }

        public bool Active { get; set; }
    }
}
