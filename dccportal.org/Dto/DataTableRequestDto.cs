using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dccportal.org.Dto
{
    public class DataTableRequestDto
    {
        public DataTableRequestDto()
        {
        }
        public string Draw {get; set;}
        public string Start {get; set;}
        public string Length {get; set;}
        public string  SortColumn {get; set;}
        public string  SortColumnDirection {get; set;}
        public string  SearchValue {get; set;}
        public int PageSize { get {
            return Length != null ? Convert.ToInt32(Length) : 0;
        }}
        public int  Skip {
            get{
                return Start != null ? Convert.ToInt32(Start) : 0;
            }
        }
       
    }
}