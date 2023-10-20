
#if ODIN_INSPECTOR_3
using System;
using Sirenix.OdinInspector;

namespace RazerCore.Utils.Attributes
{
    public class ColoredBoxGroupAttribute : PropertyGroupAttribute
    {
        public float R, G, B, A;
        public string LabelText;
        public bool Fold;

        public ColoredBoxGroupAttribute(string groupId) : base(groupId)
        {

        }

        public ColoredBoxGroupAttribute(string path, bool fold, float r, float g, float b, float a = 1f) : base(path)
        {
            this.R = r;
            this.G = g;
            this.B = b;
            this.A = a;

            LabelText = path[(path.LastIndexOf("/", StringComparison.Ordinal) + 1)..];
            Fold = fold;
        }

        public ColoredBoxGroupAttribute(string path, bool fold, bool randomColor) : base(path)
        {
            this.R = UnityEngine.Random.Range(0f, 1f);
            this.G = UnityEngine.Random.Range(0f, 1f);
            this.B = UnityEngine.Random.Range(0f, 1f);
            this.A = 1f;

            LabelText = path[(path.LastIndexOf("/", StringComparison.Ordinal) + 1)..];
            Fold = fold;
        }

        protected override void CombineValuesWith(PropertyGroupAttribute other)
        {
            ColoredBoxGroupAttribute otherAttr = (ColoredBoxGroupAttribute)other;

            this.R = Math.Max(otherAttr.R, this.R);
            this.G = Math.Max(otherAttr.G, this.G);
            this.B = Math.Max(otherAttr.B, this.B);
            this.A = Math.Max(otherAttr.A, this.A);
        }
    }
}
#endif
