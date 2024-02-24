using Flex.Application.Authorize;
using Flex.Application.Contracts.Authorize;
using Flex.Application.Contracts.Exceptions;
using Flex.Application.Jwt;
using Flex.Core.Helper;
using Flex.Web.Handlers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Flex.Web.Jwt
{
    public static class JwtServiceExtensions
    {
        public static IServiceCollection AddJwtService(this IServiceCollection services, IConfiguration configuration)
        {
            //绑定appsetting中的jwtsetting
            services.Configure<JwtSetting>(configuration.GetSection(nameof(JwtSetting)));

            //注册jwtservice
            services.AddSingleton<JwtService>();
            //注册IHttpContextAccessor
            services.AddScoped<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IClaimsAccessor, ClaimsAccessor>();

            var jwtConfig = nameof(JwtSetting).Config<JwtSetting>();

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(o =>
                {
                    o.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                          {
                              if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                              {
                                  context.Response.Headers.Add("token-expired", "true");
                              }
                              return Task.CompletedTask;
                          },
                        OnChallenge = context =>
                        {
                            context.HandleResponse();
                            context.Response.StatusCode = ErrorCodes.NoOperationPermission.ToInt();
                            context.Response.ContentType = "application/json";
                            //无授权返回自定义信息
                            context.Response.WriteAsync(JsonHelper.ToJson(new Message<string> { code = ErrorCodes.NoOperationPermission.ToInt(), msg = ErrorCodes.NoOperationPermission.GetEnumDescription() }));
                            return Task.CompletedTask;
                        }
                    };
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.SecurityKey)),

                        ValidateIssuer = true,
                        ValidIssuer = jwtConfig.Issuer,

                        ValidateAudience = true,
                        ValidAudience = jwtConfig.Audience,

                        //总的Token有效时间 = JwtRegisteredClaimNames.Exp + ClockSkew ；
                        RequireExpirationTime = true,
                        ValidateLifetime = true,// 是否验证Token有效期，使用当前时间与Token的Claims中的NotBefore和Expires对比.同时启用ClockSkew 
                        ClockSkew = TimeSpan.Zero //注意这是缓冲过期时间，总的有效时间等于这个时间加上jwt的过期时间，如果不配置，默认是5分钟

                    };
                });
            services.AddScoped<IAuthorizationHandler, PermissionHandler>();
            services.AddAuthorization(options =>
             {

                 options.AddPolicy(AuthorizePolicy.Default, policy =>
                 {
                     policy.Requirements.Add(new PermissionRequirement());
                 });
             });
            return services;
        }
    }
}
