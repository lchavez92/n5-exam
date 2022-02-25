using System.Threading;
using System.Threading.Tasks;
using MediatR;
using N5.Exam.Domain.CQRS.Commands;
using N5.Exam.Domain.Exceptions;
using N5.Exam.Domain.Models;
using N5.Exam.Domain.Repositories;

namespace N5.Exam.Domain.CQRS.Handlers
{
    public class RequestPermissionCommandHandler : IRequestHandler<RequestPermissionCommand, PermissionItemDto>
    {
        private readonly IPermissionRepository _permissionRepository;

        public RequestPermissionCommandHandler(IPermissionRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        public async Task<PermissionItemDto> Handle(RequestPermissionCommand request, CancellationToken cancellationToken)
        {
            if (!await _permissionRepository.CheckIfPermissionTypeExistsAsync(request.Permission.PermissionTypeId, cancellationToken))
            {
                throw new ValidationException("The given permission id is invalid.");
            }

            var result = await _permissionRepository.AddAsync(request.Permission, cancellationToken);
            await _permissionRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return await _permissionRepository.GetAsync(result.Id, cancellationToken);
        }
    }
}
