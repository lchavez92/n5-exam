using System;

namespace N5.Exam.Domain.Models
{
    public class PermissionItemDto
    {
        /// <summary>
        /// Permission's ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Employee's Forename
        /// </summary>
        public string EmployeeForename { get; set; }

        /// <summary>
        /// Employee's Surname
        /// </summary>
        public string EmployeeSurname { get; set; }

        /// <summary>
        /// Permission Type ID
        /// </summary>
        public int PermissionTypeId { get; set; }

        /// <summary>
        /// Description of Permission Type
        /// </summary>
        public string PermissionTypeDescription { get; set; }

        /// <summary>
        /// Permission Issue Date
        /// </summary>
        public DateTime PermissionDate { get; set; }
    }
}
