using FluentValidation;
using Insurance.Domain.Exceptions;
using Insurance.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Insurance.WebApi.Middleware;

public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var error = MapException(exception);

        context.Response.StatusCode = error.StatusCode;
        context.Response.ContentType = "application/json";

        await context.Response.WriteAsync(JsonSerializer.Serialize(new
        {
            error = error.Message,
            status = error.StatusCode
        }));
    }

    private static ErrorResponse MapException(Exception exception)
    {
        return exception switch
        {
            ValidationException vEx =>
                new ErrorResponse(
                    StatusCodes.Status400BadRequest,
                    string.Join("; ", vEx.Errors.Select(e => e.ErrorMessage))),

            BusinessException bEx =>
                new ErrorResponse(
                    StatusCodes.Status400BadRequest,
                    bEx.Message),

            NotFoundException nfEx =>
                new ErrorResponse(
                    StatusCodes.Status404NotFound,
                    nfEx.Message),

            ConflictException cEx =>
                new ErrorResponse(
                    StatusCodes.Status409Conflict,
                    cEx.Message),

            ForbiddenException fEx =>
                new ErrorResponse(
                    StatusCodes.Status403Forbidden,
                    fEx.Message),

            UnauthorizedException uEx =>
                new ErrorResponse(
                    StatusCodes.Status401Unauthorized,
                    uEx.Message),

            DbUpdateException dbEx when IsUniqueConstraintViolation(dbEx) =>
                new ErrorResponse(
                    StatusCodes.Status409Conflict,
                    "A resource with the same unique value already exists."),

            DbUpdateException =>
                new ErrorResponse(
                    StatusCodes.Status500InternalServerError,
                    "Database error occurred."),

            _ =>
                new ErrorResponse(
                    StatusCodes.Status500InternalServerError,
                    "An unexpected error occurred.")
        };
    }

    private static bool IsUniqueConstraintViolation(DbUpdateException ex)
    {
        return ex.InnerException is SqlException sqlEx
            && (sqlEx.Number == 2601 || sqlEx.Number == 2627);
    }

    private record ErrorResponse(int StatusCode, string Message);
}
