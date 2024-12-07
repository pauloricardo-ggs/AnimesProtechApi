using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Application;

public class InternalServerErrorObjectResult : ObjectResult
{
    public InternalServerErrorObjectResult(object value) : base(value) { StatusCode = StatusCodes.Status500InternalServerError; }
}

public class ForbidObjectResult : ObjectResult
{
    public ForbidObjectResult(object value) : base(value) { StatusCode = StatusCodes.Status403Forbidden; }
}

public class CreatedObjectResult : ObjectResult
{
    public CreatedObjectResult(object value) : base(value) { StatusCode = StatusCodes.Status201Created; }
}