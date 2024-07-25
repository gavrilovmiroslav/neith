using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static NeithCore.Graph;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NeithCore
{
    public readonly struct Knot
    {
        public readonly Strand Self;
        public Knot() => Self = Knots.Make();
    }

    public readonly struct Arrow
    {
        public readonly Strand Self;
        public Arrow(Strand a, Strand b) => Self = (a, b).Join();
    }

    public static class ContextOperations
    {
        public static IO<Strand> Knot()
        {
            var knot = new Knot();
            return knot.ToIO<Knot, Strand>();
        }

        public static IO<Strand> Arrow(Strand a, Strand b)
        {
            var arrow = new Arrow(a, b);
            return arrow.ToIO<Arrow, Strand>();
        }
    }

    public class Graph
    {
        internal List<Strand> Strands = new();

        public override string ToString()
        {
            return string.Join("\n", Strands);
        }

        public Graph() { }

        public A Run<A>(IO<A> p)
        {
            return p switch
            {
                Return<A> r => r.Result,
                IO<Knot, Strand, A> x => Run(x.As(i => { this.Strands.Add(i.Self); return i.Self; })),
                IO<Arrow, Strand, A> x => Run(x.As(i => { this.Strands.Add(i.Self); return i.Self; })),
                _ => throw new NotSupportedException($"Not supported operation {p}"),
            };
        }

        public static implicit operator Graph(IO<Unit> d) => d.ToGraph();

        public IEnumerable<T> Select<T>(Func<Strand, T> f)
        {
            return this.Strands.Select(f);
        }

        public IEnumerable<T> SelectMany<T>(Func<Strand, IEnumerable<T>> f)
        {
            return this.Strands.SelectMany(f);
        }
    }

    public static class ContextExtensions
    {
        public static Graph ToGraph<A>(this IO<A> io)
        {
            var ctx = new Graph();
            ctx.Run(io);
            return ctx;
        }
    }
}
