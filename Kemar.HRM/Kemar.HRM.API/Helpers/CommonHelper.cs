using Kemar.HRM.Model.Common;
using Microsoft.AspNetCore.Mvc;

namespace Kemar.HRM.API.Helpers
{
    public static class CommonHelper
    {
        public static void SetUserInformation<T>(ref T model, int id, HttpContext context)
        {

            var userRole = context.User.Claims
                .FirstOrDefault(c => c.Type == "role")?.Value ?? "HostelManager";

            if (id == 0)
            {

                model.GetType().GetProperty("CreatedBy")?.SetValue(model, userRole);

                model.GetType().GetProperty("UpdatedBy")?.SetValue(model, null);
                model.GetType().GetProperty("UpdatedAt")?.SetValue(model, null);
            }
            else
            {

                model.GetType().GetProperty("UpdatedBy")?.SetValue(model, "admin");
                model.GetType().GetProperty("UpdatedAt")?.SetValue(model, DateTime.UtcNow);
            }
        }

        public static IActionResult ReturnActionResultByStatus(ResultModel result, ControllerBase cntbase)
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
                    _ => "Success"
                };
            }

            object BodyNoData() => new { statusCode = (int)result.StatusCode, message = result.Message };
            object BodyWithData() => new { statusCode = (int)result.StatusCode, message = result.Message, data = result.Data };

            return result.StatusCode switch
            {
                ResultCode.SuccessfullyCreated => cntbase.StatusCode(201, BodyNoData()),
                ResultCode.SuccessfullyUpdated => cntbase.StatusCode(202, BodyNoData()),

                ResultCode.Success => result.Data != null
                    ? cntbase.Ok(BodyWithData())
                    : cntbase.Ok(BodyNoData()),

                ResultCode.RecordNotFound => cntbase.NotFound(
                    new { statusCode = 404, message = result.Message }),

                ResultCode.Unauthorized => cntbase.Unauthorized(
                    new { statusCode = 401, message = result.Message }),

                ResultCode.DuplicateRecord => cntbase.Conflict(
                    new { statusCode = 409, message = result.Message }),

                ResultCode.Invalid => cntbase.BadRequest(
                    new { statusCode = 400, message = result.Message }),

                ResultCode.NotAllowed => cntbase.StatusCode(405,
                    new { statusCode = 405, message = result.Message }),

                ResultCode.ExceptionThrown => cntbase.StatusCode(500,
                    new { statusCode = 500, message = result.Message }),

                _ => cntbase.StatusCode(500,
                    new { statusCode = 500, message = "Internal server error" }),
            };
        }
    }
}
