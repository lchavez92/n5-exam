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
    public class ModifyPermissionCommandHandlerTests
    {
        private readonly ModifyPermissionCommandHandler _handler;

        public ModifyPermissionCommandHandlerTests()
        {
            _handler = new ModifyPermissionCommandHandler(MockPermissionRepository.GetPermissionRepository());
        }

        [Fact]
        public async Task Valid_Permission_Updated()
        {
            var result = await _handler.Handle(new ModifyPermissionCommand(1, new ModifyPermissionDto
            {
                EmployeeForename = "Leonel",
                EmployeeSurname = "Sinatra",
                PermissionTypeId = 1
            }), CancellationToken.None);

            result.ShouldNotBeNull();
            result.ShouldBeOfType<PermissionItemDto>();
            result.EmployeeForename.ShouldBe("Leonel");
        }

        [Fact]
        public async Task InValid_Permission_Updated()
        {
            await Assert.ThrowsAsync<ValidationException>(async () => await _handler.Handle(
                new ModifyPermissionCommand(2, new ModifyPermissionDto
                {
                    PermissionTypeId = 6
                }), CancellationToken.None));

        }
    }
}
