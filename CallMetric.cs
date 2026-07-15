
namespace MafMetricsLog
{
    public record CallMetric(
    string Timestamp,
    string Question,
    int ResponseCharCount,
    int EstimatedWordCount,
    long ElapsedMs);
}
