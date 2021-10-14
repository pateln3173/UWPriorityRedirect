using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    [Serializable]
    [DataContract]
    public class UnderwritingPriority
    {
        public string LoanNumber { get; set; }
        public string LoanProgram { get; set; }
        public string Underwriter { get; set; }
        public string LoanOfficer { get; set; }
        public string LoanOfficerEmail { get; set; }
        public string LOAPAEmail { get; set; }
        public string CurrentMilestone { get; set; }
        public DateTime DateSubmitted { get; set; }
        public DateTime EstimatedClosingDt { get; set; }
        public int Id { get; set; }
        public string LoanProcessor { get; set; }
        public string LoanProcessorEmail { get; set; }
        public string BorrowerLastName { get; set; }
        public string LoanType { get; set; }
        public string Comment { get; set; }

    }
}
