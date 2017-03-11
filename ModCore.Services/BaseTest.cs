﻿using System;
using NUnit.Framework;
using NUnit.Framework.Internal;
using ModCore.Abstraction.DataAccess;
using ModCore.Abstraction.Site;
using ModCore.Models.BaseEntities;
using AutoMapper;
using Moq;

namespace ModCore.Services.Tests
{
    [TestFixture]
    public abstract class BaseTest<T> where T : BaseEntity
    {
        protected Mock<IDataRepositoryAsync<T>> _mockRepos;
        protected Mock<IMapper> _mockMapper;
        protected Mock<ILog> _mockLogger;
        protected Mock<ISiteSettingsManagerAsync> _mockSiteSettings;

        public BaseTest()
        {
            _mockRepos = new Mock<IDataRepositoryAsync<T>>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILog>();
            _mockSiteSettings = new Mock<ISiteSettingsManagerAsync>();
        }
    }
}
