using ModCore.Models.Access;
using NUnit.Framework;
using System;
using ModCore.Services.Access;
using Moq;
using ModCore.Specifications.Access;
using ModCore.Abstraction.DataAccess;
using ModCore.Specifications.BuiltIns;
using System.Collections.Generic;

namespace ModCore.Services.Tests
{
    public class RoleServiceTest : BaseServiceTest<Role>
    {
        [Test]
        public void GetAllRolesAsync_Test()
        {
            var mapper = _mockMapper.Object;
            var logger = _mockLogger.Object;
            var siteSettings = _mockSiteSettings.Object;
            //var repos = _mockRepos
            //    .Setup(a => a.FindAllAsync(It.IsAny<ISpecification<Role>>()))
            //    .Returns(a => new Task<List<Role>>()
            //    {
            //        new Role(),
            //        new Role()
            //    });
           

            //var roleService = new RoleService();
        }


    }
}
