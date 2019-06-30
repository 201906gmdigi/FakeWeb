using System.Threading.Tasks;
using CoreWebCommon;
using NLog;

namespace CoreService
{
    public class ApiService
    {
        private readonly IMyLogger _logger;

        public ApiService(IMyLogger logger)
        {
            _logger = logger;
        }

        public async Task<TResp> PostApi<TReq, TResp>(TReq req)
        {
            return await Task.FromResult(default(TResp));
        }
    }
}