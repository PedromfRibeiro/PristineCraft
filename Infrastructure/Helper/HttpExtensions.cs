using Microsoft.AspNetCore.Http;
using PristineCraft.Application.Common.Models;
using System.Text.Json;

namespace Infrastructure.Helper;

public static class HttpExtensions
{
    public static void AddPaginationHeader(this HttpResponse response, int currentPage, int itemsPerPage, int totalItems, int totalPages)
    {
        var paginationHeader = new PaginationHeader(currentPage, itemsPerPage, totalItems, totalPages);
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        //TODO: Remove comments - fix error
        //response.Headers.Add("Pagination", JsonSerializer.Serialize(paginationHeader, options));
        //response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
    }
}