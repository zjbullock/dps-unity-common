using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace DPS.Common {
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
    
    #nullable enable
    public static async Task<IList<T>> GetObjectListByAssetReferences<T>(List<AssetReference> assetReferences) where T: UnityEngine.Object
        {
            if (assetReferences.Count == 0)
            {
                return new List<T>();
            }

            Debug.Log($"Asset References Count: {assetReferences.Count}");
            AsyncOperationHandle<IList<T>> handle = Addressables.LoadAssetsAsync<T>(assetReferences, null, Addressables.MergeMode.Union);

            try {
                await handle.Task;
                // Execution resumes here once the operation is completed
                if (handle.Status == AsyncOperationStatus.Failed)
                {
                    Debug.LogError($"Failed to load scriptable objects: {assetReferences}");
                    return new List<T>();
                }
                
                return handle.Result; // Access the result

            } catch (Exception e)    {
                Debug.LogError($"Error loading: {e}");
            } finally   {
                handle.Release();
            }
            return new List<T>();     
        }

    public static async Task<GenericDictionary<string, T>> GetObjectDictionaryByAssetReferences<T>(List<AssetReferenceT<T>> assetReferences) where T: UnityEngine.Object
        {
            if (assetReferences.Count == 0)
            {
                return new GenericDictionary<string, T>();
            }

            GenericDictionary<string, T> loadedRefs = new();
            GenericDictionary<string, AsyncOperationHandle<T>> handles = new();
            List<Task> tasks = new();
            foreach(AssetReferenceT<T> assetRef in assetReferences)
            {
                try {
                    if (assetRef == null || !assetRef.RuntimeKeyIsValid())
                    {
                        continue;
                    }
                    if(handles.ContainsKey(assetRef.AssetGUID))
                    {
                        continue;
                    }
                    AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(assetRef);

                    handles.Add(assetRef.AssetGUID, handle);
                    tasks.Add(handle.Task);
                    
                } catch (Exception e)    {
                    Debug.LogError($"Error loading: {e}");
                }
            }

            await Task.WhenAll(tasks);
            foreach(var assetHandle in handles)
            {
                if (assetHandle.Value.Status == AsyncOperationStatus.Failed)
                {
                    Addressables.Release(assetHandle.Value);
                    continue;
                }
                if (!loadedRefs.ContainsKey(assetHandle.Key))
                {
                    loadedRefs.Add(new(assetHandle.Key, assetHandle.Value.Result));
                }
                Addressables.Release(assetHandle.Value);
            }


            return loadedRefs;
        }

        public static async Task<T?> GetObjectByAssetReference<T>(AssetReference assetReference) where T: UnityEngine.Object
    {
        if (assetReference == null || !assetReference.RuntimeKeyIsValid())
        {
            return null;
        }

        AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(assetReference);

        try {
            await handle.Task;
            // Execution resumes here once the operation is completed
            if (handle.Status == AsyncOperationStatus.Failed)
            {
                Debug.LogError($"Failed to load scriptable objects: {assetReference.AssetGUID}");
                return null;
            }
            
            return handle.Result; // Access the result

        } catch (Exception e)    {
            Debug.LogError($"Error loading: {e}");
        } finally   {
            handle.Release();
        }
        return null;     
    }
    #nullable disable

}
}