using PostService.Services;

namespace PostService.Middlewares;

public class SimulatedLatencyMiddleware
{
    private readonly HaltService _haltService;
    private readonly RequestDelegate _next;
    private readonly int _delayInMs;

    public SimulatedLatencyMiddleware(
        RequestDelegate next,
        HaltService haltService)
    {
        _next = next;
        _haltService = haltService;
        _delayInMs = 1800;
    }

    public async Task Invoke(HttpContext context)
    {
        if (_haltService.IsHalted)
        {
            await Task.Delay(_delayInMs);
        }
        await _next(context);
    }
}
