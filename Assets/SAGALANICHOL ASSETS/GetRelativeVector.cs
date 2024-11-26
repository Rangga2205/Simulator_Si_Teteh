// (c) Copyright HutongGames, LLC 2010-2021. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(ActionCategory.Input)]
    [Tooltip("Gets a world direction Vector from a Source Vector3. The result is calculated relative to another GameObject (usually the camera) and stored in a Result Vector3 variable.")]
    public class GetRelativeVector : FsmStateAction
    {
        [RequiredField]
        [Tooltip("The source vector that will be processed.")]
        public FsmVector3 sourceVector3;

        [Tooltip("Calculate the vector relative to this game object. Typically the camera.")]
        public FsmGameObject relativeTo;

        [UIHint(UIHint.Variable)]
        [Tooltip("Store the resulting vector relative to the specified GameObject. You can use this in {{Translate}} or other movement actions.")]
        public FsmVector3 resultVector3;

        public override void Reset()
        {
            sourceVector3 = null;
            relativeTo = null;
            resultVector3 = null;
        }

        public override void OnUpdate()
        {
            if (sourceVector3.Value != null && relativeTo.Value != null)
            {
                // Get the transform of the relativeTo GameObject
                var relativeTransform = relativeTo.Value.transform;

                // Calculate the vector relative to the specified GameObject
                var result = relativeTransform.TransformDirection(sourceVector3.Value);

                // Store the resulting vector
                resultVector3.Value = result;
            }
        }
    }
}
