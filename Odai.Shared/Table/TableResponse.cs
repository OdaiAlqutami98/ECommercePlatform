using Newtonsoft.Json;

namespace Odai.Shared.Table
{
    public class TableResponse<T>
    {
        [JsonProperty("recordsTotal")]
        public int RecordsTotal { get; set; }
        [JsonProperty("data")]
        public IEnumerable<T>? Data { get; set; }

        public TableResponse(IEnumerable<T> data, int recordsTotal)
        {
            Data = data;
            RecordsTotal = recordsTotal;
        }
    }
}
