using MediatR;
using N5.Exam.Domain.Models;

namespace N5.Exam.Domain.CQRS.Commands
{
    public class ModifyPermissionCommand: IRequest<PermissionItemDto>
    {
        public ModifyPermissionDto ModifiedPermission { get; }
        public int PermissionId { get; }

        public ModifyPermissionCommand(int permissionId, ModifyPermissionDto modifiedPermission)
        {
            PermissionId = permissionId;
            ModifiedPermission = modifiedPermission;
        }
    }
}
