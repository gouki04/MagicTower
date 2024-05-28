using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditorInternal;
using UnityEngine;

namespace MagicTower.Editor
{
    public partial class PrefabGeneratorWindow : EditorWindow
    {
        public class PlayerGenerator : Generator
        {
            public AnimationClip GenerateAnimationClip(string animation_type, string direction)
            {
                string animation_name = "player_" + animation_type + "_" + direction;

                string[] frames;
                if (animation_type == "idle")
                    frames = new string[] { animation_name };
                else if (animation_type == "move")
                    frames = new string[] {
                        "player_" + animation_type + "_" + direction,
                        "player_" + "idle" + "_" + direction,
                        "player_" + animation_type + "_" + direction + "_1",
                        "player_" + "idle" + "_" + direction
                    };
                else
                    frames = new string[] { animation_name };

                return Helper.CreateAnimationClip(animation_name, 1f, 30f,
                    frames, true,
                    string.Format("{0}{1}.anim", m_AnimationPath, animation_name));
            }

            public UnityEditor.Animations.AnimatorController GenerateAnimatorController(Dictionary<string, Dictionary<string, AnimationClip>> clips)
            {
                var animator_controller = Helper.CreateEmptyAnimatorController(m_AnimatorControllerPath + "player_controller.controller");
                animator_controller.AddParameter("moving", UnityEngine.AnimatorControllerParameterType.Bool);
                animator_controller.AddParameter("direction", UnityEngine.AnimatorControllerParameterType.Int);

                var layer = animator_controller.layers[0];
                var sm = layer.stateMachine;

                var states = new Dictionary<string, Dictionary<string, UnityEditor.Animations.AnimatorState>>();
                states["idle"] = new Dictionary<string, UnityEditor.Animations.AnimatorState>();
                states["move"] = new Dictionary<string, UnityEditor.Animations.AnimatorState>();

                foreach (var animation_types in clips)
                {
                    foreach (var clip in animation_types.Value)
                    {
                        var state = sm.AddState(clip.Value.name);
                        animator_controller.SetStateEffectiveMotion(state, clip.Value, layer.syncedLayerIndex);

                        states[animation_types.Key][clip.Key] = state;
                    }
                }

                AddTransition(sm, states["idle"]["left"], states["idle"]["right"], "direction", 3);
                AddTransition(sm, states["idle"]["up"], states["idle"]["right"], "direction", 3);
                AddTransition(sm, states["idle"]["down"], states["idle"]["right"], "direction", 3);

                AddTransition(sm, states["idle"]["left"], states["idle"]["up"], "direction", 0);
                AddTransition(sm, states["idle"]["right"], states["idle"]["up"], "direction", 0);
                AddTransition(sm, states["idle"]["down"], states["idle"]["up"], "direction", 0);

                AddTransition(sm, states["idle"]["right"], states["idle"]["left"], "direction", 2);
                AddTransition(sm, states["idle"]["up"], states["idle"]["left"], "direction", 2);
                AddTransition(sm, states["idle"]["down"], states["idle"]["left"], "direction", 2);

                AddTransition(sm, states["idle"]["left"], states["idle"]["down"], "direction", 1);
                AddTransition(sm, states["idle"]["up"], states["idle"]["down"], "direction", 1);
                AddTransition(sm, states["idle"]["right"], states["idle"]["down"], "direction", 1);

                AddTransition(sm, states["idle"]["left"], states["move"]["left"], "moving", true);
                AddTransition(sm, states["move"]["left"], states["idle"]["left"], "moving", false);

                AddTransition(sm, states["idle"]["up"], states["move"]["up"], "moving", true);
                AddTransition(sm, states["move"]["up"], states["idle"]["up"], "moving", false);

                AddTransition(sm, states["idle"]["down"], states["move"]["down"], "moving", true);
                AddTransition(sm, states["move"]["down"], states["idle"]["down"], "moving", false);

                AddTransition(sm, states["idle"]["right"], states["move"]["right"], "moving", true);
                AddTransition(sm, states["move"]["right"], states["idle"]["right"], "moving", false);

                AddTransition(sm, states["move"]["left"], states["move"]["right"], "direction", 3);
                AddTransition(sm, states["move"]["up"], states["move"]["right"], "direction", 3);
                AddTransition(sm, states["move"]["down"], states["move"]["right"], "direction", 3);

                AddTransition(sm, states["move"]["left"], states["move"]["up"], "direction", 0);
                AddTransition(sm, states["move"]["right"], states["move"]["up"], "direction", 0);
                AddTransition(sm, states["move"]["down"], states["move"]["up"], "direction", 0);

                AddTransition(sm, states["move"]["right"], states["move"]["left"], "direction", 2);
                AddTransition(sm, states["move"]["up"], states["move"]["left"], "direction", 2);
                AddTransition(sm, states["move"]["down"], states["move"]["left"], "direction", 2);

                AddTransition(sm, states["move"]["left"], states["move"]["down"], "direction", 1);
                AddTransition(sm, states["move"]["up"], states["move"]["down"], "direction", 1);
                AddTransition(sm, states["move"]["right"], states["move"]["down"], "direction", 1);

                AssetDatabase.SaveAssets();

                return animator_controller;
            }

            public override void Generate()
            {
                var clips = new Dictionary<string, Dictionary<string, AnimationClip>>();
                clips["idle"] = new Dictionary<string, AnimationClip>();
                clips["move"] = new Dictionary<string, AnimationClip>();

                clips["idle"]["left"] = GenerateAnimationClip("idle", "left");
                clips["idle"]["right"] = GenerateAnimationClip("idle", "right");
                clips["idle"]["up"] = GenerateAnimationClip("idle", "up");
                clips["idle"]["down"] = GenerateAnimationClip("idle", "down");

                clips["move"]["left"] = GenerateAnimationClip("move", "left");
                clips["move"]["right"] = GenerateAnimationClip("move", "right");
                clips["move"]["up"] = GenerateAnimationClip("move", "up");
                clips["move"]["down"] = GenerateAnimationClip("move", "down");

                var animator_controller = GenerateAnimatorController(clips);

                var go = new GameObject();
                go.name = "player";

                var sprite_renderer = go.AddComponent<SpriteRenderer>();
                sprite_renderer.sprite = Utils.SpriteSheetManager.Instance["player_idle_down"];

                var animator = go.AddComponent<Animator>();
                animator.runtimeAnimatorController = animator_controller;

                PrefabUtility.CreatePrefab(string.Format("{0}player.prefab", m_PrefabPath), go);

                DestroyImmediate(go);
            }

            private void AddTransition(UnityEditor.Animations.AnimatorStateMachine sm, UnityEditor.Animations.AnimatorState src, UnityEditor.Animations.AnimatorState dst, string param, int value)
            {
                var trans = src.AddTransition(dst);
                trans.AddCondition(AnimatorConditionMode.Equals, value, param);
            }

            private void AddTransition(UnityEditor.Animations.AnimatorStateMachine sm, UnityEditor.Animations.AnimatorState src, UnityEditor.Animations.AnimatorState dst, string param, bool value)
            {
                var trans = src.AddTransition(dst);
                trans.AddCondition(value ? AnimatorConditionMode.If : AnimatorConditionMode.IfNot,
                    0f, param);
            }
        }
    }
}
