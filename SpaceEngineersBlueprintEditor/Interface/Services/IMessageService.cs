namespace SpaceEngineersBlueprintEditor.Interface.Services;

public interface IMessageService : IGlobalService
{
    void SendMessage(string message);
}
