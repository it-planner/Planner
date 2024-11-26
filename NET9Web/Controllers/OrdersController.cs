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

        //��ȡ
        [HttpGet(Name = "")]
        [Tags("�ݵȽӿ�")]
        [EndpointDescription("��ȡ�����б�")]
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

        //ɾ��
        [HttpDelete(Name = "{id}")]
        [Tags("�ݵȽӿ�")]
        [EndpointSummary("ɾ������")]
        [EndpointDescription("���ݶ���id��ɾ����Ӧ����")]
        public bool Delete(string id)
        {
            return true;
        }

        /// <summary>
        /// ���
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        [HttpPost(Name = "")]
        [Tags("���ݵȽӿ�")]
        public bool Post([FromBody] Order order)
        {
            return true;
        }

        /// <summary>
        /// �޸�
        /// </summary>
        /// <param name="id"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        [HttpPut(Name = "{id}")]
        [Tags("���ݵȽӿ�")]
        public bool Put([Description("����Id")] string id, [FromBody] Order order)
        {
            return true;
        }

        [HttpPost("upload/image")]
        [EndpointDescription("ͼƬ�ϴ��ӿ�")]
        [DisableRequestSizeLimit]
        public bool UploadImgageAsync(IFormFile file)
        {
            return true;
        }

        /// <summary>
        /// ��¼�ɹ�������token
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("login")]
        [EndpointDescription("��¼�ɹ�������token")]
        [AllowAnonymous]
        public string  Login()
        {
            //��¼�ɹ�����һ��token
            // 1.������Ҫʹ�õ���Claims
            var claims = new[] { new Claim("UserId", "test") };

            // 2.�� appsettings.json �ж�ȡSecretKey
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSettingOption.Secret));
            // 3.ѡ������㷨
            var algorithm = SecurityAlgorithms.HmacSha256;
            // 4.����Credentials
            var signingCredentials = new SigningCredentials(secretKey, algorithm);
            var now = DateTime.Now;
            var expires = now.AddMinutes(JwtSettingOption.Expires);
            // 5.�������ϣ�����token
            var jwtSecurityToken = new JwtSecurityToken(
                JwtSettingOption.Issuer,         //Issuer
                JwtSettingOption.Audience,       //Audience
                claims,                          //Claims,
                now,                             //notBefore
                expires,                         //expires
                signingCredentials               //Credentials
            );
            // 6.��token��Ϊstring
            var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            return token;
        }
    }

    [JsonConverter(typeof(JsonStringEnumConverter<OrderStatus>))]
    public enum OrderStatus
    {
        [Description("�ȴ�����")]
        Pending = 1,
        [Description("������")]
        Processing = 2,
        [Description("�ѷ���")]
        Shipped = 3,
        [Description("���ʹ�")]
        Delivered = 4,
    }
}
