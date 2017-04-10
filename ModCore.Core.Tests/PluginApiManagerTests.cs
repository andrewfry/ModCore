using ModCore.Abstraction.PluginApi;
using ModCore.Core.PluginApi;
using ModCore.Models.PluginApi;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ModCore.Core.Tests
{
    public class PluginApiManagerTests : BaseTest
    {

        [Fact]
        public async Task BasicApiHandler()
        {

            IPluginApiManager apiManager = new PluginApiManager();
            Func<ApiArgument, Task<ApiHandlerResponse>> handler = async (arg) =>
            {
                return new ApiHandlerResponse()
                {
                    Success = true,
                    Value = new ExampleReturnObj
                    {
                        Message = "test"
                    }
                };
            };


            apiManager.RegisterApiRequestHander("exAmplerHandler", null, handler);

            var response = await apiManager.FullfilApiRequest("examplerhandler", null);

            Assert.True(response.Success == true);
            Assert.True(response.Value is ExampleReturnObj);
            Assert.True(response.HandledBy.Count == 1);

        }

        [Fact]
        public async Task BasicApiHandlerInSeperateThread()
        {

            IPluginApiManager apiManager = new PluginApiManager();
            Func<ApiArgument, Task<ApiHandlerResponse>> handler = async (arg) =>
            {
                return new ApiHandlerResponse()
                {
                    Success = true,
                    Value = new ExampleReturnObj
                    {
                        Message = "test"
                    }
                };
            };


            Task.Run(() =>
            {
                IPluginApiManager apiManager2 = new PluginApiManager();

                var response = apiManager2.FullfilApiRequest("examplerhandler", null).Result;

                Assert.True(response.Success == true);
                Assert.True(response.Value is ExampleReturnObj);
            });

            Task.Run(() =>
            {

                IPluginApiManager apiManager2 = new PluginApiManager();
                var response = apiManager2.FullfilApiRequest("examplerhandler", null).Result;

                Assert.True(response.Success == true);
                Assert.True(response.Value is ExampleReturnObj);
            });

        }

        [Fact]
        public async Task BasicApiTwoHandler()
        {

            IPluginApiManager apiManager = new PluginApiManager();
            Func<ApiArgument, Task<ApiHandlerResponse>> handler = async (arg) =>
            {
                return new ApiHandlerResponse()
                {
                    Success = true,
                    Value = new ExampleReturnObj
                    {
                        Message = "test"
                    }
                };
            };
            Func<ApiArgument, Task<ApiHandlerResponse>> handler2 = async (arg) =>
            {
                return new ApiHandlerResponse()
                {
                    Success = true,
                    Value = new ExampleReturnObj
                    {
                        Message = "test_handler_two"
                    }
                };
            };

            apiManager.RegisterApiRequestHander("exAmplerHandler", null, handler);
            apiManager.RegisterApiRequestHander("examplerhandler", null, handler2);


            var response = await apiManager.FullfilApiRequest("examplerhandler", null);

            Assert.True(response.Success == true);
            Assert.True(response.Value is IEnumerable<object>);
            foreach (var v in (response.Value as IEnumerable<object>))
            {
                Assert.True(v is ExampleReturnObj);
            }
            Assert.True(response.HandledBy.Count == 2);

        }

        [Fact]
        public async Task BasicApiTwoHandlerSingleExecution()
        {

            IPluginApiManager apiManager = new PluginApiManager();
            Func<ApiArgument, Task<ApiHandlerResponse>> handler = async (arg) =>
            {
                return new ApiHandlerResponse()
                {
                    Success = true,
                    Value = new ExampleReturnObj
                    {
                        Message = "test"
                    }
                };
            };
            Func<ApiArgument, Task<ApiHandlerResponse>> handler2 = async (arg) =>
            {
                return new ApiHandlerResponse()
                {
                    Success = true,
                    Value = new ExampleReturnObj
                    {
                        Message = "test_handler_two"
                    }
                };
            };

            apiManager.RegisterApiRequestHander("exAmplerHandler", null, handler);
            apiManager.RegisterApiRequestHander("examplerhandler", null, handler2);


            var response = await apiManager.FullfilApiRequest("examplerhandler", null, ApiExecutionType.Single);

            Assert.True(response.Success == false);
            Assert.True(response.Exception != null);
        }

        [Fact]
        public async Task BasicApiTwoHandlerFirstExecution()
        {

            IPluginApiManager apiManager = new PluginApiManager();
            Func<ApiArgument, Task<ApiHandlerResponse>> handler = async (arg) =>
            {
                return new ApiHandlerResponse()
                {
                    Success = true,
                    Value = new ExampleReturnObj
                    {
                        Message = "test"
                    }
                };
            };
            Func<ApiArgument, Task<ApiHandlerResponse>> handler2 = async (arg) =>
            {
                return new ApiHandlerResponse()
                {
                    Success = true,
                    Value = new ExampleReturnObj
                    {
                        Message = "test_handler_two"
                    }
                };
            };

            apiManager.RegisterApiRequestHander("exAmplerHandler", null, handler);
            apiManager.RegisterApiRequestHander("examplerhandler", null, handler2);


            var response = await apiManager.FullfilApiRequest("examplerhandler", null, ApiExecutionType.First);

            Assert.True(response.Success == true);
            Assert.True(response.Value is ExampleReturnObj);
            Assert.True(response.HandledBy.Count == 1);
        }
    }

    public class ExampleReturnObj
    {
        public string Message { get; set; }
    }
}
