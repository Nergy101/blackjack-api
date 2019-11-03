using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlackJackAPI2.Eventbus;
using BlackJackAPI2.Model;
using BlackJackAPI2.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlackJackAPI2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlackJackController : ControllerBase
    {
        ISpelerService _spelerService;
        IEventService _eventService;
        public BlackJackController(ISpelerService spelerService, IEventService eventService)
        {
            _spelerService = spelerService;
            _eventService = eventService;
        }
        // GET: api/BlackJack
        [HttpGet]
        public IEnumerable<int> Get()
        {
            return _eventService.GetNumbers();
        }

        // GET: api/BlackJack/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/BlackJack
        [HttpPost]
        public void Post([FromBody] VoegSpelerToeEvent name) // voegspelertoe
        {
            _spelerService.VoegSpelerToe(name.Name);
        }

        // POST: api/BlackJack
        [HttpGet("start/{startbool}")]
        public void StartGame(string startbool) // voegspelertoe
        {
            if (startbool == "true")
            {
                _spelerService.StartGame();
            }
        }

        // PUT: api/BlackJack/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] VoegSpelerToeEvent value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
