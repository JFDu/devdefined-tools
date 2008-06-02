using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DevDefined.Common.Extensions.Annotations
{
    public interface IAnnotation
    {
        object this[object key] { get; set; }
        void Clear();
        void Remove(object key);
        int Count { get; }
        void Annotate<T>(params Func<string, T>[] args);
    }
}
