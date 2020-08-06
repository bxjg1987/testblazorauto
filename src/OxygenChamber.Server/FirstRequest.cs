using System;
using System.Collections.Generic;
using System.Text;
using SuperSocket;
using SuperSocket.Command;
namespace OxygenChamber.Server
{

    public class FirstRequest : ICommand<OxygenChamberPackage>
    {
       
        public void Execute(IAppSession session, OxygenChamberPackage package)
        {
            throw new NotImplementedException();
        }
    }
}
