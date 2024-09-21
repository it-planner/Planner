using AutoMapper;
using BenchmarkDotNet.Attributes;
using FastDeepCloner;
using Force.DeepCloner;
using MessagePack;
using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace CSharp
{
    public class MemberwiseCloneModel
    {
        public int Age { get; set; }
        public string Name { get; set; }
        public MemberwiseCloneModel Clone()
        {
            return (MemberwiseCloneModel)MemberwiseClone();
        }
    }

    public record class RecordWithModel
    {
        public int Age { get; set; }
        public string Name { get; set; }
    }

    public class CloneModel
    {
        public int Age { get; set; }
        public string Name { get; set; }
        public List<CloneModel> Models { get; set; }
    }

    [DataContract]
    [Serializable]
    public class DataContractModel
    {
        [DataMember]
        public int Age { get; set; }
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public List<DataContractModel> Models { get; set; }
    }

    public class DeepCopyILEmit<T>
    {
        private static readonly Dictionary<Type, Func<T, T>> _cacheILEmit = [];

        public static T ILEmit(T original)
        {
            var type = typeof(T);
            if (!_cacheILEmit.TryGetValue(type, out var func))
            {
                var dymMethod = new DynamicMethod($"{type.Name}DoClone", type, new Type[] { type }, true);
                var cInfo = type.GetConstructor(new Type[] { });
                var generator = dymMethod.GetILGenerator();
                var lbf = generator.DeclareLocal(type);
                generator.Emit(OpCodes.Newobj, cInfo);
                generator.Emit(OpCodes.Stloc_0);
                foreach (var field in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                {
                    generator.Emit(OpCodes.Ldloc_0);
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Ldfld, field);
                    generator.Emit(OpCodes.Stfld, field);
                }

                generator.Emit(OpCodes.Ldloc_0);
                generator.Emit(OpCodes.Ret);

                func = (Func<T, T>)dymMethod.CreateDelegate(typeof(Func<T, T>));
                _cacheILEmit.Add(type, func);
            }

            return func(original);
        }
    }

    public class DeepCopyExpressionTree<T>
    {
        private static readonly Dictionary<Type, Func<T, T>> _cacheExpressionTree = [];
        public static T ExpressionTree(T original)
        {
            var type = typeof(T);
            if (!_cacheExpressionTree.TryGetValue(type, out var func))
            {
                var originalParam = Expression.Parameter(type, "original");
                var clone = Expression.Variable(type, "clone");

                var expressions = new List<Expression>
                {
                    Expression.Assign(clone, Expression.New(type))
                };

                foreach (var prop in type.GetProperties())
                {
                    var originalProp = Expression.Property(originalParam, prop);
                    var cloneProp = Expression.Property(clone, prop);
                    expressions.Add(Expression.Assign(cloneProp, originalProp));
                }

                expressions.Add(clone);

                var lambda = Expression.Lambda<Func<T, T>>(Expression.Block(new[] { clone }, expressions), originalParam);
                func = lambda.Compile();
                _cacheExpressionTree.Add(type, func);
            }

            return func(original);
        }
    }
    public class DeepCopy
    {
        private static readonly IMapper _mapper;
        static DeepCopy()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DataContractModel, DataContractModel>();
                // 配置更多映射  
            });

            _mapper = config.CreateMapper();
        }
        public static void Run()
        {
            NativeMemberwiseClone();
            NativeRecordWith();
        }

        public static void NativeMemberwiseClone()
        {
            var original = new MemberwiseCloneModel();
            var clone = original.Clone();
            Console.WriteLine(original == clone);
            Console.WriteLine(ReferenceEquals(original, clone));
        }

        public static void NativeRecordWith()
        {
            var original = new RecordWithModel();
            var clone = original with { };
            Console.WriteLine(original == clone);
            Console.WriteLine(ReferenceEquals(original, clone));
        }
        public static void ManualPure()
        {
            var original = new CloneModel
            {
                Models =
                [
                    new()
                    {
                        Age= 1,
                        Name="1"
                    }
                ]
            };
            var clone = new CloneModel
            {
                Age = original.Age,
                Name = original.Name,
                Models = original.Models.Select(x => new CloneModel
                {
                    Age = x.Age,
                    Name = x.Name,
                }).ToList()
            };
            Console.WriteLine(original == clone);
            Console.WriteLine(ReferenceEquals(original, clone));
        }

        public static T SerializeByBinary<T>(T original)
        {
            //using (var memoryStream = new MemoryStream())
            //{
            //    var formatter = new BinaryFormatter();
            //    formatter.Serialize(memoryStream, original);
            //    memoryStream.Seek(0, SeekOrigin.Begin);
            //    return (T)formatter.Deserialize(memoryStream);
            //}
            return default;
        }
        public static T SerializeByMessagePack<T>(T original)
        {
            var bytes = MessagePackSerializer.Serialize(original);
            return MessagePackSerializer.Deserialize<T>(bytes);
        }
        public static T SerializeByDataContract<T>(T original)
        {
            using var stream = new MemoryStream();

            var serializer = new DataContractSerializer(typeof(T));
            serializer.WriteObject(stream, original);
            stream.Position = 0;

            return (T)serializer.ReadObject(stream);
        }
        public static T SerializeByXml<T>(T original)
        {
            using var ms = new MemoryStream();
            var s = new XmlSerializer(typeof(T));
            s.Serialize(ms, original);
            ms.Position = 0;
            return (T)s.Deserialize(ms);
        }
        public static T SerializeByTextJson<T>(T original)
        {
            var json = System.Text.Json.JsonSerializer.Serialize(original);
            return System.Text.Json.JsonSerializer.Deserialize<T>(json);
        }
        public static T SerializeByJsonNet<T>(T original)
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(original);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        }

        public static T ThirdPartyByAutomapper<T>(T original)
        {
            var clone = _mapper.Map<T, T>(original);
            return clone;
        }
        public static T ThirdPartyByDeepCloner<T>(T original)
        {
            return original.DeepClone();
        }
        public static T ThirdPartyByFastDeepCloner<T>(T original)
        {
            return (T)DeepCloner.Clone(original);
        }

        public static T Reflection<T>(T original)
        {
            var type = original.GetType();

            //如果是值类型、字符串或枚举，直接返回
            if (type.IsValueType || type.IsEnum || original is string)
            {
                return original;
            }

            //处理集合类型
            if (typeof(IEnumerable).IsAssignableFrom(type))
            {
                var listType = typeof(List<>).MakeGenericType(type.GetGenericArguments()[0]);
                var listClone = (IList)Activator.CreateInstance(listType);
                foreach (var item in (IEnumerable)original)
                {
                    listClone.Add(Reflection(item));
                }
                return (T)listClone;
            }

            //创建新对象
            var clone = Activator.CreateInstance(type);
            //处理字段
            foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                var fieldValue = field.GetValue(original);
                if (fieldValue != null)
                {
                    field.SetValue(clone, Reflection(fieldValue));
                }
            }

            //处理属性
            foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                if (property.CanRead && property.CanWrite)
                {
                    var propertyValue = property.GetValue(original);
                    if (propertyValue != null)
                    {
                        property.SetValue(clone, Reflection(propertyValue));
                    }
                }
            }

            return (T)clone;
        }
    }

    [MemoryDiagnoser]
    public class DeepCopyBenchmark
    {
        /// <summary>
        /// 测试次数
        /// </summary>
        private const int TestNumber = 10000;

        /// <summary>
        /// 数组长度
        /// </summary>
        private readonly int[] ArrayLengths = new int[3] { 100, 1000, 10000 };

        private readonly Dictionary<int, DataContractModel[]> Datas = [];


        [GlobalSetup]
        public void Setup()
        {
            var random = new Random();

            foreach (var al in ArrayLengths)
            {
                var nu = new (DataContractModel[] models, int target)[TestNumber];

                var list = new List<DataContractModel>();
                for (var i = 0; i < al; i++)
                {
                    var num = random.Next(1, al);
                    list.Add(new DataContractModel
                    {
                        Age = num++,
                        Name = num.ToString(),
                        Models =
                            [
                                new DataContractModel
                                {
                                    Age = num++,
                                    Name = num.ToString(),
                                },
                                new DataContractModel
                                {
                                    Age = num++,
                                    Name = num.ToString(),
                                },
                            ]
                    });
                }

                var models = list.ToArray();
                Datas.Add(al, models);
            }
        }

        [Benchmark]
        public void SerializeByBinary_100()
        {
            var res = Datas[100];
            Handle(res, models => _ = DeepCopy.SerializeByBinary(models));
        }
        [Benchmark]
        public void SerializeByMessagePack_100()
        {
            var res = Datas[100];
            Handle(res, models => _ = DeepCopy.SerializeByMessagePack(models));
        }
        [Benchmark]
        public void SerializeByDataContract_100()
        {
            var res = Datas[100];
            Handle(res, models => _ = DeepCopy.SerializeByDataContract(models));
        }
        [Benchmark]
        public void SerializeByXml_100()
        {
            var res = Datas[100];
            Handle(res, models => _ = DeepCopy.SerializeByXml(models));
        }
        [Benchmark]
        public void SerializeByTextJson_100()
        {
            var res = Datas[100];
            Handle(res, models => _ = DeepCopy.SerializeByTextJson(models));
        }
        [Benchmark]
        public void SerializeByJsonNet_100()
        {
            var res = Datas[100];
            Handle(res, models => _ = DeepCopy.SerializeByJsonNet(models));
        }
        [Benchmark]
        public void ThirdPartyByAutomapper_100()
        {
            var res = Datas[100];
            Handle(res, models => _ = DeepCopy.ThirdPartyByAutomapper(models));
        }
        [Benchmark]
        public void ThirdPartyByDeepCloner_100()
        {
            var res = Datas[100];
            Handle(res, models => _ = DeepCopy.ThirdPartyByDeepCloner(models));
        }
        [Benchmark]
        public void ThirdPartyByFastDeepCloner_100()
        {
            var res = Datas[100];
            Handle(res, models => _ = DeepCopy.ThirdPartyByFastDeepCloner(models));
        }
        [Benchmark]
        public void Reflection_100()
        {
            var res = Datas[100];
            Handle(res, models => _ = DeepCopy.Reflection(models));
        }
        [Benchmark]
        public void ILEmit_100()
        {
            var res = Datas[100];
            Handle(res, models => _ = DeepCopyILEmit<DataContractModel>.ILEmit(models));
        }
        [Benchmark]
        public void ExpressionTree_100()
        {
            var res = Datas[100];
            Handle(res, models => _ = DeepCopyExpressionTree<DataContractModel>.ExpressionTree(models));
        }
        [Benchmark]
        public void SerializeByBinary_1000()
        {
            var res = Datas[1000];
            Handle(res, models => _ = DeepCopy.SerializeByBinary(models));
        }
        [Benchmark]
        public void SerializeByMessagePack_1000()
        {
            var res = Datas[1000];
            Handle(res, models => _ = DeepCopy.SerializeByMessagePack(models));
        }
        [Benchmark]
        public void SerializeByDataContract_1000()
        {
            var res = Datas[1000];
            Handle(res, models => _ = DeepCopy.SerializeByDataContract(models));
        }
        [Benchmark]
        public void SerializeByXml_1000()
        {
            var res = Datas[1000];
            Handle(res, models => _ = DeepCopy.SerializeByXml(models));
        }
        [Benchmark]
        public void SerializeByTextJson_1000()
        {
            var res = Datas[1000];
            Handle(res, models => _ = DeepCopy.SerializeByTextJson(models));
        }
        [Benchmark]
        public void SerializeByJsonNet_1000()
        {
            var res = Datas[1000];
            Handle(res, models => _ = DeepCopy.SerializeByJsonNet(models));
        }
        [Benchmark]
        public void ThirdPartyByAutomapper_1000()
        {
            var res = Datas[1000];
            Handle(res, models => _ = DeepCopy.ThirdPartyByAutomapper(models));
        }
        [Benchmark]
        public void ThirdPartyByDeepCloner_1000()
        {
            var res = Datas[1000];
            Handle(res, models => _ = DeepCopy.ThirdPartyByDeepCloner(models));
        }
        [Benchmark]
        public void ThirdPartyByFastDeepCloner_1000()
        {
            var res = Datas[1000];
            Handle(res, models => _ = DeepCopy.ThirdPartyByFastDeepCloner(models));
        }
        [Benchmark]
        public void Reflection_1000()
        {
            var res = Datas[1000];
            Handle(res, models => _ = DeepCopy.Reflection(models));
        }
        [Benchmark]
        public void ILEmit_1000()
        {
            var res = Datas[1000];
            Handle(res, models => _ = DeepCopyILEmit<DataContractModel>.ILEmit(models));
        }
        [Benchmark]
        public void ExpressionTree_1000()
        {
            var res = Datas[1000];
            Handle(res, models => _ = DeepCopyExpressionTree<DataContractModel>.ExpressionTree(models));
        }
        [Benchmark]
        public void SerializeByBinary_10000()
        {
            var res = Datas[10000];
            Handle(res, models => _ = DeepCopy.SerializeByBinary(models));
        }
        [Benchmark]
        public void SerializeByMessagePack_10000()
        {
            var res = Datas[10000];
            Handle(res, models => _ = DeepCopy.SerializeByMessagePack(models));
        }
        [Benchmark]
        public void SerializeByDataContract_10000()
        {
            var res = Datas[10000];
            Handle(res, models => _ = DeepCopy.SerializeByDataContract(models));
        }
        [Benchmark]
        public void SerializeByXml_10000()
        {
            var res = Datas[10000];
            Handle(res, models => _ = DeepCopy.SerializeByXml(models));
        }
        [Benchmark]
        public void SerializeByTextJson_10000()
        {
            var res = Datas[10000];
            Handle(res, models => _ = DeepCopy.SerializeByTextJson(models));
        }
        [Benchmark]
        public void SerializeByJsonNet_10000()
        {
            var res = Datas[10000];
            Handle(res, models => _ = DeepCopy.SerializeByJsonNet(models));
        }
        [Benchmark]
        public void ThirdPartyByAutomapper_10000()
        {
            var res = Datas[10000];
            Handle(res, models => _ = DeepCopy.ThirdPartyByAutomapper(models));
        }
        [Benchmark]
        public void ThirdPartyByDeepCloner_10000()
        {
            var res = Datas[10000];
            Handle(res, models => _ = DeepCopy.ThirdPartyByDeepCloner(models));
        }
        [Benchmark]
        public void ThirdPartyByFastDeepCloner_10000()
        {
            var res = Datas[10000];
            Handle(res, models => _ = DeepCopy.ThirdPartyByFastDeepCloner(models));
        }
        [Benchmark]
        public void Reflection_10000()
        {
            var res = Datas[10000];
            Handle(res, models => _ = DeepCopy.Reflection(models));
        }
        [Benchmark]
        public void ILEmit_10000()
        {
            var res = Datas[10000];
            Handle(res, models => _ = DeepCopyILEmit<DataContractModel>.ILEmit(models));
        }
        [Benchmark]
        public void ExpressionTree_10000()
        {
            var res = Datas[10000];
            Handle(res, models => _ = DeepCopyExpressionTree<DataContractModel>.ExpressionTree(models));
        }
        private static void Handle(DataContractModel[] models, Func<DataContractModel, DataContractModel> func)
        {
            foreach (var model in models)
            {
                func(model);
            }
        }
    }
}
