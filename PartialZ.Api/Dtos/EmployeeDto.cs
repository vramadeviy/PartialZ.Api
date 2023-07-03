namespace PartialZ.Api.Dtos
{
    public class EmployeeDto
    {
        public int EmployeeId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string BusinessTitle { get; set; }

        public string PhoneNumber { get; set; }

        public int IsVerified { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime LastModifedDate { get; set; }
    }
}
