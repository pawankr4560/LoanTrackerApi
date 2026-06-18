using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Data.Entity
{
    public class Loan
    {
        [Key]
        [Column("F_Loan_Index")]
        public int Id { get; set; }

        [Column("F_User_Index")]
        public string UserId { get; set; } = string.Empty;

        [Column("F_Loan_Number")]
        public string LoanNumber { get; set; }

        [Column("F_Loan_Amount")]
        public double LoanAmount { get; set; }
        [Column("F_Installment")]
        public double EMI { get; set; }

        [Column("F_Rate")]
        public double Rate { get; set; }
        [Column("F_Tenure")]
        public float Tenure { get; set; }
        public bool Active { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime F_Created_Date_Time { get; set; }
        public DateTime F_Updated_Date_Time { get; set; }
        public int F_User_Index_Created { get; set; }
        public DateTime F_User_Index_Update { get; set; }
    }
}
