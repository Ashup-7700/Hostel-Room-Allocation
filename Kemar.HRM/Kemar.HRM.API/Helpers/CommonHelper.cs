using Kemar.HRM.Model.Common;
using Microsoft.AspNetCore.Mvc;


namespace Kemar.HRM.API.Helpers
{
    public static class CommonHelper
    {
        public static void SetUserInformation<T>(ref T model, int id, HttpContext context)
        {
            var username = context.User?.Identity?.Name ?? "admin";

            if (id == 0) 
            {
                model.GetType().GetProperty("CreatedBy")?.SetValue(model, username);
               
                model.GetType().GetProperty("UpdatedBy")?.SetValue(model, null);
                model.GetType().GetProperty("UpdatedAt")?.SetValue(model, null);
            }
            else 
            {
                model.GetType().GetProperty("UpdatedBy")?.SetValue(model, username);
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


            //object BodyForSuccessNoData() => new { statusCode = (int)ResultCode.Success, message = result.Message };
            //object BodyForSuccessWithData() => new { statusCode = (int)ResultCode.Success, message = result.Message, data = result.Data };

            //return result.StatusCode switch
            //{
            //    ResultCode.SuccessfullyCreated => cntbase.Ok(BodyForSuccessNoData()), 
            //    ResultCode.SuccessfullyUpdated => cntbase.Ok(BodyForSuccessNoData()),
            //    ResultCode.Success => result.Data != null ? cntbase.Ok(BodyForSuccessWithData()) : cntbase.Ok(BodyForSuccessNoData()),
            //    ResultCode.Unauthorized => cntbase.Unauthorized(new { statusCode = (int)ResultCode.Unauthorized, message = result.Message }),
            //    ResultCode.DuplicateRecord => cntbase.Conflict(new { statusCode = (int)ResultCode.DuplicateRecord, message = result.Message }),
            //    ResultCode.RecordNotFound => cntbase.NotFound(new { statusCode = (int)ResultCode.RecordNotFound, message = result.Message }),
            //    ResultCode.NotAllowed => cntbase.BadRequest(new { statusCode = (int)ResultCode.NotAllowed, message = result.Message }),
            //    ResultCode.Invalid => cntbase.BadRequest(new { statusCode = (int)ResultCode.Invalid, message = result.Message }),
            //    ResultCode.ExceptionThrown => cntbase.StatusCode(500, new { statusCode = 500, message = result.Message }),
            //    _ => cntbase.StatusCode(500, new { statusCode = 500, message = result.Message }),
            //};





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
