
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
        //����ַ�������Ҫ�󣬲������д������ᱨ��
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
                //ȡ��˽Կ
                var secretByte = Encoding.UTF8.GetBytes(JwtSettingOption.Secret);
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    //��֤������
                    ValidateIssuer = true,
                    ValidIssuer = JwtSettingOption.Issuer,
                    //��֤������
                    ValidateAudience = true,
                    ValidAudiences = new List<string> { JwtSettingOption.Audience, JwtSettingOption.Audience },
                    //��֤�Ƿ����
                    ValidateLifetime = true,
                    //��֤˽Կ
                    IssuerSigningKey = new SymmetricSecurityKey(secretByte),
                    ClockSkew = TimeSpan.FromHours(1), //����ʱ���ݴ�ֵ�������������ʱ�䲻ͬ�����⣨�룩
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
                        Title = "����΢����",
                        Version = "v1",
                        Description = "������ؽӿ�"
                    };
                    return Task.CompletedTask;
                });

                options.AddSchemaTransformer((schema, context, cancellationToken) =>
                {
                    //�ҳ�ö������
                    if (context.JsonTypeInfo.Type.BaseType == typeof(Enum))
                    {
                        var list = new List<IOpenApiAny>();
                        //��ȡö����
                        foreach (var enumValue in schema.Enum.OfType<OpenApiString>())
                        {
                            //��ö����תΪö������
                            if (Enum.TryParse(context.JsonTypeInfo.Type, enumValue.Value, out var result))
                            {
                                //ͨ��ö����չ������ȡö������
                                var description = ((Enum)result).ToDescription();
                                //������֯ö��ֵչʾ�ṹ
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
