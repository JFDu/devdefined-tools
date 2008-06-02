using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DevDefined.Common.Pipeline
{
    public delegate T Filter<T, TContext>(T input, TContext context);
}
