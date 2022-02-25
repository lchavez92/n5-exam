using System.Threading;
using System.Threading.Tasks;
using N5.Exam.Domain.Models;
using N5.Exam.Infrastructure.Models;
using N5.Exam.Shared.Repositories;

namespace N5.Exam.Domain.Repositories
{
    public interface IPermissionRepository: IRepository
    {
        Task<PermissionItemDto> GetAsync(int permissionId, CancellationToken cancellationToken = default);

        Task<Permission> AddAsync(RequestPermissionDto requestPermission,
            CancellationToken cancellationToken = default);

        Task UpdateAsync(int permissionId, ModifyPermissionDto requestPermission,
            CancellationToken cancellationToken = default);

        Task<bool> CheckIfPermissionTypeExistsAsync(int permissionId,
            CancellationToken cancellationToken = default);
    }
}
