using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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
		[StringLength(50)]
		public string Status { get; set; } = default!;

		// ← chave estrangeira para Funcionario. O cliente só envia esse campo no JSON.
		[Required]
		public int FuncionarioId { get; set; }

		// ← Navegação: não vamos vinculá-la via JSON no PUT/POST.
		//    O [JsonIgnore] evita que o model binder
		//    espere algo no campo "Funcionario" do JSON.
		[JsonIgnore]
		public Funcionario Funcionario { get; set; } = default!;
	}
}
