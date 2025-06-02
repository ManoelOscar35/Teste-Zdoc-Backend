public class Funcionario
{
	public int Id { get; set; }
	public string Nome { get; set; }
	public string Cargo { get; set; }
	public DateTime DataAdmissao { get; set; }
	public decimal Salario { get; set; }
	public string Status { get; set; }
	public ICollection<Ferias> Ferias { get; set; }
}
