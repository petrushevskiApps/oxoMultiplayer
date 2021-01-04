using UnityEngine;


namespace Grid
{
    public class GridWorldSize
    {
        public float LowXPosition  { get; private set; } = Mathf.Infinity;
        public float HighXPosition { get; private set; } = Mathf.NegativeInfinity;
        public float LowYPosition  { get; private set; } = Mathf.Infinity;
        public float HighYPosition { get; private set; } = Mathf.NegativeInfinity;

        public void SetXPosition(float value)
        {
            if (value < LowXPosition) LowXPosition = value;
            if (value > HighXPosition) HighXPosition = value;
        }

        public void SetYPosition(float value)
        {
            if (value < LowYPosition) LowYPosition = value;
            if (value > HighYPosition) HighYPosition = value;
        }

        public Vector2 GetWorldSize()
        {
            float x = Mathf.Abs(LowXPosition) + Mathf.Abs(HighXPosition);
            float y = Mathf.Abs(LowYPosition) + Mathf.Abs(HighYPosition);

            return new Vector2(x, y);
        }
    }
}