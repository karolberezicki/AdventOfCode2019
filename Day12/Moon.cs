using System;
using System.Diagnostics;

namespace Day12
{
    [DebuggerDisplay("pos=<x={X}, y= {Y}, z= {Z}>, vel=<x= {Vx}, y= {Vy}, z= {Vz}>")]
    public class Moon
    {
        public Moon(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public int Vx { get; set; }
        public int Vy { get; set; }
        public int Vz { get; set; }

        public void ApplyGravity(Moon moon)
        {
            if (X < moon.X)
            {
                Vx += 1;
                moon.Vx -= 1;
            }

            if (X > moon.X)
            {
                Vx -= 1;
                moon.Vx += 1;
            }

            if (Y < moon.Y)
            {
                Vy += 1;
                moon.Vy -= 1;
            }

            if (Y > moon.Y)
            {
                Vy -= 1;
                moon.Vy += 1;
            }

            if (Z < moon.Z)
            {
                Vz += 1;
                moon.Vz -= 1;
            }

            if (Z > moon.Z)
            {
                Vz -= 1;
                moon.Vz += 1;
            }
        }

        public void ApplyVelocity()
        {
            X += Vx;
            Y += Vy;
            Z += Vz;
        }

        public int PotentialEnergy => Math.Abs(X) + Math.Abs(Y) + Math.Abs(Z);
        public int KineticEnergy => Math.Abs(Vx) + Math.Abs(Vy) + Math.Abs(Vz);
        public int TotalEnergy => PotentialEnergy * KineticEnergy;
    }
}