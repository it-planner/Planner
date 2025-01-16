using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Web.Controllers;

namespace Web
{
    public class Order
    {
        public string? Id { get; set; }

        [property: Description("创建日期")]
        public DateOnly Date { get; set; }

        [property: Required]
        [property: DefaultValue(120)]
        [property: Description("订单价格")]
        public int Price { get; set; }

        [property: Description("订单折扣价格")]
        public int PriceF => (int)(Price * 0.5556);

        [property: Description("商品名称")]
        public string? Name { get; set; }


        [property: Description("订单状态")]
        public OrderStatus Status { get; set; }
    }
}
