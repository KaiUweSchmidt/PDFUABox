namespace PDFUABox.ConverterServices;

internal class JobPool
{
    internal List<Job> Jobs = new List<Job>();

    internal void AddJob(Job job)
    {
        Jobs.Add(job);
    }
}
