﻿using System;
using System.Linq.Expressions;

namespace Loaf.EntityFrameworkCore.Extensions.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class LoafRightLikeAttribute : LoafWhereAttribute
    {
        public override Expression GetCompareExpression(Expression propertyExpression, Expression valueExpression)
        {
            var stringStartWith = typeof(string).GetMethod(nameof(string.EndsWith), new Type[] { typeof(string) });
            return Expression.Call(propertyExpression, stringStartWith, valueExpression);
        }
    }
}