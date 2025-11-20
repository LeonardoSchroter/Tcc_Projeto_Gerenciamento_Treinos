using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Projeto_gerencia_treinos_musculacao.Services // ou o namespace que preferir
{
    // Classe principal que representa toda a resposta da API
    public class GeminiResponse
    {
        [JsonPropertyName("candidates")]
        public List<Candidate> Candidates { get; set; }
    }

    // Representa uma das sugestões de resposta do modelo
    public class Candidate
    {
        [JsonPropertyName("content")]
        public Content Content { get; set; }
    }

    // Representa o bloco de conteúdo
    public class Content
    {
        [JsonPropertyName("parts")]
        public List<Part> Parts { get; set; }
    }

    // Representa a parte que contém o texto final 
    public class Part
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }
    }
}