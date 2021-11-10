using System;
using System.Linq.Expressions;
using System.Diagnostics.Contracts;

namespace MathLib
{
    public static class GenericOperators
    {        
        public static Func<T, T, T> AddDelegate<T>()
        {
            ParameterExpression paramLhs = Expression.Parameter(typeof(T), "lhs"),
                paramRhs = Expression.Parameter(typeof(T), "rhs");           
            BinaryExpression body = Expression.Add(paramLhs, paramRhs);            
            Func<T, T, T> add = Expression.Lambda<Func<T, T, T>>(body, paramLhs, paramRhs).Compile();
            return add;
        }

        public static Func<T, T, T> SubtractDelegate<T>()
        {
            ParameterExpression paramLhs = Expression.Parameter(typeof(T), "lhs"),
                paramRhs = Expression.Parameter(typeof(T), "rhs");
            BinaryExpression body = Expression.Subtract(paramLhs, paramRhs);
            Func<T, T, T> subtract = Expression.Lambda<Func<T, T, T>>(body, paramLhs, paramRhs).Compile();
            return subtract;                    
        }

        public static Func<T, T> NegateDelegate<T>()
        {
            ParameterExpression paramArg = Expression.Parameter(typeof(T), "arg");
            UnaryExpression body = Expression.Negate(paramArg);
            Func<T, T> negate = Expression.Lambda<Func<T, T>>(body, paramArg).Compile();
            return negate;                
        }

        public static Func<T, T, T> MultiplyDelegate<T>()
        {
            ParameterExpression paramLhs = Expression.Parameter(typeof(T), "lhs"),
                paramRhs = Expression.Parameter(typeof(T), "rhs");
            BinaryExpression body = Expression.Multiply(paramLhs, paramRhs);
            Func<T, T, T> multiply = Expression.Lambda<Func<T, T, T>>(body, paramLhs, paramRhs).Compile();
            return multiply;
        }

        public static Func<T, T, T> DivideDelegate<T>()
        {
            ParameterExpression paramLhs = Expression.Parameter(typeof(T), "lhs"),
                paramRhs = Expression.Parameter(typeof(T), "rhs");
            BinaryExpression body = Expression.Divide(paramLhs, paramRhs);
            Func<T, T, T> divide = Expression.Lambda<Func<T, T, T>>(body, paramLhs, paramRhs).Compile();
            return divide;
        }

        public static Func<T, T, T> AddAssignDelegate<T>()
        {
            ParameterExpression paramLhs = Expression.Parameter(typeof(T), "lhs"),
                paramRhs = Expression.Parameter(typeof(T), "rhs");
            BinaryExpression body = Expression.AddAssign(paramLhs, paramRhs);
            // Contract.Assume(body != null);
            Func<T, T, T> addAssign = Expression.Lambda<Func<T, T, T>>(body, paramLhs, paramRhs).Compile();
            return addAssign;
        }
    }
}
