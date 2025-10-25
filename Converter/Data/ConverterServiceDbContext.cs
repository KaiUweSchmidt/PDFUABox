// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace ConverterServices.Data;

internal class ConverterServiceDbContext : DbContext
{
    public ConverterServiceDbContext(DbContextOptions<ConverterServiceDbContext> options)
        : base(options)
    { }
    public DbSet<Job> Jobs { get; set; }
       
}
