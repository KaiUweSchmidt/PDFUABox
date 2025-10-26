// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace PDFUABox.ConverterServices;

internal class JobPool
{
    internal List<Job> Jobs = new List<Job>();

    internal void AddJob(Job job)
    {
        Jobs.Add(job);
    }
}
