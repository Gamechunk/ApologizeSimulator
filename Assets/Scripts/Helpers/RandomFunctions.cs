using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public static class RandomFunctions
{
    /// <summary>
    /// Возвращает true если вероятность прокнула
    /// </summary>
    /// <returns></returns>
    public static bool CheckChance(int procChance)
    {
        if (procChance == 0)
            return false;
        else if (procChance == 100)
            return true;

        int maxChance = 100;
        int count = maxChance / procChance;
        int r = Random.Range(0, count);

        //проверяем на 0 так как при любой вероятности 0 будет равновероятен всем числам и будет присутствовать во всех выборках
        if (r == 0)
            return true;

        return false;
    }

    /// <summary>
    /// Возвращает рандомную точку, которая принадлежит коллайдеру
    /// </summary>
    /// <param name="boundConstant"></param>
    /// <param name="collider"></param>
    /// <returns></returns>
    public static Vector2 GetRandomPointInsideCollider(float boundConstant, Collider2D collider)
    {
        if (collider == null)
            return Vector2.zero;

        //take random point in room col with constant border
        Vector2 randomPointInRoomCol = new Vector2(
            Random.Range(0 + boundConstant, collider.bounds.size.x - boundConstant),
            Random.Range(0 + boundConstant, collider.bounds.size.y - boundConstant)
            );

        //смещаем систему отсчета с центра коллайдера в левый угол
        return (Vector2)collider.transform.position - (Vector2)collider.bounds.extents + randomPointInRoomCol;

    }

    public static int GetRandomAngle(int? prevAngle = null, int? amountOfPrevAngle = null)
    {

        if (prevAngle == null || amountOfPrevAngle == null)
            return Random.Range(0, 360);
        if (amountOfPrevAngle > 180 || amountOfPrevAngle < 1)
            Debug.LogError("Amount of angle > 180 or < 1. Set it to less 180 and more 1.");
        else if (prevAngle != null && amountOfPrevAngle != null)
        {
            int leftSide = (int)prevAngle;
            int rightSide = (int)(360 - prevAngle);

            if (leftSide > rightSide && leftSide > amountOfPrevAngle)
                return Random.Range(0, (int)(leftSide - amountOfPrevAngle));
            else if (rightSide > leftSide && rightSide > amountOfPrevAngle)
                return Random.Range((int)(leftSide + amountOfPrevAngle), 360);
        }

        return 0;
    }

    /// <summary>
    /// Возвращает рандомное число из диапозона.
    /// </summary>
    /// <param name="minValueInclusive">Min include value</param>
    /// <param name="maxValueExclusive">Max include value</param>
    /// <param name="alreadyUsedObjects">Если указать, то эти числа будут исключены из рандома</param>
    /// <returns></returns>
    public static int GetRandomInt(int minValueInclusive, int maxValueExclusive, List<int> alreadyUsedObjects)
    {
        int rand = UnityEngine.Random.Range(minValueInclusive, maxValueExclusive);

        if (alreadyUsedObjects.Contains(rand))
            rand = GetRandomInt(minValueInclusive, maxValueExclusive, alreadyUsedObjects);

        return rand;
    }

    public static float GetPercentedValueFromNumber(int percent, float number)
    {
        percent = Mathf.Clamp(percent, 0, 100);
        if (percent == 100)
            return number;
        else if (percent == 0)
            return 0;

        return (number * percent) / 100f;

    }

    public static Vector2 GetPointOnCircle(float radius, int angle)
    {
        angle /= 57;
        var x = radius * Mathf.Cos(angle);
        var y = radius * Mathf.Sin(angle);
        return new Vector2(x, y);
    }

}
