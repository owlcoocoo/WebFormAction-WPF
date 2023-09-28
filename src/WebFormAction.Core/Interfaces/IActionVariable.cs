namespace WebFormAction.Core.Interfaces
{
    public interface IActionVariable
    {
        string Name { get; set; }
        dynamic Value { get; set; }
    }
}
