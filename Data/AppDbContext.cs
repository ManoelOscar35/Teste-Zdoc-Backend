using Microsoft.EntityFrameworkCore;
using GestaoPessoalApi.Models; 

namespace GestaoPessoalApi.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

		public DbSet<Funcionario> Funcionarios { get; set; }
		public DbSet<Ferias> Ferias { get; set; }
		public DbSet<HistoricoAlteracao> Historicos { get; set; }

	}
}
