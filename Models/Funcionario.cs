using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GestaoPessoalApi.Models
{
	public class Funcionario
	{
		public int Id { get; set; }

		[Required]
		public string Nome { get; set; } = default!;

		[Required]
		public string Cargo { get; set; } = default!;

		public DateTime DataAdmissao { get; set; }

		public decimal Salario { get; set; }

		[Required]
		public string Status { get; set; } = default!;

		// Navegação para as férias (1 Funcionario → N Ferias)
		public ICollection<Ferias> Ferias { get; set; } = new List<Ferias>();
	}
}
