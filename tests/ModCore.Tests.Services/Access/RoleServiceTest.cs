using ModCore.Models.Access;
using System;
using ModCore.Services.Access;
using Moq;
using ModCore.Specifications.Access;
using ModCore.Abstraction.DataAccess;
using ModCore.Specifications.BuiltIns;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ModCore.Tests.Services
{
    public class RoleServiceTest : BaseServiceTest<Role>
    {
        [Fact]
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

            Assert.True(result.Count == 2);
        }


    }
}
