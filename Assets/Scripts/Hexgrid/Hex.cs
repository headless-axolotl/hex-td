using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#warning Incomplete implementation.

namespace Lotl.Hexgrid
{
    /// <summary>
    /// Axial coordinate system implementation for
    /// a "pointy" hexagonal grid coordinate.
    /// https://www.redblobgames.com/grids/hexagons/
    /// </summary>
    [Serializable]
    public struct Hex
    {
        public int q, r;

        public Hex(int q, int r)
        {
            this.q = q;
            this.r = r;
        }

        #region Operators

        public static Hex operator +(Hex lhs, Hex rhs)
            => new(lhs.q + rhs.q, lhs.r + rhs.r);

        public static Hex operator -(Hex hex)
            => new(-hex.q, -hex.r);

        public static Hex operator -(Hex lhs, Hex rhs)
            => lhs + (-rhs);


        public static Hex operator *(Hex lhs, int rhs)
            => new(lhs.q * rhs, lhs.r * rhs);

        public static Hex operator /(Hex lhs, int rhs)
            => new(lhs.q / rhs, lhs.r / rhs);


        public static bool operator ==(Hex lhs, Hex rhs)
            => (lhs.q == rhs.q) && (lhs.r == rhs.r);

        public static bool operator !=(Hex lhs, Hex rhs)
            => !(lhs == rhs);

        public override bool Equals(object obj)
        {
            return obj is Hex hex &&
                   q == hex.q &&
                   r == hex.r;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(q, r);
        }

        public override string ToString()
        {
            return $"Hex(q:{q}, r:{r}, s:{-q - r})";
        }

        #endregion

        #region Neighbours

        private static readonly Hex[] direction_vectors = new Hex[6]
        {
            new(+1, 0), new(+1, -1), new(0, +1),
            new(-1, 0), new(-1, +1), new(0, -1),
        };

        public static Hex GetNeighbourVector(Direction direction)
            => direction_vectors[(int)direction];

        public static List<Hex> GetNeighbourVectors()
            => direction_vectors.ToList();

        public static Hex GetNeighbouHex(Hex from, Direction direction)
            => from + GetNeighbourVector(direction);
        
        public enum Direction { R = 0, UR = 1, UL = 2, L = 3, DL = 4, DR = 5 };

        
        private static readonly Hex[] diagonal_vectors = new Hex[6]
        {
            new(+2, -1), new(-1, +2), new(+1, +1),
            new(-2, +1), new(+1, -2), new(-1, -1),
        };

        public static Hex GetDiagonalVector(Diagonal diagonal)
            => diagonal_vectors[(int)diagonal];

        public static List<Hex> GetDiagonalVectors()
            => diagonal_vectors.ToList();

        public static Hex GetDiagonalHex(Hex from, Diagonal diagonal)
            => from + GetDiagonalVector(diagonal);

        public enum Diagonal { UR = 0, U = 1, UL = 2, DL = 3, D = 4, DR = 5 };

        #endregion

        #region Helper Methods

        private static readonly float root3 = Mathf.Sqrt(3);
        private const float epsilon = 1e-5f;

        public static int Distance(Hex from, Hex to)
        {
            Hex difference = from - to;
            return (Mathf.Abs(difference.q)
                + Mathf.Abs(difference.q + difference.r)
                + Mathf.Abs(difference.r)) >> 1;
        }

        public static Vector2 HexToPixel(Hex hex, float size)
        {
            return new Vector2(
                size * (root3 * hex.q + root3 / 2 * hex.r),
                size * (                 3.0f / 2 * hex.r)
            );
        }

        public static Hex PixelToHex(Vector2 pixel, float size)
        {
            float x =  pixel.x / size / root3;
            float y = -pixel.y / size / root3;
            // Algorithm from Charles Chambers
            // with modifications and comments by Chris Cox 2023
            // <https://gitlab.com/chriscox/hex-coordinates>
            float t = root3 * y + 1;          // scaled y, plus phase
            float temp1 = Mathf.Floor(t + x); // (y+x) diagonal, this calc needs floor
            float temp2 = (t - x);           // (y-x) diagonal, no floor needed
            float temp3 = (2 * x + 1);       // scaled horizontal, no floor needed, needs +1 to get correct phase
            float qf = (temp1 + temp3) / 3.0f;  // pseudo x with fraction
            float rf = (temp1 + temp2) / 3.0f;  // pseudo y with fraction
            // pseudo x/y, quantized and thus requires floor:
            return new(Mathf.FloorToInt(qf), -Mathf.FloorToInt(rf));
        }

        [Obsolete("Use the PixelToHex method.", true)]
        public static Hex Round(Vector2 fractionHex)
        {
            float unroundedS = -fractionHex.x - fractionHex.y;
            
            int q = Mathf.RoundToInt(fractionHex.x);
            int r = Mathf.RoundToInt(fractionHex.y);
            int s = Mathf.RoundToInt(unroundedS);

            float qDifference = Mathf.Abs(fractionHex.x - q);
            float rDifference = Mathf.Abs(fractionHex.y - r);
            float sDifference = Mathf.Abs(unroundedS - s);

            if (qDifference > rDifference && qDifference > sDifference)
                q = -r - s;
            else if (rDifference > sDifference)
                r = -q - s;
            //else
            //    s = -q - r;

            return new(q, r);
        }

        public static List<Hex> LinearPath(Vector2 pixelFrom, Vector2 pixelTo, float size)
        {
            List<Hex> hexes = new();

            Hex from = PixelToHex(pixelFrom, size),
                to = PixelToHex(pixelTo, size);

            Vector2 offset = new(epsilon, 2 * epsilon);
            pixelFrom = HexToPixel(from, size) + offset;
            pixelTo   = HexToPixel(to, size);

            int hexCount = Distance(from, to);
            float step = 1.0f / hexCount + epsilon, t = 0.0f;

            Vector2 lerped;

            for (int i = 0; i <= hexCount; i++)
            {
                lerped = Vector2.Lerp(pixelFrom, pixelTo, t);
                hexes.Add(PixelToHex(lerped, size));
                t += step;
            }

            return hexes;
        }

        #endregion
    }
}
