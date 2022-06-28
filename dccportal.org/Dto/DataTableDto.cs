using System.Text.Json;
using System.Text.Json.Serialization;

namespace dccportal.org.Dto
{
    public class DataTableDto<T> where T : class
    {
        
        [JsonPropertyName("draw")]
        public int Draw { get; set; }

        /// <summary>
        /// Total records, before filtering (i.e. the total number of records in the database)
        /// </summary>
        [JsonPropertyName("recordsTotal")]
        public int RecordsTotal { get; set; }

        /// <summary>
        /// Total records, after filtering (i.e. the total number of records after filtering has been applied - not just the number of records being returned for this page of data).
        /// </summary>
        [JsonPropertyName("recordsFiltered")]
        public int RecordsFiltered { get; set; }

        /// <summary>
        /// The data to be displayed in the table.
        /// This is an array of data source objects, one for each row, which will be used by DataTables.
        /// Note that this parameter's name can be changed using the ajaxDT option's dataSrc property.
        /// </summary>
        [JsonPropertyName("data")]
        public IEnumerable<T> Data { get; set; }
    }
}