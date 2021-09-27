using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Innovare.Shop.Business.Abstractions.Dto
{
    public abstract class FilterDtoBase : DtoBase
    {
        public FilterDtoBase()
        {
            PageNumber = 1;
            PageSize = 10;
        }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }
    }
}