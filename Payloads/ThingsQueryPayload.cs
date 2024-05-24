
using System.Collections.Generic;

namespace StationeersWebApi.Payloads
{
    public class ThingsQueryPayload
    {
        public List<string> referenceIds { get; set; } = new();
        public List<string> prefabNames { get; set; } = new();
        public List<int> prefabHashes { get; set; } = new();
        public List<string> displayNames { get; set; } = new();

        public bool matchIntersection { get; set; } = false;
    }
}