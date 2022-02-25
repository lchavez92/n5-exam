namespace N5.Exam.Domain.Models
{
    public class ModifyPermissionDto
    {
        public string EmployeeForename { get; set; }

        /// <summary>
        /// Employee's Surname
        /// </summary>
        public string EmployeeSurname { get; set; }

        /// <summary>
        /// Permission Type ID
        /// </summary>
        public int PermissionTypeId { get; set; }
    }
}
