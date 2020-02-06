﻿using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

namespace ListPool.Benchmarks
{
    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [MemoryDiagnoser]
    [GcServer(true)]
    [GcConcurrent]
    public class ListPoolCreateAndAddAndEnumerateBenchmarks
    {
        [Params(50, 1000, 10_000)]
        public int N { get; set; }

        [Benchmark(Baseline = true)]
        public int List()
        {
            int count = 0;
            List<int> list = new List<int>(N);
            for (int i = 0; i < N; i++)
            {
                list.Add(i);
            }

            foreach (int item in list)
            {
                count += item;
            }

            return count;
        }

        [Benchmark]
        public int ListPool()
        {
            int count = 0;
            using ListPool<int> list = new ListPool<int>(N);
            for (int i = 0; i < N; i++)
            {
                list.Add(i);
            }

            foreach (int item in list)
            {
                count += item;
            }

            return count;
        }

        [Benchmark]
        public int ListPool_AsSpan()
        {
            int count = 0;
            using ListPool<int> list = new ListPool<int>(N);
            for (int i = 0; i < N; i++)
            {
                list.Add(i);
            }

            foreach (int item in list.AsSpan())
            {
                count += item;
            }

            return count;
        }

        [Benchmark]
        public int ValueListPool()
        {
            int count = 0;
            using ValueListPool<int> list = N < 1024
                ? new ValueListPool<int>(stackalloc int[N], ValueListPool<int>.SourceType.UseAsInitialBuffer)
                : new ValueListPool<int>(N);

            for (int i = 0; i < N; i++)
            {
                list.Add(i);
            }

            foreach (int item in list)
            {
                count += item;
            }

            return count;
        }

        [Benchmark]
        public int ValueListPool_AsSpan()
        {
            int count = 0;
            using ValueListPool<int> list = N < 1024
                ? new ValueListPool<int>(stackalloc int[N], ValueListPool<int>.SourceType.UseAsInitialBuffer)
                : new ValueListPool<int>(N);

            for (int i = 0; i < N; i++)
            {
                list.Add(i);
            }

            foreach (int item in list.AsSpan())
            {
                count += item;
            }

            return count;
        }
    }
}
