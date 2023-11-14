using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public static class Helper 
{
    public static class Vector
    {
        public static void Vector3sAreParallel(Vector3 vectorToCompare,ref Vector3 vectorToCheck, Vector3 incomingForceVector)
        {
            var compareVectorAndForceVectorDot =
                Vector3.Dot(vectorToCompare.normalized, incomingForceVector.normalized);

            var compareVectorAndCheckVector = Vector3.Dot(vectorToCompare.normalized, vectorToCheck.normalized);

            if (compareVectorAndForceVectorDot < 0 && compareVectorAndCheckVector < 0)
            {
                vectorToCheck = -vectorToCheck;
                Debug.Log("Reverse Vector");
            } else if (compareVectorAndForceVectorDot == 0 && compareVectorAndCheckVector == 0)
            {
                vectorToCheck = -vectorToCheck;
                Debug.Log("Reverse Vector");
            } else if (compareVectorAndForceVectorDot < 0 && compareVectorAndCheckVector > 0)
            {
                Debug.Log("Not Reverse Vector: equation{ compareVectorAndForceVectorDot < 0 && compareVectorAndCheckVector > 0 }");
                return;
            } else if (compareVectorAndForceVectorDot > 0 && compareVectorAndCheckVector < 0)
            {
                Debug.Log("Not Reverse Vector: equation{ compareVectorAndForceVectorDot > 0 && compareVectorAndCheckVector < 0 }");
                return;
            }
        }

        public static void VectorsSameDirection(Vector3 vectorA,ref Vector3 vectorB)
        {
            var dir = Vector3.Dot(vectorA, vectorB);

            if (dir > 0)
            {
                vectorB = -vectorB;
            }
            else
            {
                return;
            }
        }
    }
    
    public static class Angle
    {
        public static float CalculateVector2Angle360Deg(Vector2 a, Vector2 b)
        {
            a = a.normalized;
            b = b.normalized;

            float angle = Vector2.SignedAngle(a, b);

            if (angle < 0)
            {
                angle += 360f;
            }

            return angle;
        }
    }
    
    public static int NumberExtractor(string text)
    {
        Regex regex = new Regex(@"\d+");
        Match match = regex.Match(text);

        if (match.Success) {
            return int.Parse(match.Value);
        } else {
            return -1; 
        }
    }

    public static float CalculateAngle(Vector3 from, Vector3 to)
    {

        return Quaternion.FromToRotation(Vector3.up, to - from).eulerAngles.z;

    }

    public static Vector3 GetReflectProjectile(Vector3 inDirection, Vector3 normalVector3)
    {
        return Vector3.Reflect(inDirection, normalVector3);
    }
    
    public static void DrawWireArc(Vector3 position, Vector3 dir, float anglesRange, float radius, float maxSteps = 20)
    {
        var srcAngles = GetAnglesFromDir(position, dir);
        var initialPos = position;
        var posA = initialPos;
        var stepAngles = anglesRange / maxSteps;
        var angle = srcAngles - anglesRange / 2;
        for (var i = 0; i <= maxSteps; i++)
        {
            var rad = Mathf.Deg2Rad * angle;
            var posB = initialPos;
            posB += new Vector3(radius * Mathf.Cos(rad), radius * Mathf.Sin(rad), 0);

            Gizmos.DrawLine(posA, posB);

            angle += stepAngles;
            posA = posB;
        }
        Gizmos.DrawLine(posA, initialPos);
    }
    
    static float GetAnglesFromDir(Vector3 position, Vector3 dir)
    {
        var forwardLimitPos = position + dir;
        var srcAngles = Mathf.Rad2Deg * Mathf.Atan2(forwardLimitPos.z - position.z, forwardLimitPos.x - position.x);

        return srcAngles;
    }

    public static void AddIfNotExistsDict<TKey, TValue>(Dictionary<TKey, TValue> dict, TKey key, TValue value)
    {
        dict.TryAdd(key, value);
    }

    public static void PrintDict<TKey, TValue>(Dictionary<TKey, TValue> dict)
    {
        foreach (var pair in dict)
        {
            Debug.Log($"Key: {pair.Key}, Value: {pair.Value}");
        }
    }
}

