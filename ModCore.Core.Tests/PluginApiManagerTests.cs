using ModCore.Abstraction.PluginApi;
using ModCore.Core.PluginApi;
using ModCore.Models.PluginApi;
using System;
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


            apiManager.RegisterApiHander("exAmplerHandler", null, handler);

            var response = await apiManager.FullfilEventApiRequest("examplerhandler", null);

            Assert.True(response.Success == true);
            Assert.True(response.Value is ExampleReturnObj);


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


            Task.Run(() => {
                IPluginApiManager apiManager2 = new PluginApiManager();

                var response =  apiManager2.FullfilEventApiRequest("examplerhandler", null).Result;

                Assert.True(response.Success == true);
                Assert.True(response.Value is ExampleReturnObj);
            });

            Task.Run(() => {

                IPluginApiManager apiManager2 = new PluginApiManager();
                var response =  apiManager2.FullfilEventApiRequest("examplerhandler", null).Result;

                Assert.True(response.Success == true);
                Assert.True(response.Value is ExampleReturnObj);
            });

        }

    }

    public class ExampleReturnObj
    {
        public string Message { get; set; }
    }
}

