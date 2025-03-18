using DvlDev.SATC.Contracts.Responses;
using FluentValidation;
using FluentValidation.Results;

namespace DvlDev.SATC.API.Mappers;

public class ValidationMapperMiddleware(RequestDelegate next)
{
	public async Task InvokeAsync(HttpContext httpContext)
	{
		try
		{
			await next(httpContext);
		}
		catch (ValidationException vex)
		{
			httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
			var validationFailureResponse = new ValidationFailureResponse
			{
				Errors = vex.Errors.Select(x => new ValidationResponse
				{
					PropertyName = x.PropertyName,
					ErrorMessage = x.ErrorMessage
				})
			};
			
			await httpContext.Response.WriteAsJsonAsync(validationFailureResponse);
		}
	}
}