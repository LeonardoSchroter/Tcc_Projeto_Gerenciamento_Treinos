namespace Projeto_gerencia_treinos_musculacao.ViewModels
{
    public class ItemTreinoViewModel
    {
        public string ExercicioId { get; set; }
        public string? ExercicioNome { get; set; } // Para exibir o nome na tela
        public string Series { get; set; }
        public string Repeticoes { get; set; }
        public string? Carga { get; set; }
        public string? Descanso { get; set; }
        public int Ordem { get; set; }
    }
}