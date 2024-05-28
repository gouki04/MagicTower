using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace MagicTower.Editor
{
    public class Helper
    {
        public static AnimationClip CreateAnimationClip(
            string animation_name,
            float animation_duration,
            float frame_rate,
            string[] frame_names,
            bool loop,
            string asset_path)
        {
            var clip = new AnimationClip();
            AnimationUtility.SetAnimationType(clip, ModelImporterAnimationType.Generic);

            var curve_binding = new EditorCurveBinding {
                type = typeof(SpriteRenderer),
                path = "",
                propertyName = "m_Sprite"
            };

            var key_frames = new ObjectReferenceKeyframe[frame_names.Length];

            var frame_time = animation_duration / frame_names.Length;
            for (var i = 0; i < frame_names.Length; ++i) {
                var sprite = Utils.SpriteSheetManager.Instance.GetSprite(frame_names[i]);

                key_frames[i] = new ObjectReferenceKeyframe {
                    time = frame_time * i,
                    value = sprite
                };
            }

            clip.frameRate = frame_rate;

            if (loop) {
                //设置为循环动画
                var serialized_clip = new SerializedObject(clip);
                var clip_settings =
                    new AnimationClipSettings(serialized_clip.FindProperty("m_AnimationClipSettings")) {
                        loopTime = true
                    };
                serialized_clip.ApplyModifiedProperties();
            }

            AnimationUtility.SetObjectReferenceCurve(clip, curve_binding, key_frames);
            AssetDatabase.CreateAsset(clip, asset_path);
            AssetDatabase.SaveAssets();

            return clip;
        }

        public static UnityEditor.Animations.AnimatorController CreateEmptyAnimatorController(string asset_path)
        {
            return UnityEditor.Animations.AnimatorController.CreateAnimatorControllerAtPath(asset_path);
        }

        public static UnityEditor.Animations.AnimatorController CreateSimpleAnimatorController(AnimationClip clip,
            string asset_path)
        {
            var animator_controller = CreateEmptyAnimatorController(asset_path);

            var layer = animator_controller.layers[0];
            var sm = layer.stateMachine;

            var state = sm.AddState(clip.name);
            animator_controller.SetStateEffectiveMotion(state, clip, layer.syncedLayerIndex);

            AssetDatabase.SaveAssets();

            return animator_controller;
        }

        private class AnimationClipSettings
        {
            SerializedProperty m_Property;

            private SerializedProperty Get(string property)
            {
                return m_Property.FindPropertyRelative(property);
            }

            public AnimationClipSettings(SerializedProperty prop)
            {
                m_Property = prop;
            }

            public float startTime
            {
                get => Get("m_StartTime").floatValue;
                set => Get("m_StartTime").floatValue = value;
            }

            public float stopTime
            {
                get => Get("m_StopTime").floatValue;
                set => Get("m_StopTime").floatValue = value;
            }

            public float orientationOffsetY
            {
                get => Get("m_OrientationOffsetY").floatValue;
                set => Get("m_OrientationOffsetY").floatValue = value;
            }

            public float level
            {
                get => Get("m_Level").floatValue;
                set => Get("m_Level").floatValue = value;
            }

            public float cycleOffset
            {
                get => Get("m_CycleOffset").floatValue;
                set => Get("m_CycleOffset").floatValue = value;
            }

            public bool loopTime
            {
                get => Get("m_LoopTime").boolValue;
                set => Get("m_LoopTime").boolValue = value;
            }

            public bool loopBlend
            {
                get => Get("m_LoopBlend").boolValue;
                set => Get("m_LoopBlend").boolValue = value;
            }

            public bool loopBlendOrientation
            {
                get => Get("m_LoopBlendOrientation").boolValue;
                set => Get("m_LoopBlendOrientation").boolValue = value;
            }

            public bool loopBlendPositionY
            {
                get => Get("m_LoopBlendPositionY").boolValue;
                set => Get("m_LoopBlendPositionY").boolValue = value;
            }

            public bool loopBlendPositionXZ
            {
                get => Get("m_LoopBlendPositionXZ").boolValue;
                set => Get("m_LoopBlendPositionXZ").boolValue = value;
            }

            public bool keepOriginalOrientation
            {
                get => Get("m_KeepOriginalOrientation").boolValue;
                set => Get("m_KeepOriginalOrientation").boolValue = value;
            }

            public bool keepOriginalPositionY
            {
                get => Get("m_KeepOriginalPositionY").boolValue;
                set => Get("m_KeepOriginalPositionY").boolValue = value;
            }

            public bool keepOriginalPositionXZ
            {
                get => Get("m_KeepOriginalPositionXZ").boolValue;
                set => Get("m_KeepOriginalPositionXZ").boolValue = value;
            }

            public bool heightFromFeet
            {
                get => Get("m_HeightFromFeet").boolValue;
                set => Get("m_HeightFromFeet").boolValue = value;
            }

            public bool mirror
            {
                get => Get("m_Mirror").boolValue;
                set => Get("m_Mirror").boolValue = value;
            }
        }
    }
}
