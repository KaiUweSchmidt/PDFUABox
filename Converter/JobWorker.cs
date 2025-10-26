using System.ComponentModel;
using log4net;

namespace PDFUABox.ConverterServices;

public class JobWorker : IDisposable
{
    private readonly BackgroundWorker _backgroundWorker;
    private bool _stopRequested;
    private readonly Converter _converter;
    private readonly ILog _logger = LogManager.GetLogger(typeof(JobWorker));
    public JobWorker(Converter converter)
    {
        _converter = converter;
        _backgroundWorker = new BackgroundWorker();
        _backgroundWorker.DoWork += BackgroundWorker_DoWork;
        _backgroundWorker.WorkerSupportsCancellation = true;
    }

    private void BackgroundWorker_DoWork(object? sender, DoWorkEventArgs e) {
        while (!_stopRequested)
        {
            var job = _converter.GetNextPendingJob();
            if (job != null)
            {
                try
                {
                    job.Run();
                }
                catch (InvalidOperationException ex)
                {
                    _logger.Error($"Error processing job {job.Id}: {ex.Message}", ex);
                }
            }
            else
            {
                // No pending jobs, sleep for a while
                Task.Delay(1000).Wait();
            }
        }
    }

    public void Start()
    {
        _stopRequested = false;

        _backgroundWorker.RunWorkerAsync();

        _logger.Debug("Service started successfully.");
    }

    public void Stop()
    {
        _stopRequested = true;

        Dispose();

        _logger.Debug("Service stopped successfully.");
    }
    
    private bool isDisposed;
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    protected virtual void Dispose(bool disposing)
    {
        if (isDisposed) return;

        if (disposing)
        {
            _stopRequested = true;

            if (_backgroundWorker != null && _backgroundWorker.IsBusy)
            {
                _backgroundWorker.CancelAsync();
                _backgroundWorker.Dispose();
            }
        }

        isDisposed = true;
    }

}
