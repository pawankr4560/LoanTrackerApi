namespace WebApp.Model.Transaction
{
    public class LoanDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public string? UserName { get; set; }

        public string LoanNumber { get; set; } = string.Empty;

        public double LoanAmount { get; set; }

        public double Rate { get; set; }
        public double EMI { get; set; }
        public float Tenure { get; set; }

        public bool Active { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public DateTime UpdatedDateTime { get; set; }

        public int CreatedBy { get; set; }

        public int UpdatedBy { get; set; }
    }
}
