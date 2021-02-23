using DIO.Api.Data.Collections;
using DIO.Api.Model;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System;

namespace DIO.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InfectadoController : Controller
    {
        Data.MongoDB _mongoDB;
        IMongoCollection<Infectado> _infectadosCollection;
        
        public InfectadoController(Data.MongoDB mongoDB)
        {
            _mongoDB = mongoDB;
            _infectadosCollection = _mongoDB.DB.GetCollection<Infectado>(typeof(Infectado).Name.ToLower());
        }
        [HttpPost]
        public ActionResult SalvarInfectado([FromBody] InfectadoDto dto)
        {
            var infectado = new Infectado(dto.DataNascimento, dto.Sexo, dto.Latitude, dto.longitude);
            _infectadosCollection.InsertOne(infectado);
            return StatusCode(201, "Infectado adicionado com sucesso");
        }
        [HttpGet]
        public ActionResult ObterInfectado()
        {
            var infectados = _infectadosCollection.Find(Builders<Infectado>.Filter.Empty).ToList();
            return Ok(infectados);
        }
        [HttpPut]
        public ActionResult AtualizarInfectado([FromBody] InfectadoDto dto)
        {
            _infectadosCollection.UpdateOne(Builders<Infectado>.Filter.Where(v => v.DataNascimento == dto.DataNascimento),
                Builders<Infectado>.Update.Set("sexo", dto.Sexo));
            return Ok("Atualizado com sucesso");
        }

        [HttpDelete("{dataNasc}")]
        public ActionResult Delete(DateTime  dateNasc)
        {
            _infectadosCollection.DeleteOne(Builders<Infectado>.Filter.Where(v => v.DataNascimento == dateNasc));
            return Ok("Deletado com sucesso");
        }
    }
}
