using System;

namespace GetFeedBack.Models
{
    public class Pager
    {
        public string SearchText { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }


        public int TotalItems { get;  set; }
        public int CurrentPage { get;  set; }
        public int PageSize { get;  set; }
        public int Count { get;  set; }
        public int TotalPages { get;  set; }
        public int StartPage { get; set; }
        public int EndPage { get; set; }
        public int IndexOne { get; set; }
        public int IndexTwo { get; set;  }
        public bool ShowPrevious => CurrentPage > 1; 
        public bool ShowFrist => CurrentPage != 1;
        public bool ShowLast => CurrentPage != TotalPages;
        public Pager()
        {

        }
        public Pager(int totalItems, int? page, int pageSize = 5)
        {
            // Calculate total, start and end pages
            var totalPages = (int)Math.Ceiling((decimal)totalItems / (decimal)pageSize);
            var currentPage = page != null ? (int)page : 1;
            var startPage = currentPage - 5;
            var endPage = currentPage + 4;
            if (startPage <= 0)
            {
                endPage -= (startPage - 1);
                startPage = 1;
            }
            if (endPage > totalPages)
            {
                endPage = totalPages;
                if (endPage > 10)
                {
                    startPage = endPage - 9;
                }
            }

            TotalItems = totalItems;
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalPages = totalPages;
            StartPage = startPage;
            EndPage = endPage;
        }

    }
}
