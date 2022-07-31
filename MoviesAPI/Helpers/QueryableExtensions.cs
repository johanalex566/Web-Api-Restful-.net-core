﻿using MoviesAPI.DTOs;
using System.Linq;

namespace MoviesAPI.Helpers
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Paginate<T>(this IQueryable<T> queryable, PaginationDTO paginationDTO)
        {
            return queryable
                    .Skip((paginationDTO.Page - 1) * paginationDTO.NumbersOfRecordsPerPage)
                    .Take(paginationDTO.NumbersOfRecordsPerPage);
        }
    }
}
