using System;

namespace GestaoPessoalApi.Models
{
	public class HistoricoAlteracao
	{
		public int Id { get; set; }
		public DateTime DataHora { get; set; }
		public string CampoAlterado { get; set; }
		public string ValorAntigo { get; set; }
		public string ValorNovo { get; set; }
		public int FuncionarioId { get; set; }
		public Funcionario Funcionario { get; set; }
	}
}
