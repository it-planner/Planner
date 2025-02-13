using System.Threading;
using Perfolizer.Horology;

namespace CSharp
{
    public class ThreadMonitor
    {
        #region lock(值类型)
        public class LockValueTypeExample
        {
            private static readonly int _lock = 88;
            public void Method1()
            {
                try
                {
                    Monitor.Enter(_lock);

                    var threadId = Thread.CurrentThread.ManagedThreadId;
                    Console.WriteLine($"线程 {threadId} 通过 lock(值类型) 锁进入 Method1");
                    Console.WriteLine($"进入时间 {DateTime.Now:HH:mm:ss}");
                    Console.WriteLine($"开始休眠 5 秒");
                    Console.WriteLine($"------------------------------------");
                    Thread.Sleep(5000);
                }
                finally
                {
                    Console.WriteLine($"开始释放锁 {DateTime.Now:HH:mm:ss}");
                    Monitor.Exit(_lock);
                    Console.WriteLine($"完成锁释放 {DateTime.Now:HH:mm:ss}");
                }
            }
        }

        public static void LockValueTypeRun()
        {
            var example = new LockValueTypeExample();
            var thread1 = new Thread(example.Method1);
            thread1.Start();
        }

        #endregion

        #region try/finally
        public class LockBeforeExceptionExample
        {
            private static readonly object _lock = new object();
            public void Method1()
            {
                try
                {
                    if (new Random().Next(2) == 1)
                    {
                        Console.WriteLine($"在调用Monitor.Enter前发生异常");
                        throw new Exception("在调用Monitor.Enter前发生异常");
                    }
                    Monitor.Enter(_lock);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"捕捉到异常：{ex.Message}");
                }
                finally
                {
                    Console.WriteLine($"开始释放锁 {DateTime.Now:HH:mm:ss}");
                    Monitor.Exit(_lock);
                    Console.WriteLine($"完成锁释放 {DateTime.Now:HH:mm:ss}");
                }
            }
        }

        public static void LockBeforeExceptionRun()
        {
            var example = new LockBeforeExceptionExample();
            var thread1 = new Thread(example.Method1);
            thread1.Start();
        }

        #endregion

        #region try/finally
        public class LockSolveBeforeExceptionExample
        {
            private static readonly object _lock = new object();
            public void Method1()
            {
                var lockTaken = false;
                try
                {
                    if (new Random().Next(2) == 1)
                    {
                        Console.WriteLine($"在调用Monitor.Enter前发生异常");
                        throw new Exception("在调用Monitor.Enter前发生异常");
                    }
                    Monitor.Enter(_lock, ref lockTaken);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"捕捉到异常：{ex.Message}");
                }
                finally
                {
                    if (lockTaken)
                    {
                        Console.WriteLine($"开始释放锁 {DateTime.Now:HH:mm:ss}");
                        Monitor.Exit(_lock);
                        Console.WriteLine($"完成锁释放 {DateTime.Now:HH:mm:ss}");
                    }
                    else
                    {
                        Console.WriteLine($"未执行锁定，无需释放锁");
                    }
                }
            }
        }

        public static void LockSolveBeforeExceptionRun()
        {
            var example = new LockSolveBeforeExceptionExample();
            var thread1 = new Thread(example.Method1);
            thread1.Start();
        }

        #endregion


        #region TryEnter
        public class LockTryEnterExample
        {
            private static readonly object _lock = new object();
            public void Method1()
            {
                try
                {
                    Monitor.Enter(_lock);
                    Console.WriteLine($"Method1 | 获取锁成功，并锁定 5 秒");
                    Thread.Sleep(5000);
                }
                finally
                {
                    Monitor.Exit(_lock);
                }
            }
            public void Method2()
            {
                Console.WriteLine($"Method2 | 尝试获取锁");
                if (Monitor.TryEnter(_lock, 3000))
                {
                    try
                    {
                    }
                    finally
                    {
                    }
                }
                else
                {
                    Console.WriteLine($"Method2 | 3 秒内未获取到锁，自动退出锁");
                }
            }
            public void Method3()
            {
                Console.WriteLine($"Method3 | 尝试获取锁");
                if (Monitor.TryEnter(_lock, 7000))
                {
                    try
                    {
                        Console.WriteLine($"Method3 | 7 秒内获取到锁");
                    }
                    finally
                    {
                        Console.WriteLine($"Method3 |开始释放锁");
                        Monitor.Exit(_lock);
                        Console.WriteLine($"Method3 |完成锁释放");
                    }
                }
                else
                {
                    Console.WriteLine($"Method3 | 7 秒内未获取到锁，自动退出锁");
                }
            }
        }

        public static void LockTryEnterRun()
        {
            var example = new LockTryEnterExample();
            var thread1 = new Thread(example.Method1);
            var thread2 = new Thread(example.Method2);
            var thread3 = new Thread(example.Method3);
            thread1.Start();
            thread2.Start();
            thread3.Start();
        }

        #endregion

        #region 生产者-消费者模型
        public class LockProducerConsumerExample
        {
            private static Queue<int> queue = new Queue<int>();
            private static object _lock = new object();

            //生产者
            public  void Producer()
            {
                while (true)
                {
                    lock (_lock)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        if (queue.Count < 3)
                        {
                            var item = new Random().Next(100);
                            queue.Enqueue(item);
                            Console.WriteLine($"生产者，生产： {item}");
                            //唤醒消费者
                            Monitor.Pulse(_lock);  
                        }
                        else
                        {
                            //队列满时，生产者等待
                            Console.WriteLine($"队列已满，生产者等待中……");
                            Monitor.Wait(_lock);  
                        }
                    }

                    Thread.Sleep(500);
                }
            }

            //消费者
            public  void Consumer()
            {
                while (true)
                {
                    lock (_lock)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        if (queue.Count > 0)
                        {
                            var item = queue.Dequeue();
                            Console.WriteLine($"消费者，消费： {item}");
                            //唤醒生产者
                            Monitor.Pulse(_lock);  
                        }
                        else
                        {
                            //队列空时，消费者等待
                            Console.WriteLine($"队列已空，消费者等待中……");
                            Monitor.Wait(_lock);  
                        }
                    }
                    Thread.Sleep(10000);
                }
            }
        }

        public static void LockProducerConsumerRun()
        {
            var example = new LockProducerConsumerExample();
            var thread1 = new Thread(example.Producer);
            var thread2 = new Thread(example.Consumer);
            thread1.Start();
            thread2.Start();
            thread1.Join();
            thread2.Join();
        }

        #endregion

    }
}
