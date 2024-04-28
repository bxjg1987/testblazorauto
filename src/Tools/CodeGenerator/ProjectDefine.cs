using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator
{
    public class ProjectDefine
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public bool IsDefault { get; set; }

        public List<ModelDefine> Models { get; set; }
        //public List<TemplateDefine> Templates { get; set; }
    }
}
