// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDFUABox.ConverterServices;

namespace ConverterServices
{
    internal class JobPool
    {
        internal List<Job> Jobs = new List<Job>();


        internal void AddJob(Job job)
        {
            Jobs.Add(job);
        }
    }
}
