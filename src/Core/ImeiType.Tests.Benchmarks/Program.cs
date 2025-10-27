#if RELEASE

using BenchmarkDotNet.Running;

using BMTLab.ImeiType.Tests.Benchmarks;

BenchmarkRunner.Run<NewRandomImeiBenchmark>();
#else
await Console.Error.WriteLineAsync("Skipping benchmarks test with DEBUG configuration");
#endif