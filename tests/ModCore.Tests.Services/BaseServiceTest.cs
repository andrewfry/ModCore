using System;
using ModCore.Abstraction.DataAccess;
using ModCore.Abstraction.Site;
using ModCore.Models.BaseEntities;
using AutoMapper;
using Moq;

namespace ModCore.Tests.Services
{
    public abstract class BaseServiceTest<T> where T : BaseEntity
    {
        protected Mock<IDataRepositoryAsync<T>> _mockRepos;
        protected Mock<IMapper> _mockMapper;
        protected Mock<ILog> _mockLogger;
        protected Mock<ISiteSettingsManagerAsync> _mockSiteSettings;

        public BaseServiceTest()
        {
            _mockRepos = new Mock<IDataRepositoryAsync<T>>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILog>();
            _mockSiteSettings = new Mock<ISiteSettingsManagerAsync>();
        }
    }
}
