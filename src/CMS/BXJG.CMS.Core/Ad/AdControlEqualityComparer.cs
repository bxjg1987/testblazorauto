using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace BXJG.CMS.Ad
{
    public class AdControlEqualityComparer : IEqualityComparer<AdControlEntity>
    {
        public bool Equals([AllowNull] AdControlEntity x, [AllowNull] AdControlEntity y)
        {
            throw new NotImplementedException();
        }

        public int GetHashCode([DisallowNull] AdControlEntity obj)
        {
            throw new NotImplementedException();
        }
    }
}
