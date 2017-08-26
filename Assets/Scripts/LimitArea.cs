using UnityEngine;

public struct LimitArea
{
	private Vector2 min, max;

	public LimitArea(Vector2 min, Vector2 max)
	{
		this.min = min;
		this.max = max;

		Resizing(min, max);
	}

	private LimitArea(LimitArea limitArea,
					  float marginTop, float marginRight,
					  float marginBottom, float marginLeft)
	{
		min = limitArea.min;
		max = limitArea.max;

		min.y += marginTop;
		min.x += marginLeft;

		max.y -= marginBottom;
		max.x -= marginRight;

		Resizing(min, max);
	}

	public LimitArea AddMargin(float margin)
	{
		return AddMargin(margin, margin, margin, margin);
	}

	public LimitArea AddMargin(float vertical, float horizontal)
	{
		return AddMargin(vertical, horizontal, vertical, horizontal);
	}

	public LimitArea AddMargin(float top, float right, float bottom, float left)
	{
		return new LimitArea(this, top, right, bottom, left);
	}

	public Vector2 Min { get { return min; } }
	public Vector2 Max { get { return max; } }

	public Vector3 Clamp(Vector2 value)
	{
		return Vector2.Max(Vector2.Min(value, max), min);
	}

	public bool Contains(Vector2 value) {
		return min.x <= value.x && value.x <= max.x &&
			      min.y <= value.y && value.y <= max.y;
	}

	public void Resizing()
	{
		Resizing(min, max);
	}

	private void Resizing(Vector2 value1, Vector2 value2)
	{
		this.min = Vector2.Min(value1, value2);
		this.max = Vector2.Max(value1, value2);
	}
}
