using System;

namespace GestaoPessoalApi.Models
{
	public class HistoricoAlteracao
	{
		public int Id { get; set; }

		// chave estrangeira para Funcionario
		public int FuncionarioId { get; set; }

		// data e hora da altera��o
		public DateTime DataHora { get; set; }

		// campo que sofreu altera��o
		public string CampoAlterado { get; set; }

		// os valores antigo e novo
		public string ValorAnterior { get; set; }
		public string ValorNovo { get; set; }
	}
}
