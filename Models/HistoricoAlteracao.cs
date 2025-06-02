using System;

namespace GestaoPessoalApi.Models
{
	public class HistoricoAlteracao
	{
		public int Id { get; set; }
		public DateTime DataHora { get; set; }
		public required string CampoAlterado { get; set; }
		public required string ValorAntigo { get; set; }
		public required string ValorNovo { get; set; }
		public int FuncionarioId { get; set; }
		public Funcionario? Funcionario { get; set; }
	}
}
