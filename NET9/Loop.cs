

using System;
using System.Drawing;
using System.Reflection;
using System.Runtime.ConstrainedExecution;

public class PartialExamples
{
    //.NET 9 之前
    public void Loop()
    {
        List<string> items = ["张三", "李四", "王五"];
        var idx = 0;
        foreach (var item in items)
        {
            idx++;
            Console.WriteLine($"第{idx}个人名字是：{item}");
        }
    }

    //.NET 9
    public void LoopNew()
    {
        List<string> items = ["张三", "李四", "王五"];
        //直接获取索引、元素
        foreach ((int Index, string Item) in items.Index())
        {
            Console.WriteLine($"第{Index + 1}个人名字是：{Item}");
        }
    }

    //.NET 9
    public void LoopNew2()
    {
        List<string> items = ["张三", "李四", "王五"];
        //先获取元组后，再获取索引、元素
        foreach (var item in items.Index())
        {
            Console.WriteLine($"第{item.Index + 1}个人名字是：{item.Item}");
        }
    }
}