namespace Infrastructure.Events
{
    public interface ICommandSender
    {
        void Send(ICommand command);
    }
}