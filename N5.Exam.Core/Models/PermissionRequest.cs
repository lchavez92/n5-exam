using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5.Exam.Domain.Models
{
    public class PermissionRequest
    {
        public Guid Id { get; set; }

        public string OperationName { get; set; }
    }
}
