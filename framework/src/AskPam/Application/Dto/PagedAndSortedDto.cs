﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskPam.Application.Dto
{
    public class PagedAndSortedDto
    {
        public int MaxResultCount { get; set; }
        public int SkipCount { get; set; }
        public string Sorting { get; set; }
    }
}
