using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.Dto
{
    /// <summary>
    /// Easyui gridview列表对应的分页数据模型
    /// </summary>
    /// <typeparam name="TDto"></typeparam>
    public class EasyuiPageData<TDto>
    {
        public long Total { get; set; }
        public IList<TDto> Rows { get; set; }

        public EasyuiPageData()
        {
          
        }
        public EasyuiPageData(long total, IList<TDto> rows) {
            this.Total = total;
            this.Rows = rows;
        }
    }
}
