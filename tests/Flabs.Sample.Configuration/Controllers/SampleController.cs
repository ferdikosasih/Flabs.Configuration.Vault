using Microsoft.AspNetCore.Mvc;
using Flabs.Configuration.VaultSharp.Extensions;

namespace Flabs.Sample.Configuration.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SampleController : ControllerBase
    {
        private readonly IServiceProvider _serviceProvider;
        public SampleController(IServiceProvider serviceProvider)
        {

            _serviceProvider = serviceProvider;
        }

        [HttpGet(Name = "Sample")]
        public string Get()
        {
            SampleOptions sampleOpt = _serviceProvider.GetConfig<SampleOptions>();
            return sampleOpt.SampleValue;
        }
    }
}
