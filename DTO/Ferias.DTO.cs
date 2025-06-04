namespace GestaoPessoalApi.Dtos
{
	public class FeriasDto
	{
		public int Id { get; set; }
		public DateTime DataInicio { get; set; }
		public DateTime DataTermino { get; set; }
		public string Status { get; set; } = default!;
		public int FuncionarioId { get; set; }

		// Opcional: se quiser retornar tamb�m o nome do funcion�rio na resposta de GET
		public string? NomeFuncionario { get; set; }
	}
}
