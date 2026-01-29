using FluentValidation;
using Insurance.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Insurance.WebApi.Middleware
{
    public class ExceptionHandlingMiddleware
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
            context.Response.ContentType = "application/json";

            var (statusCode, message) = exception switch
            {
                ValidationException vEx =>
                    (HttpStatusCode.BadRequest, string.Join("; ", vEx.Errors.Select(e => e.ErrorMessage))),

                BusinessException bEx =>
                    MapBusinessException(bEx),

                DbUpdateException dbEx when IsUniqueConstraintViolation(dbEx) =>
                (HttpStatusCode.Conflict,
                 "A resource with the same unique value already exists."),

                DbUpdateException =>
               (HttpStatusCode.InternalServerError,
                "Database error occurred."),


                _ =>
                    (HttpStatusCode.InternalServerError,
                     "An unexpected error occurred.")
            };

            context.Response.StatusCode = (int)statusCode;

            var response = new
            {
                error = message,
                status = context.Response.StatusCode
            };

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response));
        }

        private static (HttpStatusCode, string) MapBusinessException(BusinessException ex)
        {
            return ex switch
            {
                NotFoundException =>
                    (HttpStatusCode.NotFound, ex.Message),

                ConflictException =>
                    (HttpStatusCode.Conflict, ex.Message),

                ForbiddenException =>
                    (HttpStatusCode.Forbidden, ex.Message),

                _ =>
                    (HttpStatusCode.BadRequest, ex.Message)
            };
        }

        private static bool IsUniqueConstraintViolation(DbUpdateException ex)
        {
            if (ex.InnerException is SqlException sqlEx)
                return sqlEx.Number == 2601 || sqlEx.Number == 2627;

            return false;
        }

    }
}
