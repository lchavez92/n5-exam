using MediatR;
using N5.Exam.Domain.Models;

namespace N5.Exam.Domain.CQRS.Commands
{
    public class GetPermissionByIdQuery : IRequest<PermissionItemDto>
    {
        public int PermissionId { get; }

        public GetPermissionByIdQuery(int permissionId)
        {
            PermissionId = permissionId;
        }
    }
}
