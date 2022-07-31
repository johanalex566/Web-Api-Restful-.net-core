using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesAPI.Helpers
{
    public static class httpContextExtensions
    {
        public async static Task InsertRecordParameter<T>(this HttpContext httpContext, IQueryable<T> queryable, int recordsPerPage)
        {

            double quantity = await queryable.CountAsync();
            double quantityPages = Math.Ceiling(quantity / recordsPerPage);
            httpContext.Response.Headers.Add("QuantityPages", quantityPages.ToString());

        }

    }
}
