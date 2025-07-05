using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace BMTLab.ImeiType.Tests.Benchmarks;

[SimpleJob(RuntimeMoniker.Net80, baseline: true)]
[SimpleJob(RuntimeMoniker.Net90)]
[MemoryDiagnoser]
[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
[SuppressMessage("Performance", "CA1822:Mark members as static")]
public class NewRandomImeiBenchmark
{
    [Benchmark]
    public Imei WithSeed() =>
        Imei.NewRandomImei(42);


    [Benchmark]
    public Imei WithCryptoModule() =>
        Imei.NewRandomImei();
}