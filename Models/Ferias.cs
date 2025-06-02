public class Ferias
{
	public int Id { get; set; }
	public DateTime DataInicio { get; set; }
	public DateTime DataTermino { get; set; }
	public required string Status { get; set; }
	public int FuncionarioId { get; set; }
	public Funcionario? Funcionario { get; set; } 
}