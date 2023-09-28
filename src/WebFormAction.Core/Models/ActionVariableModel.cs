using WebFormAction.Core.Interfaces;

namespace WebFormAction.Core.Models
{
    public class ActionVariableModel : IActionVariable
    {
        public ActionVariableModel(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
        public dynamic Value { get; set; }
    }
}
