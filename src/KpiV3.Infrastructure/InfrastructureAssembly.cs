﻿using System.Reflection;

namespace KpiV3.Infrastructure;

public static class InfrastructureAssembly
{
    public static Assembly Instance => typeof(InfrastructureAssembly).Assembly;
}
