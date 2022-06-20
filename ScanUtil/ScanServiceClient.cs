using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ScanUtil.Dto;

namespace ScanUtil
{
    public class ScanServiceClient
    {
        private static readonly HttpClient Client = new()
        {
            BaseAddress = new Uri("http://localhost:5000/")
        };

        private async Task<T> HandleResponse<T>(HttpResponseMessage response)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                {
                    var result = await response.Content.ReadFromJsonAsync<T>();
                    return result;
                }
                case HttpStatusCode.BadRequest:
                {
                    var errorResponse = await response.Content.ReadFromJsonAsync<ExceptionDto>();
                    throw new HttpRequestException(errorResponse?.Error);
                }
                default:
                {
                    throw new Exception(response.Content.ToString());
                }
            }
        }

        public async Task<ScanTaskDto> StartScan(string directoryPath)
        {
            var requestBody = new ScanTaskCreateDto {DirectoryPath = directoryPath};
            
            var response = await Client.PostAsJsonAsync("DirectoryScanner", requestBody);

            return await HandleResponse<ScanTaskDto>(response);
        }

        public async Task<ScanResultDto> GetStatus(int taskId)
        {
            var response = await Client.GetAsync($"DirectoryScanner/{taskId}");

            return await HandleResponse<ScanResultDto>(response);
        }
    }
}