using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DevDefined.Common.Pipeline
{
    public interface IOperation<T, TContext>
    {
        T Execute(T input, TContext context);
    }
}
