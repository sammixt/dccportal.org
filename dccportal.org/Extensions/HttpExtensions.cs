using System.Text.Json;
using dccportal.org.Dto;
using dccportal.org.Helper;
//using Microsoft.AspNetCore.Http;

namespace dccportal.org.Extensions
{
    public static class HttpExtensions
    {
        public static void AddPaginationHeader(this HttpResponse response, int currentPage, 
            int itemsPerPage, int totalItems, int totalPages)
        {
            var paginationHeader = new PaginationHeader(currentPage, itemsPerPage, totalItems, totalPages);
            
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            
            response.Headers.Add("Pagination", JsonSerializer.Serialize(paginationHeader, options));
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
            
        }

        public static DataTableRequestDto GetDataTableRequestForm(this HttpRequest request){
            DataTableRequestDto dataTableRequestDto = new DataTableRequestDto();
             dataTableRequestDto.Draw = request.Form["draw"].FirstOrDefault();
            dataTableRequestDto.Start = request.Form["start"].FirstOrDefault();
            dataTableRequestDto.Length = request.Form["length"].FirstOrDefault();
            dataTableRequestDto.SortColumn = request.Form["columns[" + request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            dataTableRequestDto.SortColumnDirection = request.Form["order[0][dir]"].FirstOrDefault();
            dataTableRequestDto.SearchValue = request.Form["search[value]"].FirstOrDefault();
            return dataTableRequestDto;
        }

    }
}