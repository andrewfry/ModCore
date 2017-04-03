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
    public class UserServiceTest : BaseServiceTest<User>
    {
        [Fact]
        public async Task GetByEmailAsync_Test()
        {
            var mapper = _mockMapper.Object;
            var logger = _mockLogger.Object;
            var siteSettings = _mockSiteSettings.Object;
            _mockRepos
                .Setup(a => a.FindAsync(It.IsAny<GetByEmail>()))
                .ReturnsAsync(new User
                {
                     EmailAddress = "test@test.com"
                });

            var repos = _mockRepos.Object;

            var service = new UserService(repos, mapper, logger, siteSettings);
            var result = await service.GetByEmailAsync("test@test.com");

            Assert.True(result.EmailAddress == "test@test.com");
        }


    }
}
