using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Web.Controllers;

namespace Web
{
    public class Order
    {
        public string? Id { get; set; }

        [property: Description("��������")]
        public DateOnly Date { get; set; }

        [property: Required]
        [property: DefaultValue(120)]
        [property: Description("�����۸�")]
        public int Price { get; set; }

        [property: Description("�����ۿۼ۸�")]
        public int PriceF => (int)(Price * 0.5556);

        [property: Description("��Ʒ����")]
        public string? Name { get; set; }


        [property: Description("����״̬")]
        public OrderStatus Status { get; set; }
    }
}
