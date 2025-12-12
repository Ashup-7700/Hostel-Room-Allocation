using Kemar.HRM.Model.Common;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Kemar.HRM.API.Helpers
{
    public static class CommonHelper
    {
        /// <summary>
        /// Sets audit information (CreatedBy, CreatedByUserId, UpdatedBy, UpdatedAt) for the given entity.
        /// Only sets CreatedByUserId if the entity has that property.
        /// </summary>
        public static void SetUserInformation<T>(T model, int id, HttpContext context)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (context == null) throw new ArgumentNullException(nameof(context));

            // Get username from JWT or fallback
            var username = context.User.Identity?.Name ?? "System";

            // Get UserId from JWT claim (NameIdentifier)
            var userIdClaim = context.User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdClaim, out int userId))
                throw new Exception("User not logged in or UserId claim missing");

            var modelType = model.GetType();

            if (id == 0) // New record
            {
                SetPropertyIfExists(modelType, model, "CreatedBy", username);
                SetPropertyIfExists(modelType, model, "CreatedByUserId", userId); // Only set if property exists
                SetPropertyIfExists(modelType, model, "UpdatedBy", null);
                SetPropertyIfExists(modelType, model, "UpdatedAt", null);
            }
            else // Update record
            {
                SetPropertyIfExists(modelType, model, "UpdatedBy", username);
                SetPropertyIfExists(modelType, model, "UpdatedAt", DateTime.UtcNow);
            }
        }

        /// <summary>
        /// Sets a property only if it exists in the entity.
        /// </summary>
        private static void SetPropertyIfExists(Type type, object model, string propertyName, object? value)
        {
            var prop = type.GetProperty(propertyName);
            if (prop != null && prop.CanWrite)
                prop.SetValue(model, value);
        }

        /// <summary>
        /// Returns an IActionResult based on ResultModel status code and data
        /// </summary>
        public static IActionResult ReturnActionResultByStatus(ResultModel result, ControllerBase controller)
        {
            if (result == null) throw new ArgumentNullException(nameof(result));
            if (controller == null) throw new ArgumentNullException(nameof(controller));

            if (string.IsNullOrEmpty(result.Message))
            {
                result.Message = result.StatusCode switch
                {
                    ResultCode.SuccessfullyCreated => "Created successfully",
                    ResultCode.SuccessfullyUpdated => "Updated successfully",
                    ResultCode.Success => "Success",
                    ResultCode.RecordNotFound => "Record not found",
                    ResultCode.DuplicateRecord => "Duplicate record",
                    ResultCode.Invalid => "Invalid request",
                    ResultCode.NotAllowed => "Action not allowed",
                    ResultCode.Unauthorized => "Unauthorized",
                    ResultCode.ExceptionThrown => "Internal server error",
                    _ => "Success"
                };
            }

            object BodyNoData() => new { statusCode = (int)result.StatusCode, message = result.Message };
            object BodyWithData() => new { statusCode = (int)result.StatusCode, message = result.Message, data = result.Data };

            return result.StatusCode switch
            {
                ResultCode.SuccessfullyCreated => result.Data != null ? controller.StatusCode(201, BodyWithData()) : controller.StatusCode(201, BodyNoData()),
                ResultCode.SuccessfullyUpdated => result.Data != null ? controller.StatusCode(202, BodyWithData()) : controller.StatusCode(202, BodyNoData()),
                ResultCode.Success => result.Data != null ? controller.Ok(BodyWithData()) : controller.Ok(BodyNoData()),
                ResultCode.RecordNotFound => controller.NotFound(BodyNoData()),
                ResultCode.Unauthorized => controller.Unauthorized(BodyNoData()),
                ResultCode.DuplicateRecord => controller.Conflict(BodyNoData()),
                ResultCode.Invalid => controller.BadRequest(BodyNoData()),
                ResultCode.NotAllowed => controller.StatusCode(405, BodyNoData()),
                ResultCode.ExceptionThrown => controller.StatusCode(500, BodyNoData()),
                _ => controller.StatusCode(500, new { statusCode = 500, message = "Internal server error" }),
            };
        }
    }
}
