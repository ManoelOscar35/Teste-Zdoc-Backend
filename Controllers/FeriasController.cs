using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestaoPessoalApi.Data;
using GestaoPessoalApi.Dtos;
using GestaoPessoalApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

		// GET: api/Ferias
		[HttpGet]
		public async Task<ActionResult<IEnumerable<FeriasDto>>> GetAll()
		{
			// 1) Inclui o Funcionario para puxar o Nome
			var listaEntidades = await _context.Ferias
											   .Include(f => f.Funcionario)
											   .ToListAsync();

			// 2) Mapeia cada entidade para o DTO, preenchendo NomeFuncionario
			var listaDto = listaEntidades.Select(f => new FeriasDto
			{
				Id = f.Id,
				DataInicio = f.DataInicio,
				DataTermino = f.DataTermino,
				Status = f.Status,
				FuncionarioId = f.FuncionarioId,
				NomeFuncionario = f.Funcionario?.Nome ?? string.Empty
			})
			.ToList();

			return Ok(listaDto);
		}

		// GET: api/Ferias/{id}
		[HttpGet("{id:int}")]
		public async Task<ActionResult<FeriasDto>> GetById(int id)
		{
			var f = await _context.Ferias
								  .Include(x => x.Funcionario)
								  .FirstOrDefaultAsync(x => x.Id == id);

			if (f == null)
				return NotFound();

			var dto = new FeriasDto
			{
				Id = f.Id,
				DataInicio = f.DataInicio,
				DataTermino = f.DataTermino,
				Status = f.Status,
				FuncionarioId = f.FuncionarioId,
				NomeFuncionario = f.Funcionario?.Nome ?? string.Empty
			};

			return Ok(dto);
		}

		// POST: api/Ferias
		[HttpPost]
		public async Task<ActionResult<FeriasDto>> Create([FromBody] FeriasDto dto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			// Verifica se o Funcionario existe
			bool existeFunc = await _context.Funcionarios
											.AnyAsync(f => f.Id == dto.FuncionarioId);
			if (!existeFunc)
				return BadRequest($"FuncionárioId {dto.FuncionarioId} não encontrado.");

			var nova = new Ferias
			{
				DataInicio = dto.DataInicio,
				DataTermino = dto.DataTermino,
				Status = dto.Status,
				FuncionarioId = dto.FuncionarioId
			};

			_context.Ferias.Add(nova);
			await _context.SaveChangesAsync();

			// Carrega o nome do funcionário para retornar no DTO
			await _context.Entry(nova).Reference(f => f.Funcionario).LoadAsync();

			var dtoRet = new FeriasDto
			{
				Id = nova.Id,
				DataInicio = nova.DataInicio,
				DataTermino = nova.DataTermino,
				Status = nova.Status,
				FuncionarioId = nova.FuncionarioId,
				NomeFuncionario = nova.Funcionario?.Nome ?? string.Empty
			};

			return CreatedAtAction(nameof(GetById), new { id = dtoRet.Id }, dtoRet);
		}

		// PUT: api/Ferias/{id}
		[HttpPut("{id:int}")]
		public async Task<IActionResult> Update(int id, [FromBody] FeriasDto dto)
		{
			if (id != dto.Id)
				return BadRequest("ID da URL e do body não batem.");

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var existente = await _context.Ferias.FindAsync(id);
			if (existente == null)
				return NotFound();

			bool existeFunc = await _context.Funcionarios
											.AnyAsync(f => f.Id == dto.FuncionarioId);
			if (!existeFunc)
				return BadRequest($"FuncionárioId {dto.FuncionarioId} não encontrado.");

			// Atualiza apenas campos permitidos
			existente.DataInicio = dto.DataInicio;
			existente.DataTermino = dto.DataTermino;
			existente.Status = dto.Status;
			existente.FuncionarioId = dto.FuncionarioId;

			_context.Entry(existente).State = EntityState.Modified;
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!FeriasExists(id))
					return NotFound();
				throw;
			}

			return NoContent();
		}

		// DELETE: api/Ferias/{id}
		[HttpDelete("{id:int}")]
		public async Task<IActionResult> Delete(int id)
		{
			var f = await _context.Ferias.FindAsync(id);
			if (f == null)
				return NotFound();

			_context.Ferias.Remove(f);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		private bool FeriasExists(int id)
		{
			return _context.Ferias.Any(e => e.Id == id);
		}
	}
}
