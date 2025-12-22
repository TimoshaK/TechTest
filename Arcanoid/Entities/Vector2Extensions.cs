namespace Arkanoid.Entities
{
    public static class Vector2Extensions
    {
        public static float Length(this Vector2 vector)
        {
            return (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
        }

        public static Vector2 Normalized(this Vector2 vector)
        {
            float length = vector.Length();
            if (length > 0)
                return new Vector2(vector.X / length, vector.Y / length);
            return new Vector2(0, 0);
        }
    }
}