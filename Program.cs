using MafMetricsLog;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using OllamaSharp;
using System.Text.Json;

var logPath = "metrics.jsonl";

IChatClient ollama = new OllamaApiClient(new Uri("http://localhost:11434"), "llama3.2");
IChatClient clientWithMetrics = new MetricsChatMiddleware(ollama, logPath);

var agent = clientWithMetrics
    .AsAIAgent()
    .AsBuilder()
    .Build();

// Três perguntas com complexidade variada para gerar dados distintos no log
var perguntas = new[]
{
    "O que é uma struct em C#? Responda em uma frase.",
    "Explique o padrão Repository no contexto de Clean Architecture.",
    "Quais são as diferenças entre Task e ValueTask em .NET?"
};

foreach (var pergunta in perguntas)
{
    Console.WriteLine($"\nPergunta: {pergunta}");
    var r = await agent.RunAsync(pergunta);
    Console.WriteLine($"Resposta: {r.Messages.Last().Text}");
}

// Lê e exibe o resumo das métricas coletadas
Console.WriteLine("\n=== Resumo das métricas ===");
var linhas = await File.ReadAllLinesAsync(logPath);

foreach (var linha in linhas)
{
    var m = JsonSerializer.Deserialize<CallMetric>(linha);
    if (m is not null)
        Console.WriteLine($"[{m.Timestamp}] {m.ElapsedMs}ms | {m.EstimatedWordCount} palavras | Pergunta: {m.Question[..Math.Min(40, m.Question.Length)]}...");
}