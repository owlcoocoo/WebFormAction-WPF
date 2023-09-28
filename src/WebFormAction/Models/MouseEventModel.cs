using System.Collections.Generic;

namespace WebFormAction.Models
{
    public class MouseEventModel
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int FrameIndex { get; set; }
        public object[] InvalidFrameIdentifiers { get; set; }
        public string ElementSign { get; set; }
        public List<object> ElementSelectorData { get; set; }
        public bool IsMouseLeftDown { get; set; }
        public bool IsMouseRightDown { get; set; }
        public bool IsRightMenu { get; set; }
    }
}
