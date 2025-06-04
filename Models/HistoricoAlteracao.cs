using System;

namespace GestaoPessoalApi.Models
{
	public class HistoricoAlteracao
	{
		public int Id { get; set; }

		// chave estrangeira para Funcionario
		public int FuncionarioId { get; set; }

		// data e hora da alteração
		public DateTime DataHora { get; set; }

		// campo que sofreu alteração
		public string CampoAlterado { get; set; }

		// os valores antigo e novo
		public string ValorAnterior { get; set; }
		public string ValorNovo { get; set; }
	}
}
