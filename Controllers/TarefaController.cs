using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Models;
using System.Diagnostics;

namespace API.Controllers;

[Route("api/tarefa")]
[ApiController]
public class TarefaController : ControllerBase
{
    private readonly AppDataContext _context;

    public TarefaController(AppDataContext context) =>
        _context = context;

    // GET: api/tarefa/listar
    [HttpGet]
    [Route("listar")]
    public IActionResult Listar()
    {
        try
        {
            List<Tarefa> tarefas = _context.Tarefas.Include(x => x.Categoria).ToList();
            return Ok(tarefas);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    
    [HttpGet]
    [Route("concluida")]

    public IActionResult concluido()
    {
        try{

         List<Tarefa> tarefas = _context.Tarefas.Where( x => x.status == "concluido").Include( x => x.Categoria).ToList();
         return Ok(tarefas);
        }catch(Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet]
    [Route("naoConcluido")]

    public IActionResult naoConcluidas()
    {
        try{

         List<Tarefa> tarefas = _context.Tarefas.Where( x => x.status != "concluido").Include( x => x.Categoria).ToList();
         return Ok(tarefas);
        }catch(Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    // POST: api/tarefa/cadastrar
    [HttpPost]
    [Route("cadastrar")]
    public IActionResult Cadastrar([FromBody] Tarefa tarefa)
    {
        try
        {
            Categoria? categoria = _context.Categorias.Find(tarefa.CategoriaId);
            if (categoria == null)
            {
                return NotFound();
            }
            tarefa.Categoria = categoria;
            _context.Tarefas.Add(tarefa);
            _context.SaveChanges();
            return Created("", tarefa);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

     [HttpPatch]
    [Route("atualizer/{id}")]
    public IActionResult AtualizarUnidade(int id, [FromBody] Tarefa tarefaat )
    {
        try
        {

            Tarefa? tarefaEncontrada = _context.Tarefas.FirstOrDefault(x => x.TarefaId == id);


    tarefaEncontrada.Titulo = tarefaat.Titulo;
    tarefaEncontrada.Descricao = tarefaat.Descricao; 
    tarefaEncontrada.status = tarefaat.status;
    tarefaEncontrada.CriadoEm = tarefaat.CriadoEm;
   tarefaEncontrada.CategoriaId = tarefaEncontrada.CategoriaId;

   _context.Update(tarefaEncontrada);
   _context.SaveChanges();
   return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }   
}

