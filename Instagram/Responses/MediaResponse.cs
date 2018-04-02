using System.Collections.Generic;

namespace Instagram.Responses
{
    public class MediaResponse
    {
        public IEnumerable<MediaItemResponse> Data { get; set; }
    }
}
