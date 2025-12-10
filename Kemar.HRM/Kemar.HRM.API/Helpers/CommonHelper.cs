using Kemar.HRM.Model.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Kemar.HRM.API.Helpers
{
    public static class CommonHelper
    {
        public static void SetUserInformation<T>(ref T model, int id, HttpContext context)
        {
            var userRole = context.User.Claims
                .FirstOrDefault(c => c.Type == "role")?.Value ?? "HostelManager";

            var userIdClaim = context.User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            int userId = userIdClaim != null ? int.Parse(userIdClaim) : 0;

            var modelType = model.GetType();

            if (id == 0) // New record
            {
                modelType.GetProperty("CreatedBy")?.SetValue(model, userRole);
                modelType.GetProperty("CreatedByUserId")?.SetValue(model, userId);
                modelType.GetProperty("UpdatedBy")?.SetValue(model, null);
                modelType.GetProperty("UpdatedByUserId")?.SetValue(model, null);
                modelType.GetProperty("UpdatedAt")?.SetValue(model, null);
            }
            else // Update existing record
            {
                modelType.GetProperty("UpdatedBy")?.SetValue(model, "admin");
                modelType.GetProperty("UpdatedByUserId")?.SetValue(model, userId);
                modelType.GetProperty("UpdatedAt")?.SetValue(model, DateTime.UtcNow);
            }
        }

        public static IActionResult ReturnActionResultByStatus(ResultModel result, ControllerBase controller)
        {
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
                ResultCode.SuccessfullyCreated => controller.StatusCode(201, BodyNoData()),
                ResultCode.SuccessfullyUpdated => controller.StatusCode(202, BodyNoData()),
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
