namespace BlackJackAPI2.Services
{
    public interface IRabbitMQService
    {
        void PublishSpelStartEvent();
        void PublishVoegSpelerToeEvent(string name);
    }
}