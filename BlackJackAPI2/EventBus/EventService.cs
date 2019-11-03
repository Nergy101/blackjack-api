using BlackJackAPI2.Eventbus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlackJackAPI2.EventBus
{
    public class EventService : IEventService
    {
        public List<int> Numbers = new List<int>();
        public void AddNumber(int a)
        {
            Numbers.Add(a);
        }

        public List<int> GetNumbers()
        {
            return Numbers;
        }
    }
}
