using csdot.Attributes.DataTypes;
using GraphVizNet;

namespace NeithCore
{
    public static class DotExtension
    {
        public static string SaveImage(this csdot.Graph graph, string name = "graph.png")
        {
            var gv = new GraphViz();
            var filename = Environment.CurrentDirectory + $"\\{name}";
            gv.LayoutAndRenderDotGraph(graph.Print(), filename, "png");

            return filename;
        }

        public static string Print(this csdot.Graph graph) => graph.ElementToString(1);

        public static csdot.Graph Dot(this Graph graph)
        {
            var result = new csdot.Graph("G");
            result.strict = false;
            result.type = "digraph";

            var knotToDot = (Strand s) => new List<csdot.IDot> { new csdot.Node($"v{s.Id}") };
            var arrowToDot = (Strand s) => new List<csdot.IDot> {
            new csdot.Edge(new List<csdot.Transition> {
                new csdot.Transition($"v{s.Source}", EdgeOp.directed),
                new csdot.Transition($"v{s.Target}", EdgeOp.unspecified)
            })
        };
            var propToDot = (Strand s) => new List<csdot.IDot> {
            new csdot.Node($"v{s.Id}"),
            new csdot.Edge(new List<csdot.Transition> {
                new csdot.Transition($"v{s.Source}", EdgeOp.directed),
                new csdot.Transition($"v{s.Target}", EdgeOp.unspecified)
            })
        };

            graph.SelectMany(s =>
            {
                if (s.IsKnot()) return knotToDot(s);
                else if (s.IsArrow()) return arrowToDot(s);
                else return propToDot(s);
            }).ToList().ForEach(result.AddElement);

            return result;
        }
    }
}
