using Microsoft.Extensions.Hosting;
using Projeto_gerencia_treinos_musculacao.Models;
using System.Reflection;
using System.Security.Policy;
using System.Text.Json;

namespace Projeto_gerencia_treinos_musculacao.Services
{
    public class GeminiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public GeminiService(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _apiKey = configuration["Gemini:ApiKey"];

            if (string.IsNullOrEmpty(_apiKey))
            {
                throw new InvalidOperationException("A ApiKey do Gemini não foi configurada.");
            }
        }

        public async Task<string> GerarFichaDeTreinoAsync(Anamnese anamnese, List<Exercicio> exercicios, string instrucoes)
        {
            var prompt = CriarPromptEstruturado(anamnese, exercicios, instrucoes);

            var apiUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent";
    
            var requestBody = new { contents = new[] { new { parts = new[] { new { text = prompt } } } } };

            var request = new HttpRequestMessage(HttpMethod.Post, apiUrl);

            request.Headers.Add("X-goog-api-key", _apiKey);

            request.Content = JsonContent.Create(requestBody);

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var geminiResponse = await response.Content.ReadFromJsonAsync<GeminiResponse>();
                var cleanJson = geminiResponse.Candidates[0].Content.Parts[0].Text
                    .Trim('`', '\n', '\r', ' ')
                    .Replace("json", "", StringComparison.OrdinalIgnoreCase);

                return cleanJson;
            }
            else
            {
               
                return null;
            }
        }

        private string CriarPromptEstruturado(Anamnese anamnese, List<Exercicio> exercicios, string instrucoes)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Projeto_gerencia_treinos_musculacao.Prompts.FichaDeTreinoPrompt.txt";
            string promptTemplate = "";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                promptTemplate = reader.ReadToEnd();
            }

            // Preenche os placeholders
            promptTemplate = promptTemplate.Replace("{objetivo}", anamnese.Objetivos ?? "Não informado");
            promptTemplate = promptTemplate.Replace("{lesoes}", anamnese.Lesoes ?? "Nenhuma");
            promptTemplate = promptTemplate.Replace("{doencas}", anamnese.Doencas ?? "Nenhuma");
            promptTemplate = promptTemplate.Replace("{atividade_fisica}", anamnese.AtividadeFisicaAtual ?? "Não informado");
            promptTemplate = promptTemplate.Replace("{instrucoes}", instrucoes ?? "Seguir o objetivo principal");

            // Serializa a lista de exercícios para o formato JSON
            var exerciciosJson = JsonSerializer.Serialize(exercicios.Select(e => new { e.Id, e.Nome, e.GrupoMuscular }));
            promptTemplate = promptTemplate.Replace("{lista_exercicios}", exerciciosJson);

            return promptTemplate;
        }
    }
}
