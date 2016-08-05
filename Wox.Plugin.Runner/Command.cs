using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wox.Plugin.Runner
{
    class Command
    {
        public string Shortcut { get; set; }
        public string Description { get; set; }
        public string Path { get; set; }
        public string WorkingDirectory { get; set; }
        public string ArgumentsFormat { get; set; }
    }
}
