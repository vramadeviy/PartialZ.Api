namespace PartialZ.Api.Dtos
{
    public class AffidavitDto
    {
     

        public string EmployerEmail { get; set; }


        public string Eannumber { get; set; }

        public string Feinnumber { get; set; }
        public int EmployerID { get; set; }
        public string EmployerName { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string ZIP { get; set; }
        public string Email { get; set; }

        public string BusinessTitle { get; set; }

        public DateTime? PayrollEndDay { get; set; }
        public int ZipPlus4 { get; set; }

        public string ContactFirstName { get; set; }

        public string ContactLastName { get; set; }

        public string ContactPhone { get; set; }

        public decimal Wages { get; set; }
    }
    
}
