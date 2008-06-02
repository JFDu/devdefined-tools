using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;

namespace DevDefined.Common.Compilation
{
    public static class SwitchCompiler
    {
        public static Func<string, TReturn> CreateSwitch<TReturn>(IDictionary<string, TReturn> caseDictionary)
        {
            var cases = new List<Case<TReturn>>(caseDictionary.Count);
            foreach (var kv in caseDictionary)
                cases.Add(new Case<TReturn> { C = kv.Key, R = kv.Value });
            var p = Expression.Parameter(typeof(string), "p");
            var expr = Expression.Lambda<Func<string, TReturn>>(
                BuildStringLength(p, cases.ToOrderedArray(s => s.C.Length), 0, cases.Count - 1),
                new ParameterExpression[] { p }
            );
            var del = expr.Compile();
            return del;
        }

        static Expression BuildStringLength<TR>(ParameterExpression p, Case<TR>[] pairs, int lower, int upper)
        {
            int middle = MidPoint(lower, upper);
            if (pairs[lower].C.Length == pairs[middle].C.Length)
                return BuildStringChar(p, pairs.TakePage(lower, upper).ToOrderedArray(c => c.C), 0, 0, upper - lower);
            middle = pairs.FirstDifferentDown(middle, (c) => c.C.Length);
            return Expression.Condition(
                Expression.LessThan(Expression.Call(p, StringLength), Expression.Constant(pairs[middle + 1].C.Length)),
                BuildStringLength(p, pairs, lower, middle),
                BuildStringLength(p, pairs, middle + 1, upper)
            );
        }

        static Expression BuildStringChar<TR>(ParameterExpression p, Case<TR>[] pairs, int index, int lower, int upper)
        {
            if (pairs.TakePage(lower, upper).All(c => c.R.Equals(pairs[lower].R)))
                return Expression.Constant(pairs[lower].R);
            int middle = MidPoint(lower, upper);
            if (pairs[middle].C[index] == pairs[lower].C[index])
                return BuildStringChar(p, pairs, index + 1, lower, upper);
            middle = pairs.FirstDifferentDown(middle, (c) => c.C[index]);
            return Expression.Condition(
                Expression.LessThan(Expression.Call(p, StringIndex, Expression.Constant(index)),
                    Expression.Constant(pairs[middle + 1].C[index])),
                BuildStringChar(p, pairs, index, lower, middle),
                BuildStringChar(p, pairs, index, middle + 1, upper)
            );

        }

        struct Case<TR>
        {
            public string C { get; set; }
            public TR R { get; set; }
            public override string ToString()
            {
                return C.ToString() + " " + R.ToString();
            }
        }

        static int MidPoint(int lower, int upper)
        {
            return ((upper - lower + 1) / 2) + lower;
        }

        static MethodInfo StringLength = typeof(string).GetMethod("get_Length");
        static MethodInfo StringIndex = typeof(string).GetMethod("get_Chars");
    }
}
