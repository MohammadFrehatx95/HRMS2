namespace HRMS2.Dtos.Employees
{
    public class SaveEmployeeDto
    {
        public long? Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Email { get; set; }
        public long PositionId { get; set; }
        public DateTime? BirthDate { get; set; }
        public long? DepartmentId { get; set; }
        public decimal Salary { get; set; }
        public long? ManagerId { get; set; }
    }
}
