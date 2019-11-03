using System.Collections.Generic;

namespace BlackJackAPI2.Eventbus
{
    public interface IEventService
    {
        public void AddNumber(int a);
        public List<int> GetNumbers();
    }
}