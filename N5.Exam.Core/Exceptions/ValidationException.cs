using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5.Exam.Domain.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException()
        {

        }

        public ValidationException(string message) : base(message)
        {

        }

        public ValidationException(string format, params object[] args) : base(string.Format(format, args))
        {

        }

        public ValidationException(string message, Exception inner) : base(message, inner) { }
    }
}
