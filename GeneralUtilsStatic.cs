using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GeneralUtilsStatic
{
#nullable enable
    public static string EnumStringCleaner(string? enumString)
    {
        if (enumString == null)
        {
            return "";
        }

        return enumString.Replace("_", " ");
    }

    public static Vector3 GetVerticalArc(Vector3 start, Vector3 end, float height, float currentStep, float maxSteps)
    {
        //Gets the center of the arc.
        Vector3 center = (start + end) / 2f;

        //Increases the height of the center
        center += new Vector3(0, height, 0);

        //The fraction of animationTime that has elapsed since the startTime
        float fracComplete = currentStep / maxSteps;

        Vector3 startToCenter = Vector3.Lerp(start, center, fracComplete);
        Vector3 centerToEnd = Vector3.Lerp(center, end, fracComplete);

        //Current slerped position
        return Vector3.Lerp(startToCenter, centerToEnd, fracComplete);
    }

    public static bool AnimationParameterExists(string animationTrigger, List<AnimatorControllerParameter> animatorControllerParameters)
    {
        foreach (AnimatorControllerParameter animatorControllerParameter in animatorControllerParameters)
        {
            if (animatorControllerParameter.name == animationTrigger)
            {
                return true;
            }
        }
        return false;
    }

   
    // public static bool VerifyPauseService()
    // {
    //     return PauseService.instance != null;
    // }
    // public static bool VerifySceneTransitionService()
    // {
    //     return SceneTransitionService.instance != null;
    // }

    public static int GetIncrementIndex(int index, int listCount, bool increment)
    {
        if (listCount == 0)
        {
            return 0;
        }

        return (index + listCount + (increment ? 1 : -1)) % listCount;
    }

    #nullable disable
    public static bool AreListsEqual<T>(List<T> list1, List<T> list2)
    {
        if (list1.Count != list2.Count)
        {
            return false;
        }

        for (int i = 0; i < list1.Count; i++)
        {
            if (list1[i] == null && list2[i] == null)
            {
                continue;
            }

            if ((list1[i] == null && list2[i] != null) || (list1[i] != null && list2[i] == null) || (!list1[i].Equals(list2[i])))
            {
                return false;
            }
        }

        return true;
    }

    public static int SubtractNoNegatives(int currentvalue, int valueToSubtract)
    {
        return (currentvalue - valueToSubtract + (int) Math.Abs(currentvalue - valueToSubtract)) / 2;
    }
    
}
