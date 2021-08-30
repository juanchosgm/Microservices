namespace Microservices.MessageBus
{
    public interface IMessageBus
    {
        Task PublishMessage(BaseMessage input, string topicName);
    }
}
