﻿
namespace Shared.Filters
{
    public class PaginationFilter
    {
        const int _maxPageSize = 150;

        private int _pageSize = _maxPageSize;
        private int _pageNumber = 1;

        private int MaxPageSize { get; set; } = _maxPageSize;

        public int PageNumber
        {
            get { return _pageNumber; }
            set { _pageNumber = (value < 1) ? 1 : value; }
        }

        public int PageSize
        {
            get { return _pageSize > MaxPageSize ? MaxPageSize : _pageSize; }
            set { _pageSize = (value > _maxPageSize || value < 1) ? _maxPageSize : value; }
        }

        public string SortBy {  get; set; }
    }
}
