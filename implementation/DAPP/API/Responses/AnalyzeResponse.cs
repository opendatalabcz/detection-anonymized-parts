namespace API.Responses
{
    public class AnalyzeResponse
    {
        public string ContractName { get; internal set; } = default!;
        public string Url { get; internal set; } = default!;
        public bool ContainsAnonymizedData { get; internal set; }
        public float AnonymizedPercentage { get; internal set; }
        public int PageCount { get; internal set; }
        public Dictionary<int, float> AnonymizedPercentagePerPage { get; internal set; } = default!;
        public Dictionary<int, byte[]> OriginalImages { get; internal set; } = default!;
        public Dictionary<int, byte[]> ResultImages { get; internal set; } = default!;
    }
}
