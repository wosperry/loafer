﻿using System;

namespace Loaf.EntityFrameworkCore.Repository.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class LoafGreaterThanOrEqualThanAttribute : LoafWhereAttribute
    {
    }
}
