using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flex.Domain.Dtos.WorkFlow
{
    public class Text
    {
        public string text { get; set; }
        public TextPos textPos { get; set; }
    }

    public class TextPos
    {
        public int x { get; set; }
        public int y { get; set; }
    }

    public class Attr
    {
        public int x { get; set; }
        public int y { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

    public class State
    {
        public string type { get; set; }
        public string ID { get; set; }
        public Text text { get; set; }
        public Attr attr { get; set; }
    }

    public class PathObject
    {
        public string lineID { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public List<object> dots { get; set; }
        public Text text { get; set; }
        public Props props { get; set; }
    }

    public class Props
    {
        public Text text { get; set; }
    }

    public class WorkflowData
    {
        public Dictionary<string, State> states { get; set; }
        public Dictionary<string, PathObject> paths { get; set; }
    }
}
