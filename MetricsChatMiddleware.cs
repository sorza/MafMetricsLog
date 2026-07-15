using Microsoft.Extensions.AI;
using System.Diagnostics;
using System.Text.Json;

namespace MafMetricsLog
{
    public class MetricsChatMiddleware(IChatClient innerClient, string logPath)
    : DelegatingChatClient(innerClient)
    {
        private readonly string _logPath = logPath;

        public override async Task<ChatResponse> GetResponseAsync(
            IEnumerable<ChatMessage> messages,
            ChatOptions? options = null,
            CancellationToken cancellationToken = default)
        {
            var question = messages.LastOrDefault()?.Text ?? string.Empty;
            var sw = Stopwatch.StartNew();

            // Delega a chamada ao cliente interno (Ollama) e mede o tempo
            var response = await base.GetResponseAsync(messages, options, cancellationToken);

            sw.Stop();

            var responseText = response.Messages.LastOrDefault()?.Text ?? string.Empty;
            var wordCount = responseText.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;

            var metric = new CallMetric(
                Timestamp: DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                Question: question,
                ResponseCharCount: responseText.Length,
                EstimatedWordCount: wordCount,
                ElapsedMs: sw.ElapsedMilliseconds
            );

            // Serializa e grava a métrica como linha JSON no arquivo de log
            var json = JsonSerializer.Serialize(metric);
            await File.AppendAllTextAsync(_logPath, json + Environment.NewLine, cancellationToken);

            Console.WriteLine($"[METRICS] Resposta em {sw.ElapsedMilliseconds}ms | {wordCount} palavras | {responseText.Length} chars");

            return response;
        }
    }
}
