using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wox.Plugin.Runner
{
    public class Runner : IPlugin
    {
        public void Init( PluginInitContext context )
        {
            
        }

        public List<Result> Query( Query query )
        {
            var results = new List<Result>();
            results.Add( new Result
                {
                    Title = "Hello world!",
                    SubTitle = "You've successfully installed this plugin.",
                    IcoPath = "Images\\globe.png",
                    Action = e =>
                        {
                            return false;
                        }
                } );
            return results;
        }
    }
}
