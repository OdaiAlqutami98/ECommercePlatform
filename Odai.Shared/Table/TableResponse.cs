using Newtonsoft.Json;

namespace Odai.Shared.Table
{
    public class TableResponse<T>
    {

        [JsonProperty("recordsTotal")]
        public int RecordsTotal { get; set; }

        [JsonProperty("recordsFiltered")]
        public int RecordsFiltered { get; set; }
        [JsonProperty("data")]
        public IEnumerable<T>? Data { get; set; }
    }
}
