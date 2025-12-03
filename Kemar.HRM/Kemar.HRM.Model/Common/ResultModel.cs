namespace Kemar.HRM.Model.Common
{
    public enum ResultCode
    {
        Success = 200,
        SuccessfullyCreated = 201,
        SuccessfullyUpdated = 202,
        Unauthorized = 401,
        DuplicateRecord = 409,
        RecordNotFound = 404,
        NotAllowed = 405,
        Invalid = 400,
        ExceptionThrown = 500
    }
}

namespace Kemar.HRM.Model.Common
{
    public class ResultModel
    {
        public ResultCode StatusCode { get; set; } = ResultCode.Success;
        public string? Message { get; set; }
        public object? Data { get; set; }

        public static ResultModel Success(object? data = null, string? message = "Success")
            => new ResultModel { StatusCode = ResultCode.Success, Message = message, Data = data };

        public static ResultModel Created(object? data = null, string? message = "Created")
            => new ResultModel { StatusCode = ResultCode.SuccessfullyCreated, Message = message, Data = data };

        public static ResultModel Updated(object? data = null, string? message = "Updated")
            => new ResultModel { StatusCode = ResultCode.SuccessfullyUpdated, Message = message, Data = data };

        public static ResultModel Failure(ResultCode code = ResultCode.Invalid, string? message = "Failed", object? data = null)
            => new ResultModel { StatusCode = code, Message = message, Data = data };

        public static ResultModel NotFound(string? message = "Not found")
            => Failure(ResultCode.RecordNotFound, message);

        public static ResultModel Unauthorized(string? message = "Unauthorized")
            => Failure(ResultCode.Unauthorized, message);
    }
}
