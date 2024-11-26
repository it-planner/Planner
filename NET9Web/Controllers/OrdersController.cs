using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;

namespace NET9Web.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<OrdersController> _logger;

        public OrdersController(ILogger<OrdersController> logger)
        {
            _logger = logger;
        }

        //获取
        [HttpGet(Name = "")]
        [Tags("幂等接口")]
        [EndpointDescription("获取订单列表")]
        public IEnumerable<Order> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new Order
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                Price = Random.Shared.Next(-20, 55),
                Name = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        //删除
        [HttpDelete(Name = "{id}")]
        [Tags("幂等接口")]
        [EndpointSummary("删除订单")]
        [EndpointDescription("根据订单id，删除相应订单")]
        public bool Delete(string id)
        {
            return true;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        [HttpPost(Name = "")]
        [Tags("非幂等接口")]
        public bool Post([FromBody] Order order)
        {
            return true;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="id"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        [HttpPut(Name = "{id}")]
        [Tags("非幂等接口")]
        public bool Put([Description("订单Id")] string id, [FromBody] Order order)
        {
            return true;
        }

        [HttpPost("upload/image")]
        [EndpointDescription("图片上传接口")]
        [DisableRequestSizeLimit]
        public bool UploadImgageAsync(IFormFile file)
        {
            return true;
        }

        /// <summary>
        /// 登录成功后生成token
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("login")]
        [EndpointDescription("登录成功后生成token")]
        [AllowAnonymous]
        public string  Login()
        {
            //登录成功返回一个token
            // 1.定义需要使用到的Claims
            var claims = new[] { new Claim("UserId", "test") };

            // 2.从 appsettings.json 中读取SecretKey
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSettingOption.Secret));
            // 3.选择加密算法
            var algorithm = SecurityAlgorithms.HmacSha256;
            // 4.生成Credentials
            var signingCredentials = new SigningCredentials(secretKey, algorithm);
            var now = DateTime.Now;
            var expires = now.AddMinutes(JwtSettingOption.Expires);
            // 5.根据以上，生成token
            var jwtSecurityToken = new JwtSecurityToken(
                JwtSettingOption.Issuer,         //Issuer
                JwtSettingOption.Audience,       //Audience
                claims,                          //Claims,
                now,                             //notBefore
                expires,                         //expires
                signingCredentials               //Credentials
            );
            // 6.将token变为string
            var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            return token;
        }
    }

    [JsonConverter(typeof(JsonStringEnumConverter<OrderStatus>))]
    public enum OrderStatus
    {
        [Description("等待处理")]
        Pending = 1,
        [Description("处理中")]
        Processing = 2,
        [Description("已发货")]
        Shipped = 3,
        [Description("已送达")]
        Delivered = 4,
    }
}
