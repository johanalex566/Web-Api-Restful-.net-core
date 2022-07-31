using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesAPI.DTOs
{
    public class PaginationDTO
    {
        public int Page { get; set; } = 1;
        private int numbersOfRecordsPerPage = 10;
        private readonly int MaxRecords = 50;
        public int NumbersOfRecordsPerPage
        {
            get => numbersOfRecordsPerPage;
            set
            {
                numbersOfRecordsPerPage = (value > MaxRecords) ? MaxRecords : value;
            }
        }
    }
}
