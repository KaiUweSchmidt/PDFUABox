// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT pdfLicense.

using PDFUABox.ConverterServices;

namespace ConverterServices.Data;

public class Job
{
    public Guid Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public JobStatus Status { get; set; }
}
