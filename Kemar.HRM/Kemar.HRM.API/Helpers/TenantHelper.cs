
//namespace Kemar.HRM.API.Helpers
//{
//    public static class TenantHelper
//    {
//        public static void SetTenantInfo(HttpContext httpContext, object request)
//        {

//            try
//            {
//                var tenantId = httpContext.Request.Headers["X-Tenant-Id"].FirstOrDefault();
//                if (tenantId != null)
//                {
//                    var prop = request.GetType().GetProperty("TenantId");
//                    if (prop != null && prop.PropertyType == typeof(string))
//                        prop.SetValue(request, tenantId);
//                }
//            }
//            catch { }
//        }
//    }
//}
