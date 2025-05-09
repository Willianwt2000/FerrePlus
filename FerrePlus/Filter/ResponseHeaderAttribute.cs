using Microsoft.AspNetCore.Mvc.Filters;

public class ResponseHeaderAttribute : ActionFilterAttribute
{
    private readonly string _name;
    private readonly string _rol;

    public ResponseHeaderAttribute(string name, string rol) =>
        (_name, _rol) = (name, rol);

    public override void OnResultExecuting(ResultExecutingContext context)
    {
        context.HttpContext.Response.Headers.Add(_name, _rol);

        base.OnResultExecuting(context);
    }
}