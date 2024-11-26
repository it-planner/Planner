using NET9Web.Controllers;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NET9Web
{
    public class Order
    {
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
