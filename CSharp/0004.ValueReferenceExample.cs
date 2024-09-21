using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp
{
    public class ValueReferenceExample
    {
        public static void Run()
        {
            NewReferenceByReferenceRun();
            //ChangeReferenceByReferenceRun();
            //ValueByReferenceRun();
            //NewReferenceByValueRun();
            //ChangeReferenceByValueRun();
            //ValueByValueRun();
        }


        public static void NewReferenceByReferenceRun()
        {
            Console.WriteLine($"引用类型按引用传递 - new 新实例");
            Console.WriteLine($"");
            var a = new Test
            {
                Age = 10
            };

            Console.WriteLine($"调用者-调用方法前 a.Age 值：{a.Age}");
            NewReferenceByReference(ref a);
            Console.WriteLine($"调用者-调用方法后 a.Age 值：{a.Age}");
        }

        public static void NewReferenceByReference(ref Test a)
        {
            Console.WriteLine($"    被调用方法-接收到 a.Age 值：{a.Age}");
            a = new Test
            {
                Age = 100
            };
            Console.WriteLine($"    被调用方法-new后 a.Age 值：{a.Age}");
        }

        public static void ChangeReferenceByReferenceRun()
        {
            Console.WriteLine($"引用类型按引用传递 - 修改实例成员");
            Console.WriteLine($"");
            var a = new Test
            {
                Age = 10
            };

            Console.WriteLine($"调用者-调用方法前 a.Age 值：{a.Age}");
            ChangeReferenceByReference(ref a);
            Console.WriteLine($"调用者-调用方法后 a.Age 值：{a.Age}");
        }

        public static void ChangeReferenceByReference(ref Test a)
        {
            Console.WriteLine($"    被调用方法-接收到 a.Age 值：{a.Age}");
            a.Age = a.Age + 1;
            Console.WriteLine($"    被调用方法-修改后 a.Age 值：{a.Age}");
        }

        public static void ValueByReferenceRun()
        {
            Console.WriteLine($"值类型按引用传递");
            Console.WriteLine($"");
            var a = 10;
            Console.WriteLine($"调用者-调用方法前 a 值：{a}");
            ChangeValueByReference(ref a);
            Console.WriteLine($"调用者-调用方法后 a 值：{a}");
        }

        public static void ChangeValueByReference(ref int a)
        {
            Console.WriteLine($"    被调用方法-接收到 a 值：{a}");
            a = a + 1;
            Console.WriteLine($"    被调用方法-修改后 a 值：{a}");
        }

        public static void NewReferenceByValueRun()
        {
            Console.WriteLine($"引用类型按值传递 - new 新实例");
            Console.WriteLine($"");
            var a = new Test
            {
                Age = 10
            };

            Console.WriteLine($"调用者-调用方法前 a.Age 值：{a.Age}");
            NewReferenceByValue(a);
            Console.WriteLine($"调用者-调用方法后 a.Age 值：{a.Age}");
        }

        public static void NewReferenceByValue(Test a)
        {
            Console.WriteLine($"    被调用方法-接收到 a.Age 值：{a.Age}");
            a = new Test
            {
                Age = 100
            };
            Console.WriteLine($"    被调用方法-new后 a.Age 值：{a.Age}");
        }


        public static void ChangeReferenceByValueRun()
        {
            Console.WriteLine($"引用类型按值传递 - 修改实例成员");
            Console.WriteLine($"");
            var a = new Test
            {
                Age = 10
            };

            Console.WriteLine($"调用者-调用方法前 a.Age 值：{a.Age}");
            ChangeReferenceByValue(a);
            Console.WriteLine($"调用者-调用方法后 a.Age 值：{a.Age}");
        }

        public static void ChangeReferenceByValue(Test a)
        {
            Console.WriteLine($"    被调用方法-接收到 a.Age 值：{a.Age}");
            a.Age = a.Age + 1;
            Console.WriteLine($"    被调用方法-修改后 a.Age 值：{a.Age}");
        }

        public static void ValueByValueRun()
        {
            Console.WriteLine($"值类型按值传递");
            Console.WriteLine($"");
            var a = 10;
            Console.WriteLine($"调用者-调用方法前 a 值：{a}");
            ChangeValueByValue(a);
            Console.WriteLine($"调用者-调用方法后 a 值：{a}");
        }

        public static void ChangeValueByValue(int a)
        {
            Console.WriteLine($"    被调用方法-接收到 a 值：{a}");
            a = a + 1;
            Console.WriteLine($"    被调用方法-修改后 a 值：{a}");
        }
    }

    public class Test
    {
        public int Age { get; set; }
    }
}
