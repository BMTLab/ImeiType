using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace BMTLab.ImeiType.Tests.Benchmarks;

[SimpleJob(RuntimeMoniker.Net80, baseline: true)]
[SimpleJob(RuntimeMoniker.Net90)]
[MemoryDiagnoser]
public class NewRandomImeiBenchmark
{
    [Benchmark]
    public Imei WithSeed() =>
        Imei.NewRandomImei(42);


    [Benchmark]
    public Imei WithCryptoModule() =>
        Imei.NewRandomImei();
}