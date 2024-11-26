
using Ideal.Core.Common.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Any;
using NET9Web.Transformers;
using Scalar.AspNetCore;
using System;
using System.Text;
using System.Xml.Linq;

namespace NET9Web
{
    public class JwtSettingOption
    {
        //这个字符数量有要求，不能随便写，否则会报错
        public static string Secret { get; set; } = "123456789qwertyuiopasdfghjklzxcb";
        public static string Issuer { get; set; } = "asdfghjkkl";
        public static string Audience { get; set; } = "zxcvbnm";
        public static int Expires { get; set; } = 120;
        public static string RefreshAudience { get; set; } = "zxcvbnm.2024.refresh";
        public static int RefreshExpires { get; set; } = 10080;
    }


    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                //取出私钥
                var secretByte = Encoding.UTF8.GetBytes(JwtSettingOption.Secret);
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    //验证发布者
                    ValidateIssuer = true,
                    ValidIssuer = JwtSettingOption.Issuer,
                    //验证接收者
                    ValidateAudience = true,
                    ValidAudiences = new List<string> { JwtSettingOption.Audience, JwtSettingOption.Audience },
                    //验证是否过期
                    ValidateLifetime = true,
                    //验证私钥
                    IssuerSigningKey = new SymmetricSecurityKey(secretByte),
                    ClockSkew = TimeSpan.FromHours(1), //过期时间容错值，解决服务器端时间不同步问题（秒）
                    RequireExpirationTime = true,
                };
            });

            builder.Services.AddAuthentication().AddJwtBearer();

            builder.Services.AddOpenApi(options =>
            {
                options.AddDocumentTransformer((document, context, cancellationToken) =>
                {
                    document.Info = new()
                    {
                        Title = "订单微服务",
                        Version = "v1",
                        Description = "订单相关接口"
                    };
                    return Task.CompletedTask;
                });

                options.AddSchemaTransformer((schema, context, cancellationToken) =>
                {
                    //找出枚举类型
                    if (context.JsonTypeInfo.Type.BaseType == typeof(Enum))
                    {
                        var list = new List<IOpenApiAny>();
                        //获取枚举项
                        foreach (var enumValue in schema.Enum.OfType<OpenApiString>())
                        {
                            //把枚举项转为枚举类型
                            if (Enum.TryParse(context.JsonTypeInfo.Type, enumValue.Value, out var result))
                            {
                                //通过枚举扩展方法获取枚举描述
                                var description = ((Enum)result).ToDescription();
                                //重新组织枚举值展示结构
                                list.Add(new OpenApiString($"{enumValue.Value} - {description}"));
                            }
                            else
                            {
                                list.Add(enumValue);
                            }
                        }

                        schema.Enum = list;
                    }
                    return Task.CompletedTask;
                });

                options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
            });

            var app = builder.Build();
            app.MapScalarApiReference();
            app.MapOpenApi();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
