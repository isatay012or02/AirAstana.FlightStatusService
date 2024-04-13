using System.Net;
using AirAstanaFlightStatusService.Application.Common.Constants;
using KDS.Primitives.FluentResult;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AirAstanaFlightStatusService.Api.Controllers;

/// <summary>
/// Базовый контроллер с настройками
/// </summary>
[ApiController]
[ProducesResponseType(statusCode: (int)HttpStatusCode.BadRequest, type: typeof(ProblemDetails))]
[ProducesResponseType(statusCode: (int)HttpStatusCode.NotFound, type: typeof(ProblemDetails))]
[ProducesResponseType(statusCode: (int)HttpStatusCode.UnprocessableEntity, type: typeof(ProblemDetails))]
[ProducesResponseType(statusCode: (int)HttpStatusCode.ServiceUnavailable, type: typeof(ProblemDetails))]
[ProducesResponseType(statusCode: (int)HttpStatusCode.InternalServerError, type: typeof(ProblemDetails))]
public class BaseController : ControllerBase
{
    /// <summary>
    /// Mediator
    /// </summary>
    protected ISender Mediator =>
        HttpContext.RequestServices.GetRequiredService<ISender>() ?? throw new ArgumentNullException(nameof(ISender));

    protected ObjectResult ProblemResponse(Error error)
    {
        return error.Code switch
        {
            ErrorCode.DatabaseError => Problem(title: "Ошибка во время подключения к базе данных",
                detail: error.Message,
                statusCode: (int)HttpStatusCode.InternalServerError),
            ErrorCode.ExternalError => Problem(title: "Ошибка во время подключения к сервисной шине",
                detail: error.Message,
                statusCode: (int)HttpStatusCode.ServiceUnavailable),
            ErrorCode.LogicConflict => Problem(title: "Конфликт логической зависимости",
                detail: error.Message,
                statusCode: (int)HttpStatusCode.BadRequest),
            ErrorCode.ParameterError => Problem(title: "Невалидный параметр",
                detail: error.Message,
                statusCode: (int)HttpStatusCode.BadRequest),
            ErrorCode.NotFound => Problem(title: "Не удалось найти информацию",
                detail: error.Message,
                statusCode: (int)HttpStatusCode.NotFound),
            _ => Problem(title: "Необработанное исключение",
                detail: error.Message,
                statusCode: (int)HttpStatusCode.InternalServerError),
        };
    }
}