using apisincotorra.Datta;
using apisincotorra.Modelss;
using apisincotorra.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject1
{
    public class IntegrationTest
    {
        private readonly IPersonasService _personasService;
        private readonly AppDbContextt _context;
        public IntegrationTest()
        {
            _context = GetImmemoryContext();
            _personasService = new PersonasService(_context);
        }

        private AppDbContextt GetImmemoryContext() 
        {
            var options = new DbContextOptionsBuilder<AppDbContextt>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            return new AppDbContextt(options);           
        }

        [Fact]
        public async Task Probando_GetAll()
        {
            //arrange --> Preparacion
            var personas = new List<Persona>();
            personas.Add(new Persona { Edad = 23, Nombre = "Santiago" });
            personas.Add(new Persona { Edad = 24, Nombre = "Edgar" });
            personas.Add(new Persona { Edad = 25, Nombre = "Willian" });

            _context.Personas.AddRange(personas);
            _context.SaveChanges();


            //act --> Accion
            var personasResult = _personasService.GetAllAsync();


            //Assert
            Assert.NotNull(personasResult);
            Assert.Equal(3, personasResult.Count);
            Assert.NotNull(personasResult.FirstOrDefault(p => p.Nombre == "Edgar"));
            Assert.NotNull(personasResult.FirstOrDefault(p => p.Nombre == "Santiago"));
            Assert.NotNull(personasResult.FirstOrDefault(p => p.Nombre == "Willian"));
        }


        [Fact]
        public async Task Probando_getById() 
        {
            //arrange --> Preparacion
            var personaParaElText = new Persona { Edad = 23, Nombre = "Santiago" };
            _context.Personas.Add(personaParaElText);
            _context.SaveChanges();


            //act --> Accion
            var personaBuscada = _personasService.GetByIdAsync(personaParaElText.Id);


            //Assert
            Assert.NotNull(personaBuscada);
            Assert.Equal(23, personaBuscada.Edad);
            Assert.Equal("Santiago", personaBuscada.Nombre);
        }

    }
}
