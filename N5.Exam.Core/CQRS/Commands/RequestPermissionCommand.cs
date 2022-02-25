using MediatR;
using N5.Exam.Domain.Models;

namespace N5.Exam.Domain.CQRS.Commands
{
    public class RequestPermissionCommand: IRequest<PermissionItemDto>
    {
        public RequestPermissionDto Permission { get; }

        public RequestPermissionCommand(RequestPermissionDto permission)
        {
            Permission = permission;
        }
    }
}
