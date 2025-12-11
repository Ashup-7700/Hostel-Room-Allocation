//using Kemar.HRM.Model.Common;
//using Microsoft.AspNetCore.Mvc;

//namespace Kemar.HRM.API.Helpers
//{
//    public static class CommonHelper
//    {
//        public static void SetUserInformation<T>(T model, int id, HttpContext context)
//        {
//            var username = context.User.Claims
//                .FirstOrDefault(c => c.Type == "username")?.Value ?? "UnknownUser";

//            var userIdClaim = context.User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
//            int userId = userIdClaim != null ? int.Parse(userIdClaim) : throw new Exception("User not login");

//            var modelType = model.GetType();


//            SetProperty(modelType, model, "CreatedBy", username);
//            SetProperty(modelType, model, "CreatedByUserId", userId);

//            if (id == 0) 
//            {
//                SetProperty(modelType, model, "UpdatedBy", null);
//                SetProperty(modelType, model, "UpdatedByUserId", null);
//                SetProperty(modelType, model, "UpdatedAt", null);
//            }
//            else 
//            {
//                SetProperty(modelType, model, "UpdatedBy", username);
//                SetProperty(modelType, model, "UpdatedByUserId", userId);
//                SetProperty(modelType, model, "UpdatedAt", DateTime.UtcNow);
//            }
//        }

//        private static void SetProperty(Type type, object model, string propertyName, object? value)
//        {
//            var prop = type.GetProperty(propertyName);
//            if (prop != null)
//            {
//                prop.SetValue(model, value);
//            }
//        }

//        public static IActionResult ReturnActionResultByStatus(ResultModel result, ControllerBase controller)
//        {
//            if (string.IsNullOrEmpty(result.Message))
//            {
//                result.Message = result.StatusCode switch
//                {
//                    ResultCode.SuccessfullyCreated => "Created successfully",
//                    ResultCode.SuccessfullyUpdated => "Updated successfully",
//                    ResultCode.Success => "Success",
//                    ResultCode.RecordNotFound => "Record not found",
//                    ResultCode.DuplicateRecord => "Duplicate record",
//                    ResultCode.Invalid => "Invalid request",
//                    ResultCode.NotAllowed => "Action not allowed",
//                    ResultCode.Unauthorized => "Unauthorized",
//                    ResultCode.ExceptionThrown => "Internal server error",
//                    _ => "Success"
//                };
//            }

//            object BodyNoData() => new { statusCode = (int)result.StatusCode, message = result.Message };
//            object BodyWithData() => new { statusCode = (int)result.StatusCode, message = result.Message, data = result.Data };

//            return result.StatusCode switch
//            {
//                ResultCode.SuccessfullyCreated => controller.StatusCode(201, BodyNoData()),
//                ResultCode.SuccessfullyUpdated => controller.StatusCode(202, BodyNoData()),
//                ResultCode.Success => result.Data != null ? controller.Ok(BodyWithData()) : controller.Ok(BodyNoData()),
//                ResultCode.RecordNotFound => controller.NotFound(BodyNoData()),
//                ResultCode.Unauthorized => controller.Unauthorized(BodyNoData()),
//                ResultCode.DuplicateRecord => controller.Conflict(BodyNoData()),
//                ResultCode.Invalid => controller.BadRequest(BodyNoData()),
//                ResultCode.NotAllowed => controller.StatusCode(405, BodyNoData()),
//                ResultCode.ExceptionThrown => controller.StatusCode(500, BodyNoData()),
//                _ => controller.StatusCode(500, new { statusCode = 500, message = "Internal server error" }),
//            };
//        }
//    }
//}








































using Kemar.HRM.Model.Common;
using Microsoft.AspNetCore.Mvc;

namespace Kemar.HRM.API.Helpers
{
    public static class CommonHelper
    {
        public static void SetUserInformation<T>(T model, int id, HttpContext context)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (context == null) throw new ArgumentNullException(nameof(context));

            var username = context.User.Claims
                .FirstOrDefault(c => c.Type == "username")?.Value ?? "UnknownUser";

            var userIdClaim = context.User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
            {
                throw new Exception("User not logged in or invalid UserId claim");
            }

            var modelType = model.GetType();

            // Set Created properties
            SetProperty(modelType, model, "CreatedBy", username);
            SetProperty(modelType, model, "CreatedByUserId", userId);

            if (id == 0) // New record
            {
                SetProperty(modelType, model, "UpdatedBy", null);
                SetProperty(modelType, model, "UpdatedByUserId", null);
                SetProperty(modelType, model, "UpdatedAt", null);
            }
            else // Update record
            {
                SetProperty(modelType, model, "UpdatedBy", username);
                SetProperty(modelType, model, "UpdatedByUserId", userId);
                SetProperty(modelType, model, "UpdatedAt", DateTime.UtcNow);
            }
        }

        private static void SetProperty(Type type, object model, string propertyName, object? value)
        {
            var prop = type.GetProperty(propertyName);
            if (prop != null && prop.CanWrite)
            {
                prop.SetValue(model, value);
            }
        }

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
