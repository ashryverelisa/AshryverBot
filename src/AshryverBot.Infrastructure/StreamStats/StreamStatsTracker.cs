namespace AshryverBot.Infrastructure.StreamStats;

internal class StreamStatsTracker(TimeProvider timeProvider) : IStreamStats, IStreamStatsWriter
{
    private static readonly TimeSpan _sampleRetention = TimeSpan.FromMinutes(90);
    private static readonly TimeSpan _deltaWindow = TimeSpan.FromHours(1);
    private static readonly TimeSpan _deltaTolerance = TimeSpan.FromMinutes(5);

    private readonly Lock _gate = new();
    private readonly LinkedList<(DateTimeOffset At, int Count)> _samples = [];
    private bool _isLive;
    private int? _viewerCount;

    public bool IsLive
    {
        get { lock (_gate) return _isLive; }
    }

    public int? ViewerCount
    {
        get { lock (_gate) return _viewerCount; }
    }

    public int? ViewerDeltaLastHour
    {
        get
        {
            lock (_gate)
            {
                if (!_isLive || _viewerCount is null || _samples.First is null) return null;

                var now = timeProvider.GetUtcNow();
                var oldest = _samples.First.Value;
                if (now - oldest.At < _deltaWindow - _deltaTolerance) return null;

                return _viewerCount.Value - oldest.Count;
            }
        }
    }

    public void Update(int viewerCount)
    {
        lock (_gate)
        {
            var now = timeProvider.GetUtcNow();
            _isLive = true;
            _viewerCount = viewerCount;
            _samples.AddLast((now, viewerCount));

            var cutoff = now - _sampleRetention;
            while (_samples.First is { } first && first.Value.At < cutoff)
            {
                _samples.RemoveFirst();
            }
        }
    }

    public void MarkOffline()
    {
        lock (_gate)
        {
            _isLive = false;
            _viewerCount = null;
            _samples.Clear();
        }
    }
}
