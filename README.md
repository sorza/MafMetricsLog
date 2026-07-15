# MafMetricsLog

Middleware para **Microsoft Agent Framework (MAF)** que intercepta cada resposta do modelo (ex.: Ollama) antes mesmo de passar pelo pipeline do agente, registrando métricas estruturadas para análise e monitoramento.

## Visão Geral

O **MafMetricsLog** foi criado para fornecer **observabilidade** em agentes MAF.  
Ele captura e armazena métricas sobre cada resposta gerada, permitindo avaliar desempenho, qualidade e comportamento dos agentes em produção.

## Funcionalidades

-  **Coleta de métricas estruturadas**: tempo de resposta, tamanho da saída, tokens utilizados, entre outros.
-  **Interceptação antes do pipeline**: garante que todas as respostas sejam registradas antes de qualquer processamento adicional.
-  **Logs organizados**: saída em formato estruturado para fácil integração com sistemas de monitoramento.
-  **Compatibilidade com Ollama e MAF**: pronto para uso em pipelines de agentes que utilizam esses frameworks.

// Exemplo de log estruturado
{
  "timestamp": "2026-07-14T22:38:00",
  "agent": "SupportAgent",
  "responseTokens": 128,
  "latencyMs": 450,
  "outputLength": 1024
}
