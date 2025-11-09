using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2S3
{
    public struct CatWeight
    {
        private float weight;
        public float Weight
        {
            get { return weight; }
            set { if ((value > 0) || (value < 50)) weight = value; else weight = 1; }
        }
        public CatWeight(float w)
        {
            weight = w;
        }
        public override string ToString()
        {
            return $"{Weight:F2} кг. ";
        }
        public void Deconstruct(out float weight)
        {
            weight = Weight;
        }
    }

}
