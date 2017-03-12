using ModCore.Models.Access;
using NUnit.Framework;
using System;
using ModCore.Services.Access;
using Moq;
using ModCore.Specifications.Access;
using ModCore.Abstraction.DataAccess;
using ModCore.Specifications.BuiltIns;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ModCore.Services.Tests
{
    public class RoleServiceTest : BaseServiceTest<Role>
    {
        [Test]
        public async Task GetAllRolesAsync_Test()
        {
            var mapper = _mockMapper.Object;
            var logger = _mockLogger.Object;
            var siteSettings = _mockSiteSettings.Object;
            _mockRepos
                .Setup(a => a.FindAllAsync(It.IsAny<ISpecification<Role>>()))
                .ReturnsAsync(new List<Role>()
                {
                    new Role(),
                    new Role()
                });

            var repos = _mockRepos.Object;

            var roleService = new RoleService(repos, mapper, logger, siteSettings);
            var result = await roleService.GetAllRolesAsync();

            Assert.IsTrue(result.Count == 2);
        }


    }
}
