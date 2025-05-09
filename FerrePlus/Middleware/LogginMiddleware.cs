

namespace FerrePlus.Middleware
{
  public class LogginMiddleware

  {
    private readonly RequestDelegate _next;

    public LogginMiddleware(RequestDelegate next)
    {
      _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
      Console.WriteLine("[Middleware] --- Antes de la ejecución del endpoint");
      await _next(context);
      Console.WriteLine("[Middleware] --- Después de la ejecución del endpoint");
    }
  }
}