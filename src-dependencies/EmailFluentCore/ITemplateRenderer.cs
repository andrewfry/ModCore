using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailFluentCore
{
    public interface ITemplateRenderer
    {
        string Parse<T>(string template, T model, bool isHtml = true);
    }
}
