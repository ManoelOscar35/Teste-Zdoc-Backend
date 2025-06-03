using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestaoPessoalApi.Models
{
	public class Ferias
	{
		public int Id { get; set; }

		[Required]
		public DateTime DataInicio { get; set; }

		[Required]
		public DateTime DataTermino { get; set; }

		[Required]
		public string Status { get; set; } = default!;

		// -----------------------
		// Aqui está a “chave estrangeira”
		// -----------------------
		[ForeignKey("Funcionario")]
		public int FuncionarioId { get; set; }

		// Navegação para o Funcionario correspondente
		public Funcionario Funcionario { get; set; } = default!;
	}
}
