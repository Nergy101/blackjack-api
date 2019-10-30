using Flurl.Http;
using RabbitMQClient.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQClient
{
    public class SpelerAgent
    {
        private string _URL = "http://localhost:51155/api/blackjack/";
        public void AddSpeler(ToegevoegdeSpeler speler)
        {
            _URL.PostJsonAsync(speler);
        }

        public void StartGame()
        {
            (_URL+"start/true").GetAsync();
        }
    }
}
