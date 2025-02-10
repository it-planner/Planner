using System.Threading;
using Perfolizer.Horology;

namespace CSharp
{
    public class ThreadLock
    {
        #region lock(this)
        public class LockThisExample
        {
            public void Method1()
            {
                lock (this)
                {
                    var threadId = Thread.CurrentThread.ManagedThreadId;
                    Console.WriteLine($"线程 {threadId} 通过 lock(this) 锁进入 Method1");
                    Console.WriteLine($"进入时间 {DateTime.Now:HH:mm:ss}");
                    Console.WriteLine($"开始休眠 5 秒");
                    Console.WriteLine($"------------------------------------");
                    Thread.Sleep(5000);
                }
            }

            public void Method2()
            {
                lock (this)
                {
                    var threadId = Thread.CurrentThread.ManagedThreadId;
                    Console.WriteLine($"线程 {threadId} 通过 lock(this) 锁进入 Method2");
                    Console.WriteLine($"进入时间 {DateTime.Now:HH:mm:ss}");
                }
            }
        }

        public static void LockThisRun()
        {
            var example = new LockThisExample();
            var thread1 = new Thread(example.Method1);
            var thread2 = new Thread(example.Method2);
            var thread3 = new Thread(() =>
            {
                lock (example)
                {
                    var threadId = Thread.CurrentThread.ManagedThreadId;
                    Console.WriteLine($"线程 {threadId} 通过 lock(实例) 锁进入 Method3");
                    Console.WriteLine($"进入时间 {DateTime.Now:HH:mm:ss}");
                    Console.WriteLine($"开始休眠 5 秒");
                    Console.WriteLine($"------------------------------------");
                    Thread.Sleep(5000);
                }
            });
            thread3.Start();
            thread1.Start();
            thread2.Start();
        }

        #endregion

        #region lock(public)
        public class PublicLock
        {
            public static readonly object Lock = new object();
        }

        public class LockPublic1Example
        {
            public void Method1()
            {
                lock (PublicLock.Lock)
                {
                    var threadId = Thread.CurrentThread.ManagedThreadId;
                    Console.WriteLine($"线程 {threadId} 通过 lock(公共对象) 锁进入 Public1");
                    Console.WriteLine($"进入时间 {DateTime.Now:HH:mm:ss}");
                    Console.WriteLine($"开始休眠 5 秒");
                    Console.WriteLine($"------------------------------------");
                    Thread.Sleep(5000);
                }
            }
        }

        public class LockPublic2Example
        {
            public void Method1()
            {
                lock (PublicLock.Lock)
                {
                    var threadId = Thread.CurrentThread.ManagedThreadId;
                    Console.WriteLine($"线程 {threadId} 通过 lock(公共对象) 锁进入 Public2");
                    Console.WriteLine($"进入时间 {DateTime.Now:HH:mm:ss}");
                }
            }
        }

        public static void LockPublicRun()
        {
            var example1 = new LockPublic1Example();
            var example2 = new LockPublic2Example();
            var thread1 = new Thread(example1.Method1);
            var thread2 = new Thread(example2.Method1);
            thread1.Start();
            thread2.Start();
        }

        #endregion


        #region lock(string)

        public class LockString1Example
        {
            public void Method1()
            {
                lock ("abc")
                {
                    var threadId = Thread.CurrentThread.ManagedThreadId;
                    Console.WriteLine($"线程 {threadId} 通过 lock(字符串) 锁进入 String1");
                    Console.WriteLine($"进入时间 {DateTime.Now:HH:mm:ss}");
                    Console.WriteLine($"开始休眠 5 秒");
                    Console.WriteLine($"------------------------------------");
                    Thread.Sleep(5000);
                }
            }
        }
        public class LockString2Example
        {
            public void Method1()
            {
                lock ("abc")
                {
                    var threadId = Thread.CurrentThread.ManagedThreadId;
                    Console.WriteLine($"线程 {threadId} 通过 lock(字符串) 锁进入 String2");
                    Console.WriteLine($"进入时间 {DateTime.Now:HH:mm:ss}");
                }
            }
        }

        public static void LockStringRun()
        {
            var example1 = new LockString1Example();
            var example2 = new LockString2Example();
            var thread1 = new Thread(example1.Method1);
            var thread2 = new Thread(example2.Method1);
            thread1.Start();
            thread2.Start();
        }
        #endregion


        #region lock(readonly)
        public class LockNotReadonlyExample
        {
            private object _lock = new object();
            public void Method1()
            {
                lock (_lock)
                {
                    _lock = new object();
                    var threadId = Thread.CurrentThread.ManagedThreadId;
                    Console.WriteLine($"线程 {threadId} 进入 Method1 , 时间 {DateTime.Now:HH:mm:ss}");
                    Console.WriteLine($"------------------------------------");
                    Thread.Sleep(5000);
                }
            }
        }

        public static void LockNotReadonlyRun()
        {
            var example = new LockNotReadonlyExample();
            var thread1 = new Thread(example.Method1);
            var thread2 = new Thread(example.Method1);
            var thread3 = new Thread(example.Method1);
            thread1.Start();
            thread2.Start();
            thread3.Start();

        }
        #endregion
        //避免死锁（Deadlock）

        //Lock(typeof(MyType))
        //https://www.cnblogs.com/hxwzwiy/archive/2012/03/22/2412313.html



        #region lock(static)
        public class LockStaticExample
        {
            //这是一个实例字段，意味着类的每个实例都会有一个独立的锁对象。
            //如果你希望类的每个实例有自己独立的锁来控制并发访问，这种方式更合适。
            private readonly object _lock1 = new object();

            //这是一个静态字段，意味着类的所有实例共享同一个锁对象。
            //如果你希望类的所有实例都共享同一个锁来同步对某个静态资源访问，这种方式更合适。
            private static readonly object _lock2 = new object();
            public void Method1()
            {
                lock (_lock1)
                {
                    // 临界区代码
                }
            }

            public void Method2()
            {
                lock (_lock2)
                {
                    // 临界区代码
                }
            }

            public static void Method3()
            {
                lock (_lock2)
                {
                    // 临界区代码
                }
            }
        }

        #endregion
    }
}
