using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace BlazorServerApp.Config
{
    public class BrowserDimension
    {
        public int Width { get; set; }
        public int Height { get; set; }
    }

    public class BrowserCallResponse
    {
        public bool SuccessFlag { get; set; }
        public string ErrorMessage { get; set; }
    }


    public class BrowserService
    {
        private readonly IJSRuntime _js;

        public BrowserService(IJSRuntime js)
        {
            _js = js;
        }

        public async Task<BrowserDimension> GetDimensions()
        {
            return await _js.InvokeAsync<BrowserDimension>("getDimensions");
        }


        public async Task<BrowserCallResponse> InitApp()
        {
            //await _js.InvokeVoidAsync("reloadPage");
            return await _js.InvokeAsync<BrowserCallResponse>("initAppScripts");
        }
    }
}