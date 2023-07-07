using System.Globalization;
using System.Text.RegularExpressions;

namespace ZLJ.Web.Host.Startup
{
    /// <summary>
    /// 自定义路由约束
    /// 实现路由限定在应用中
    /// 参考：https://learn.microsoft.com/zh-cn/aspnet/core/fundamentals/routing?view=aspnetcore-7.0#custom-route-constraints
    /// 和starupt中路由部分的设置
    /// </summary>
    public class AppRouteConstraint : IRouteConstraint
    {
        public bool Match(
            HttpContext? httpContext, IRouter? route, string routeKey,
            RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (!values.TryGetValue(routeKey, out var routeValue))
            {
                return false;
            }

            var routeValueString = Convert.ToString(routeValue, CultureInfo.InvariantCulture);
            var apps = httpContext.RequestServices.GetRequiredService<CommonApplicationConfiguration>();

            return apps.Apps.ContainsKey(routeValueString);
            // return _regex.IsMatch(routeValueString);
        }
    }
}