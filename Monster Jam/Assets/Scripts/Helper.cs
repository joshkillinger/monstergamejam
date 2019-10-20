using System;

public static class Helper
{
	public static float Epsilon = 0.001f;

    public static float ClampAngle(float angle)
    {
        if (angle > 180)
        {
            angle -= 360;
        }
        else if (angle < -180)
        {
            angle += 360;
        }
        return angle;
    }
}
