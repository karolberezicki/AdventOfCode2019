using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Day12
{
    [DebuggerDisplay("pos=<x={Position[Dimension.X]}, y= {Position[Dimension.Y]}, z= {Position[Dimension.Z]}>, vel=<x= {Velocity[Dimension.X]}, y= {Velocity[Dimension.Y]}, z= {Velocity[Dimension.Z]}>")]
    public class Moon
    {
        public Dictionary<Dimension, int> Position;
        public Dictionary<Dimension, int> Velocity;

        public Moon(int x, int y, int z)
        {
            Position = new Dictionary<Dimension, int>();
            Velocity = new Dictionary<Dimension, int>();

            Position[Dimension.X] = x;
            Position[Dimension.Y] = y;
            Position[Dimension.Z] = z;

            Velocity[Dimension.X] = 0;
            Velocity[Dimension.Y] = 0;
            Velocity[Dimension.Z] = 0;
        }

        public void ApplyGravity(Moon moon)
        {
            ApplyGravity(Dimension.X, moon);
            ApplyGravity(Dimension.Y, moon);
            ApplyGravity(Dimension.Z, moon);
        }

        public void ApplyGravity(Dimension dimension, Moon moon)
        {
            if (Position[dimension] < moon.Position[dimension])
            {
                Velocity[dimension] += 1;
                moon.Velocity[dimension] -= 1;
            }

            if (Position[dimension] > moon.Position[dimension])
            {
                Velocity[dimension] -= 1;
                moon.Velocity[dimension] += 1;
            }
        }

        public void ApplyVelocity(Dimension dimension)
        {
            Position[dimension] += Velocity[dimension];
        }

        public void ApplyVelocity()
        {
            ApplyVelocity(Dimension.X);
            ApplyVelocity(Dimension.Y);
            ApplyVelocity(Dimension.Z);
        }

        public int PotentialEnergy => Math.Abs(Position[Dimension.X]) + Math.Abs(Position[Dimension.Y]) + Math.Abs(Position[Dimension.Z]);
        public int KineticEnergy => Math.Abs(Velocity[Dimension.X]) + Math.Abs(Velocity[Dimension.Y]) + Math.Abs(Velocity[Dimension.Z]);
        public int TotalEnergy => PotentialEnergy * KineticEnergy;
    }
}