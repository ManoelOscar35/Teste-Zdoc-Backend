// Models/HistoricoAlteracao.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace GestaoPessoalApi.Models
{
	public class HistoricoAlteracao
	{
		public int Id { get; set; }

		[Required]
		public int FuncionarioId { get; set; }

		[Required, StringLength(100)]
		public string CampoAlterado { get; set; } = default!;

		[Required, StringLength(200)]
		public string ValorAnterior { get; set; } = default!;

		[Required, StringLength(200)]
		public string ValorNovo { get; set; } = default!;

		[Required]
		public DateTime DataHora { get; set; }
	}
}
