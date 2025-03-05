using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace BMTLab.ImeiType.Tests.Benchmarks;

[SimpleJob(RuntimeMoniker.Net60, baseline: true)]
[SimpleJob(RuntimeMoniker.Net80)]
[SimpleJob(RuntimeMoniker.Net90)]
[MemoryDiagnoser]
[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public class NewRandomImeiBenchmark
{
    [Benchmark]
    public static Imei WithSeed() =>
        Imei.NewRandomImei(42);


    [Benchmark]
    public static Imei WithCryptoModule() =>
        Imei.NewRandomImei();
}