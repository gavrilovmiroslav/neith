
using NeithCore;
using static NeithCore.Unit;
using static NeithCore.ContextOperations;

Graph ptn = from a in Knot()
            from b in Knot()
            from c in Knot()
            from ab in Arrow(a, b)
            from ac in Arrow(a, c)
            select graph;

Graph env = from a in Knot()
            from b in Knot()
            from c in Knot()
            from d in Knot()
            from ab in Arrow(a, b)
            from ac in Arrow(a, c)
            from db in Arrow(d, b)
            from dc in Arrow(d, c)
            select graph;

Console.WriteLine(ptn.Dot().SaveImage("pattern.png"));
Console.WriteLine(env.Dot().SaveImage("search.png"));
