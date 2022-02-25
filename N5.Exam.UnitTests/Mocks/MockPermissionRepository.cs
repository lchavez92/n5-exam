using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using N5.Exam.Domain.Models;
using N5.Exam.Domain.Profiles;
using N5.Exam.Domain.Repositories;
using N5.Exam.Infrastructure;
using N5.Exam.Infrastructure.Models;

namespace N5.Exam.UnitTests.Mocks
{
    public static class MockPermissionRepository
    {
        private static readonly object _locker = new();

        public static IPermissionRepository GetPermissionRepository()
        {
            lock (_locker)
            {
                var mockMapper = new MapperConfiguration(cfg => { cfg.AddProfile(new PermissionProfile()); });
                var mapper = mockMapper.CreateMapper();

                var builder = new DbContextOptionsBuilder<N5ExamContext>();
                builder.UseInMemoryDatabase("N5ExamDbInMemory");

                var dbContextOptions = builder.Options;
                var context = new N5ExamContext(dbContextOptions);
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.Permissions.AddRange(GetSamplePermissions());
                context.SaveChanges();

                return new PermissionRepository(context, mapper);
            }

        }

        private static List<PermissionType> GetSamplePermissionTypes()
        {
            return new List<PermissionType>
            {
                new() { Id = 1, Description = "Administrator" },
                new() { Id = 2, Description = "Supervisor" },
                new() { Id = 3, Description = "Contributor" },
                new() { Id = 4, Description = "Viewer" }
            };
        }

        private static List<Permission> GetSamplePermissions()
        {
            var output = new List<Permission>
            {
                new()
                {
                    Id = 1,
                    EmployeeForename = "Leonel",
                    EmployeeSurname = "Chavez",
                    PermissionTypeId = 1,
                    PermissionDate = DateTime.Now
                },
                new ()
                {
                    Id = 2,
                    EmployeeForename = "Frank",
                    EmployeeSurname = "Smith",
                    PermissionTypeId = 1,
                    PermissionDate = DateTime.Now
                },
                new ()
                {
                    Id = 3,
                    EmployeeForename = "Mary",
                    EmployeeSurname = "Stone",
                    PermissionTypeId = 2,
                    PermissionDate = DateTime.Now
                },
                new ()
                {
                    Id = 4,
                    EmployeeForename = "Rachel",
                    EmployeeSurname = "Lite",
                    PermissionTypeId = 4,
                    PermissionDate = DateTime.Now
                },
            };

            return output;
        }
    }
}
