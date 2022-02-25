using System.Threading;
using System.Threading.Tasks;
using MediatR;
using N5.Exam.Domain.CQRS.Commands;
using N5.Exam.Domain.Exceptions;
using N5.Exam.Domain.Models;
using N5.Exam.Domain.Repositories;

namespace N5.Exam.Domain.CQRS.Handlers
{
    public class ModifyPermissionCommandHandler : IRequestHandler<ModifyPermissionCommand, PermissionItemDto>
    {
        private readonly IPermissionRepository _permissionRepository;

        public ModifyPermissionCommandHandler(IPermissionRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        public async Task<PermissionItemDto> Handle(ModifyPermissionCommand request, CancellationToken cancellationToken)
        {
            if (!await _permissionRepository.CheckIfPermissionTypeExistsAsync(request.ModifiedPermission.PermissionTypeId, cancellationToken))
            {
                throw new ValidationException("The given permission id is invalid.");
            }

            await _permissionRepository.UpdateAsync(request.PermissionId, request.ModifiedPermission, cancellationToken);
            await _permissionRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return await _permissionRepository.GetAsync(request.PermissionId, cancellationToken);
        }
    }
}
