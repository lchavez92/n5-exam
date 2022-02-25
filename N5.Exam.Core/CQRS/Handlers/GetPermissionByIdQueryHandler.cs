using System.Threading;
using System.Threading.Tasks;
using MediatR;
using N5.Exam.Domain.CQRS.Commands;
using N5.Exam.Domain.Models;
using N5.Exam.Domain.Repositories;

namespace N5.Exam.Domain.CQRS.Handlers
{
    public class GetPermissionByIdQueryHandler : IRequestHandler<GetPermissionByIdQuery, PermissionItemDto>
    {
        private readonly IPermissionRepository _permissionRepository;

        public GetPermissionByIdQueryHandler(IPermissionRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        public async Task<PermissionItemDto> Handle(GetPermissionByIdQuery request, CancellationToken cancellationToken)
        {
            var permission = await _permissionRepository.GetAsync(request.PermissionId, cancellationToken);
            return permission;
        }
    }
}
