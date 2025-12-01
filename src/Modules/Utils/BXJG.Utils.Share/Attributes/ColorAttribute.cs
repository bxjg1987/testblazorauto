using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Share.Attributes
{
    public class ColorAttribute : Attribute
    {
        // The constructor is called when the attribute is set.
        public ColorAttribute(string colorHexStr)
        {
            this.colorHexStr = colorHexStr;
        }

        // Keep a variable internally ...
        protected string colorHexStr;

        // .. and show a copy to the outside world.
        public string ColorHexStr
        {
            get { return colorHexStr; }
            set { colorHexStr = value; }
        }
    }
}
