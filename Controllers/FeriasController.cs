using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestaoPessoalApi.Data;
using GestaoPessoalApi.Models;

namespace GestaoPessoalApi.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class FeriasController : ControllerBase
	{
		private readonly AppDbContext _context;

		public FeriasController(AppDbContext context)
		{
			_context = context;
		}

		// GET: api/ferias
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Ferias>>> GetFerias()
		{
			// Inclui o funcionário na resposta para mostrar nome, cargo etc., se necessário
			return await _context.Ferias
								 .Include(f => f.Funcionario)
								 .ToListAsync();
		}

		// GET: api/ferias/{id}
		[HttpGet("{id}")]
		public async Task<ActionResult<Ferias>> GetFeriasById(int id)
		{
			var ferias = await _context.Ferias
									   .Include(f => f.Funcionario)
									   .FirstOrDefaultAsync(f => f.Id == id);

			if (ferias == null)
				return NotFound();

			return ferias;
		}

		// POST: api/ferias
		[HttpPost]
		public async Task<ActionResult<Ferias>> PostFerias(Ferias ferias)
		{
			// Validação simplificada: certifique-se de que FuncionarioId existe
			var existeFunc = await _context.Funcionarios.AnyAsync(x => x.Id == ferias.FuncionarioId);
			if (!existeFunc)
				ModelState.AddModelError(nameof(ferias.FuncionarioId), "FuncionarioId inválido.");

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			// Calcula o status automaticamente
			ferias.Status = CalcularStatus(ferias.DataInicio, ferias.DataTermino);

			_context.Ferias.Add(ferias);
			await _context.SaveChangesAsync();

			// Retorna com o objeto criado, incluindo a navegação de Funcionario
			await _context.Entry(ferias).Reference(f => f.Funcionario).LoadAsync();
			return CreatedAtAction(nameof(GetFeriasById), new { id = ferias.Id }, ferias);
		}

		// PUT: api/ferias/{id}
		[HttpPut("{id}")]
		public async Task<IActionResult> PutFerias(int id, Ferias ferias)
		{
			if (id != ferias.Id)
				return BadRequest();

			// Valida se o funcionário existe
			var existeFunc = await _context.Funcionarios.AnyAsync(x => x.Id == ferias.FuncionarioId);
			if (!existeFunc)
				ModelState.AddModelError(nameof(ferias.FuncionarioId), "FuncionarioId inválido.");

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			// Atualiza status antes de salvar
			ferias.Status = CalcularStatus(ferias.DataInicio, ferias.DataTermino);

			_context.Entry(ferias).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!await _context.Ferias.AnyAsync(f => f.Id == id))
					return NotFound();

				throw;
			}

			return NoContent();
		}

		// DELETE: api/ferias/{id}
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteFerias(int id)
		{
			var ferias = await _context.Ferias.FindAsync(id);
			if (ferias == null)
				return NotFound();

			_context.Ferias.Remove(ferias);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		// Método auxiliar para calcular status das férias
		private string CalcularStatus(DateTime dataInicio, DateTime dataTermino)
		{
			var hoje = DateTime.Today;

			if (dataInicio > hoje)
				return "Pendente";
			else if (hoje >= dataInicio && hoje <= dataTermino)
				return "Em andamento";
			else
				return "Concluídas";
		}
	}
}
