//using DeepCopy;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Common.Contracts
{
    /// <summary>
    /// 包含重置的接口
    /// </summary>
    public interface IReset
    {
        void Reset();
        //{
        //    ObjectCloner.CopyTo(Activator.CreateInstance(GetType()), this);
        //    //ObjectCloner.Clone( Activator.CreateInstance(GetType()).Clone();
        //}
    }
}
