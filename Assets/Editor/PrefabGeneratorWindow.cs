using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace MagicTower.Editor
{
    /// <summary>
    /// 基于文章《Unity2D研究院之自动生成动画、AnimationController、Prefab（一）》
    /// http://www.xuanyusong.com/archives/3243
    /// </summary>
    public partial class PrefabGeneratorWindow : EditorWindow
    {
        public abstract class Generator
        {
            protected string m_AnimationPath;
            protected string m_AnimatorControllerPath;
            protected string m_PrefabPath;

            public void Init(string animation_path, string animator_controller_path, string prefab_path)
            {
                m_AnimationPath = animation_path;
                m_AnimatorControllerPath = animator_controller_path;
                m_PrefabPath = prefab_path;
            }

            public abstract void Generate();
        }

        private MonsterGenerator m_MonsterGenerator;
        private PlayerGenerator m_PlayerGenerator;
        private TerrainGenerator m_TerrainGenerator;

        public string ANIMATION_PATH = "Assets/Resources/Animation/";
        public string ANIMATOR_CONTROLLER_PATH = "Assets/Resources/Animation/";
        public string PREFAB_PATH = "Assets/Resources/Prefabs/";

        [MenuItem("Window/Prefab Generator")]
        public static void ShowWindow()
        {
            Utils.SpriteSheetManager.Instance.Load("tile");

            EditorWindow.GetWindow(typeof(PrefabGeneratorWindow));
        }

        public PrefabGeneratorWindow()
        {
            m_MonsterGenerator = new MonsterGenerator();
            m_MonsterGenerator.Init(ANIMATION_PATH, ANIMATOR_CONTROLLER_PATH, PREFAB_PATH);

            m_PlayerGenerator = new PlayerGenerator();
            m_PlayerGenerator.Init(ANIMATION_PATH, ANIMATOR_CONTROLLER_PATH, PREFAB_PATH);

            m_TerrainGenerator = new TerrainGenerator();
            m_TerrainGenerator.Init(ANIMATION_PATH, ANIMATOR_CONTROLLER_PATH, PREFAB_PATH);
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("Monster");
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Begin Index");
            m_MonsterGenerator.BeginIndex = EditorGUILayout.IntField(m_MonsterGenerator.BeginIndex);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("End Index");
            m_MonsterGenerator.EndIndex = EditorGUILayout.IntField(m_MonsterGenerator.EndIndex);
            EditorGUILayout.EndHorizontal();

            if (!m_MonsterGenerator.IsGenerating) {
                if (GUILayout.Button("Generate")) {
                    m_MonsterGenerator.Generate();
                }
            }
            else {
                m_MonsterGenerator.Update();
            }

            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("Player");
            if (GUILayout.Button("Generate")) {
                m_PlayerGenerator.Generate();
            }

            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("Terrain");
            if (GUILayout.Button("Generate")) {
                m_TerrainGenerator.Generate();
            }

            EditorGUILayout.EndVertical();
        }
    }
}
