#if RELEASE
using BenchmarkDotNet.Running;

using BMTLab.ImeiType.Tests.Benchmarks;

BenchmarkRunner.Run<NewRandomImeiBenchmark>();
#else
Console.WriteLine("Skipping benchmarks test with DEBUG configuration");
#endif