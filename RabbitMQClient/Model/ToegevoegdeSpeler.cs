using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RabbitMQClient.Model
{
    public class ToegevoegdeSpeler
    {
        public ToegevoegdeSpeler(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}
