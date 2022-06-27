using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using WebAPI.Data;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Endpoint se refere a URl.
/// ControllerBase já traz tudo pré configurado para criar o controller do aspnet mvc, tanto pra blazor, razor, webApi.
/// Se não preencher o route permanece a mesma rota. https://localhost:7137/categories  
/// </summary>
[Route("categories")]
public class CategoryController : ControllerBase
{
    /// <summary>
    /// Utilizando a Task modo async cria threads paralelas para não parar a execução.
    /// Nessa rota ele devolve uma lista de categorias.
    /// </summary>
    [HttpGet]
    [Route("")]
    public async Task<ActionResult<List<Category>>> Get() 
    {
        return new List<Category>();
    }


    /// <summary>
    /// Nessa rota ele devolve uma categoria especifica baseada no seu id.
    /// </summary>
    /// <param name="id">Utilizando id:int na rota, se restringe o parâmetro a ser somente do tipo dele.</param>
    /// <returns></returns>
    [HttpGet]
    [Route("{id:int}")] 
    public async Task<ActionResult<Category>> GetById(int id)
    {
        return new Category();
    }


    /// <summary>
    /// Frombody está pegando o que vem no corpo da requisição, o Ok() está convertendo para um actionResult, comparando com o que está na minha categoria
    /// O ModelState verifica se está dentro do modelo necessário e validando se está cumprindo as especificações da classe
    /// FromService mostra que o DataContext está sendo gerenciado por serviços pela injeção de dependencia.
    /// Ele vai gerar o Id automaticamente usando o método do execute scalar do sql.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("")] 
    public async Task<ActionResult<List<Category>>> Post(
        [FromBody]Category model,
        [FromServices] DataContext context
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            context.Categories.Add(model);
            //Para salvar no banco usamos o Save, se não usar não persiste.,
            await context.SaveChangesAsync();
            return Ok(model);
        }
        catch (Exception)
        {
            return BadRequest(new { message = "Não foi possível criar a categoria" });
        }
    }


    /// <summary>
    /// Essa rota atualiza os dados
    /// </summary>
    /// <param name="id"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPut]
    [Route("{id:int}")]
    public async Task<ActionResult<List<Category>>> Put(
        int id, 
        [FromBody]Category model,
        [FromServices] DataContext context
        )
    {
        //verifica se o Id informado é o mesmo do modelo
        if (id != model.Id)
            return NotFound(new { message = "Categoria não encontrada" });
        
        //verifica se os dados são válidos
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        try
        {
            context.Entry<Category>(model).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return Ok(model);
        }
        catch (DbUpdateConcurrencyException)
        {
            return BadRequest(new { message = "Esse registro já se encontra atualizado" });
        }
        catch (Exception)
        {
            return BadRequest(new { message = "Não foi possível atualizar a categoria" });
        }

    }


    [HttpDelete]
    [Route("{id:int}")]
    public async Task<ActionResult<List<Category>>> Delete(
        int id,
        [FromServices]DataContext context
        )
    {
        var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
        if (category == null)
            return NotFound(new { message = "Categoria não encontrada" });
        try
        {
            context.Categories.Remove(category);
            await context.SaveChangesAsync();
            return Ok(new { message = "Categoria removida com sucesso!" });
        }
        catch (Exception)
        {
            return BadRequest(new { message = "Não foi possível remover  a categoria" });
        }
    }

}
