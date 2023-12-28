using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Share.Concurrency
{
    public interface IHasConcurrencyStamp
    {
        public string ConcurrencyStamp { get; set; }
    }
    //public class ConcurrencyException : Exception { 

    //}
    //这个扩展可以考虑 automapper也调用下
   
}
