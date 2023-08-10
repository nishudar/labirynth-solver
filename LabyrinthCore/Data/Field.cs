using System;

namespace LabyrinthCore.Data;

public struct Field : IEquatable<Field>
{
    public FieldType FieldType;
    public int X;
    public int Y;

    public bool Equals(Field other) 
        => X == other.X && Y == other.Y;

    public override bool Equals(object? obj) 
        => obj is Field other && Equals(other);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = (int) FieldType;
            hashCode = (hashCode * 397) ^ X;
            hashCode = (hashCode * 397) ^ Y;
            return hashCode;
        }
    }

    public static bool operator ==(Field left, Field right) 
        => left.Equals(right);

    public static bool operator !=(Field left, Field right) 
        => !left.Equals(right);

    public override string ToString() 
        => $"({FieldType}_{X},{Y})";
};