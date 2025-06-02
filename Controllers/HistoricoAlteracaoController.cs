using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestaoPessoalApi.Data;
using GestaoPessoalApi.Models;

namespace GestaoPessoalApi.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class HistoricoAlteracaoController : ControllerBase
	{
		private readonly AppDbContext _context;

		public HistoricoAlteracaoController(AppDbContext context)
		{
			_context = context;
		}

		// GET: api/historicoalteracao?funcionarioId=1
		[HttpGet]
		public async Task<ActionResult<IEnumerable<HistoricoAlteracao>>> GetHistorico([FromQuery] int funcionarioId)
		{
			return await _context.Historicos
								 .Where(h => h.FuncionarioId == funcionarioId)
								 .OrderByDescending(h => h.DataHora)
								 .ToListAsync();
		}
	}
}
