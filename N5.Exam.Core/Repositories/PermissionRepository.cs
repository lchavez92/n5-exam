using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using N5.Exam.Domain.Models;
using N5.Exam.Infrastructure;
using N5.Exam.Infrastructure.Models;
using N5.Exam.Shared.Repositories;

namespace N5.Exam.Domain.Repositories
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly N5ExamContext _context;
        private readonly IMapper _mapper;

        public PermissionRepository(N5ExamContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task<PermissionItemDto> GetAsync(int permissionId, CancellationToken cancellationToken = default)
        {
            var permission =
                await _context.Permissions
                    .Where(x => x.Id == permissionId)
                    .ProjectTo<PermissionItemDto>(_mapper.ConfigurationProvider)
                    .SingleOrDefaultAsync(cancellationToken);

            return permission;
        }

        public async Task<Permission> AddAsync(RequestPermissionDto requestPermission,
            CancellationToken cancellationToken = default)
        {
            var permission = _mapper.Map<Permission>(requestPermission);
            var result = await _context.Permissions.AddAsync(permission, cancellationToken);
            return result.Entity;
        }

        public async Task UpdateAsync(int permissionId, ModifyPermissionDto requestPermission,
            CancellationToken cancellationToken = default)
        {
            var existingAccount = await _context.Permissions
                .SingleOrDefaultAsync(x => x.Id == permissionId, cancellationToken);

            if (existingAccount == null)
            {
                return;
            }

            _mapper.Map(requestPermission, existingAccount);
        }

        public async Task<bool> CheckIfPermissionTypeExistsAsync(int permissionId,
            CancellationToken cancellationToken = default)
        {
            return await _context.PermissionTypes.AnyAsync(x => x.Id == permissionId, cancellationToken);
        }

    }
}
