using ModCore.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.ViewModels.Core
{
    public class vPagedResult<T> : vPagedInfo
    {
        public IList<T> CurrentPageResults { get; set; }
    }
}
