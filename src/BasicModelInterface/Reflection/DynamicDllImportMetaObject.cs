using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq.Expressions;
using System.Reflection;

namespace BasicModelInterface.Reflection
{
    /// <summary>
    /// Original source: https://code.google.com/p/dynamicdllimport/
    /// </summary>
    internal class DynamicDllImportMetaObject : DynamicMetaObject
    {
        public DynamicDllImportMetaObject(Expression expression, object value)
            : base(expression, BindingRestrictions.Empty, value)
        {
        }

        public override DynamicMetaObject BindInvokeMember(InvokeMemberBinder binder, DynamicMetaObject[] args)
        {
            Type returnType = GetMethodReturnType(binder);
            Type[] types = new Type[args.Length];
            Expression[] arguments = new Expression[args.Length];
            for (int i = 0; i < args.Length; i++)
            {
                Type type = args[i].LimitType;
                Expression expression = args[i].Expression;
                dynamic typedParameterExpression = expression;
                if (typedParameterExpression.IsByRef)
                {
                    types[i] = type.MakeByRefType();
                }
                else
                {
                    types[i] = type;
                }
                arguments[i] = expression;
            }
            MethodInfo method = (base.Value as DynamicDllImport).GetInvokeMethod(binder.Name, returnType, types);
            Expression callingExpression;
            if (method.ReturnType == typeof(void))
            {
                callingExpression = Expression.Block(Expression.Call(method, arguments), Expression.Default(typeof(object)));
            }
            else
            {
                callingExpression = Expression.Convert(Expression.Call(method, arguments), typeof(object));
            }
            BindingRestrictions bindingRestrictions = BindingRestrictions.GetTypeRestriction(this.Expression, typeof(DynamicDllImport));
            return new DynamicMetaObject(callingExpression, bindingRestrictions);
        }

        private Type GetMethodReturnType(InvokeMemberBinder binder)
        {
            IList<Type> types = binder.GetType().GetField("m_typeArguments", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(binder) as IList<Type>;
            if ((types != null) && (types.Count > 0))
            {
                return types[0];
            }
            return null;
        }
    }
}