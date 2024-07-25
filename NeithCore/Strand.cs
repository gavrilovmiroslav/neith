using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection.Metadata;

namespace NeithCore
{
    public readonly struct Strand
    {
        private static int s_StrandCount = 0;

        public static readonly int CurrentStrandId = s_StrandCount;
        public static int NextStrandId => s_StrandCount++;

        public int Id { get; init; }
        public int Source { get; init; }
        public int Target { get; init; }

        public override string ToString()
        {
            if (this.IsArrow()) return $"[{Source}] -({Id})-> [{Target}]";
            else if (this.IsProp()) return $"({Id})--> [{Target}]";
            else if (this.IsAction()) return $"[{Source}] -->({Id})";
            else return $"[({Id})]";
        }
    }

    public static class Knots
    {
        public static Strand Make()
        {
            var id = Strand.NextStrandId;
            return new Strand()
            {
                Id = id,
                Source = id,
                Target = id,
            };
        }
    }

    public static class StrandExtensions
    {
        #region PREDICATES (Is...)

        public static bool IsKnot(this Strand self) => self.Id == self.Source && self.Id == self.Target;
        public static bool IsArrow(this Strand self) => self.Id != self.Source && self.Id != self.Target;
        public static bool IsLoop(this Strand self) => self.IsArrow() && self.Source == self.Target;
        public static bool IsProp(this Strand self) => self.Id == self.Source && self.Id != self.Target;
        public static bool IsAction(this Strand self) => self.Id != self.Source && self.Id == self.Target;
        #endregion

        #region CONNECT (Make...)
        public static Strand Loop(this Strand self)
        {
            var id = Strand.NextStrandId;
            return new Strand()
            {
                Id = id,
                Source = self.Id,
                Target = self.Id,
            };
        }

        public static Strand Loop(this int self)
        {
            Debug.Assert(self < Strand.CurrentStrandId, $"Strand {self} is not valid: expected ID lower than {Strand.CurrentStrandId}.");
            var id = Strand.NextStrandId;
            return new Strand()
            {
                Id = id,
                Source = self,
                Target = self,
            };
        }

        public static Strand Join(this (Strand, Strand) pair)
        {
            return new Strand()
            {
                Id = Strand.NextStrandId,
                Source = pair.Item1.Id,
                Target = pair.Item2.Id,
            };
        }

        public static Strand Join(this (int, int) pair)
        {
            Debug.Assert(pair.Item1 < Strand.CurrentStrandId, $"Strand {pair.Item1} is not valid: expected ID lower than {Strand.CurrentStrandId}.");
            Debug.Assert(pair.Item2 < Strand.CurrentStrandId, $"Strand {pair.Item2} is not valid: expected ID lower than {Strand.CurrentStrandId}.");
            return new Strand()
            {
                Id = Strand.NextStrandId,
                Source = pair.Item1,
                Target = pair.Item2,
            };
        }

        public static Strand Prop(this Strand self)
        {
            var id = Strand.NextStrandId;
            return new Strand()
            {
                Id = id,
                Source = id,
                Target = self.Id,
            };
        }

        public static Strand Prop(this int self)
        {
            Debug.Assert(self < Strand.CurrentStrandId, $"Strand {self} is not valid: expected ID lower than {Strand.CurrentStrandId}.");
            var id = Strand.NextStrandId;
            return new Strand()
            {
                Id = id,
                Source = id,
                Target = self,
            };
        }

        public static Strand Action(this Strand self)
        {
            var id = Strand.NextStrandId;
            return new Strand()
            {
                Id = id,
                Source = self.Id,
                Target = id,
            };
        }

        public static Strand Action(this int self)
        {
            Debug.Assert(self < Strand.CurrentStrandId, $"Strand {self} is not valid: expected ID lower than {Strand.CurrentStrandId}.");
            var id = Strand.NextStrandId;
            return new Strand()
            {
                Id = id,
                Source = self,
                Target = id,
            };
        }
        #endregion
    }
}
