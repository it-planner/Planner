using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;

namespace Web.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(ILogger<OrdersController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        [HttpPost("")]
        [Tags("非幂等接口")]
        public bool Post([FromBody] Order order)
        {
            var test = new Order();
            return order.Id == "897";
        }

        //获取
        [HttpGet(Name = "{id}")]
        [Tags("幂等接口")]
        [EndpointDescription("获取订单列表")]
        public Order Get(string id)
        {
            return new Order
            {
                Id = id,
                Date = DateOnly.FromDateTime(DateTime.Now),
                Name = "小红",
                Price = 5,
                Status = OrderStatus.Pending
            };
        }

        //删除
        [HttpDelete(Name = "{id}")]
        [Tags("幂等接口")]
        [EndpointSummary("删除订单")]
        [EndpointDescription("根据订单id，删除相应订单")]
        public bool Delete(string id)
        {
            var test = new Order();
            return "897" == id;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="id"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        [HttpPut(Name = "{id}")]
        [Tags("幂等接口")]
        public bool Put([Description("订单Id")] string id, [FromBody] Order order)
        {
            var test = new Order();
            return order.Id == "897";
        }



        /// <summary>
        /// 登录成功后生成token
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/login")]
        [EndpointDescription("登录成功后生成token")]
        [AllowAnonymous]
        public string Login()
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


        //[HttpPost("upload/image")]
        //[EndpointDescription("图片上传接口")]
        //[DisableRequestSizeLimit]
        //public bool UploadImgageAsync(IFormFile file)
        //{
        //    return true;
        //}
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
