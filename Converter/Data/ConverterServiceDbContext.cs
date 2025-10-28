// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT pdfLicense.

using Microsoft.EntityFrameworkCore;

namespace ConverterServices.Data;

internal class ConverterServiceDbContext : DbContext
{
    public ConverterServiceDbContext(DbContextOptions<ConverterServiceDbContext> options)
        : base(options)
    { }
    public DbSet<Job> Jobs { get; set; }
       
}
