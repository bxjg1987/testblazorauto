using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator
{
    public class ExecuteContext
    {
        public IServiceProvider Services { get; set; }
        public ProjectDefine Project { get; set; }
        public ModelDefine Model { get; set; }

      //  public IEnumerable<TemplateDefine> Templates { get; set; }
    }
}
