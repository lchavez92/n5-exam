using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using N5.Exam.Domain.CQRS.Commands;
using N5.Exam.Domain.CQRS.Handlers;
using N5.Exam.Domain.Exceptions;
using N5.Exam.Domain.Models;
using N5.Exam.UnitTests.Mocks;
using Shouldly;
using Xunit;

namespace N5.Exam.UnitTests.CQRS.Commands
{
    public class RequestPermissionCommandHandlerTests
    {
        private readonly RequestPermissionCommandHandler _handler;

        public RequestPermissionCommandHandlerTests()
        {
            _handler = new RequestPermissionCommandHandler(MockPermissionRepository.GetPermissionRepository());
        }

        [Fact]
        public async Task Valid_Permission_Added()
        {
            var result = await _handler.Handle(new RequestPermissionCommand(new RequestPermissionDto
            {
                EmployeeForename = "Frank",
                EmployeeSurname = "Sinatra",
                PermissionTypeId = 1
            }), CancellationToken.None);

            result.ShouldNotBeNull();
            result.ShouldBeOfType<PermissionItemDto>();
            result.Id.ShouldNotBe(0);
        }

        [Fact]
        public async Task InValid_Permission_Added()
        {
            await Assert.ThrowsAsync<ValidationException>(async () => await _handler.Handle(
                new RequestPermissionCommand(new RequestPermissionDto
                {
                    EmployeeForename = "Jude",
                    EmployeeSurname = "Armstrong",
                    PermissionTypeId = 6
                }), CancellationToken.None));

        }
    }
}
