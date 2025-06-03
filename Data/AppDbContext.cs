using Microsoft.EntityFrameworkCore;
using GestaoPessoalApi.Models;

namespace GestaoPessoalApi.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

		public DbSet<Funcionario> Funcionarios { get; set; } = default!;
		public DbSet<Ferias> Ferias { get; set; } = default!;

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// Caso queira explicitar, mas n�o � obrigat�rio se voc� respeitar conven��es:
			modelBuilder.Entity<Ferias>()
						.HasOne(f => f.Funcionario)
						.WithMany(func => func.Ferias)
						.HasForeignKey(f => f.FuncionarioId);
		}
	}
}
