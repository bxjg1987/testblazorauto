using Abp.Runtime.Caching;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abp.Runtime.Caching;

public static class CecheExt
{
    public const string cn = "SecurityStamp";
    public static ICache GetSecureStampCache(this ICacheManager cm)
    {
        return cm.GetCache(cn);
    }
}
