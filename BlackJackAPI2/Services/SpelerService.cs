using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlackJackAPI2.Services
{
    public class SpelerService : ISpelerService
    {
        IRabbitMQService _rmqs;
        public SpelerService(IRabbitMQService rmqs)
        {
            _rmqs = rmqs;
        }

        public void StartGame()
        {
            // do stuff

            // publish event
            _rmqs.PublishSpelStartEvent();
        }

        public void VoegSpelerToe(string name)
        {
            // do stuff

            // publish event
            _rmqs.PublishVoegSpelerToeEvent(name);
        }
    }
}
