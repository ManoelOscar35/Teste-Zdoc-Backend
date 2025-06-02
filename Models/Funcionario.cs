public class Funcionario
{
	public int Id { get; set; }
	public required string Nome { get; set; }
	public required string Cargo { get; set; }
	public DateTime DataAdmissao { get; set; }
	public decimal Salario { get; set; }
	public required string Status { get; set; }
	public ICollection<Ferias> Ferias { get; set; } = new List<Ferias>();

}
