using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using N5.Exam.Domain.CQRS.Commands;
using N5.Exam.Domain.CQRS.Handlers;
using N5.Exam.Domain.Models;
using N5.Exam.Domain.Repositories;
using N5.Exam.UnitTests.Mocks;
using Shouldly;
using Xunit;

namespace N5.Exam.UnitTests.CQRS.Queries
{
    public class GetPermissionByIdQueryHandlerTests
    {
        private readonly IPermissionRepository _permissionRepository;

        public GetPermissionByIdQueryHandlerTests()
        {
            _permissionRepository = MockPermissionRepository.GetPermissionRepository();
        }

        [Fact]
        public async Task GetPermissionByIdTest()
        {
            var handler = new GetPermissionByIdQueryHandler(_permissionRepository);

            var result = await handler.Handle(new GetPermissionByIdQuery(1), CancellationToken.None);

            result.ShouldNotBeNull();
            result.ShouldBeOfType<PermissionItemDto>();
        }
    }
}
