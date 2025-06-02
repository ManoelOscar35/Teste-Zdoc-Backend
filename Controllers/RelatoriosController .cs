using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using GestaoPessoalApi.Data;
using GestaoPessoalApi.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace GestaoPessoalApi.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class RelatoriosController : ControllerBase
	{
		private readonly AppDbContext _context;

		public RelatoriosController(AppDbContext context)
		{
			_context = context;
		}

		// GET: api/relatorios/pdf
		[HttpGet("pdf")]
		public async Task<IActionResult> GerarRelatorioPdf()
		{
			var funcionarios = await _context.Funcionarios.ToListAsync();

			var pdf = GerarPdf(funcionarios);

			return File(pdf, "application/pdf", "RelatorioFuncionarios.pdf");
		}

		private byte[] GerarPdf(List<Funcionario> funcionarios)
		{
			var document = Document.Create(container =>
			{
				container.Page(page =>
				{
					page.Size(PageSizes.A4);
					page.Margin(20);
					page.PageColor(Colors.White);
					page.DefaultTextStyle(x => x.FontSize(12));

					page.Header()
						.Text("Relatório de Funcionários")
						.FontSize(18)
						.Bold()
						.AlignCenter();

					page.Content()
						.Table(table =>
						{
							table.ColumnsDefinition(columns =>
							{
								columns.ConstantColumn(30); // ID
								columns.RelativeColumn(2);  // Nome
								columns.RelativeColumn(2);  // Cargo
								columns.RelativeColumn(1);  // Salário
								columns.RelativeColumn(1);  // Status
							});

							table.Header(header =>
							{
								header.Cell().Text("ID").Bold();
								header.Cell().Text("Nome").Bold();
								header.Cell().Text("Cargo").Bold();
								header.Cell().Text("Salário").Bold();
								header.Cell().Text("Status").Bold();
							});

							foreach (var f in funcionarios)
							{
								table.Cell().Text(f.Id.ToString());
								table.Cell().Text(f.Nome);
								table.Cell().Text(f.Cargo);
								table.Cell().Text(f.Salario.ToString("C"));
								table.Cell().Text(f.Status);
							}
						});
				});
			});

			return document.GeneratePdf();
		}
	}
}
