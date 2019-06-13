using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MagicTower.Display
{
    public interface IUIDisplay
    {

    }

    public class TempUIDisplay : MonoBehaviour, IUIDisplay
    {
        private uint mAreaIdx = 0;

        private void TextArea(string text)
        {
            GUI.TextArea(new Rect(0f, mAreaIdx++ * 30f, 200f, 30f), text);
        }

        private void OnGUI()
        {
            mAreaIdx = 0;

            TextArea(string.Format("Hp = {0}", Logic.PlayerData.Instance.Hp));
            TextArea(string.Format("Gold = {0}", Logic.PlayerData.Instance.Gold));
            TextArea(string.Format("Exp = {0}", Logic.PlayerData.Instance.Exp));
            TextArea(string.Format("Atk = {0}", Logic.PlayerData.Instance.Attack));
            TextArea(string.Format("Def = {0}", Logic.PlayerData.Instance.Defend));

            TextArea(string.Format("Red Keys = {0}", Logic.PlayerData.Instance.RedKeys));
            TextArea(string.Format("Blue Keys = {0}", Logic.PlayerData.Instance.BlueKeys));
            TextArea(string.Format("Yellow Keys = {0}", Logic.PlayerData.Instance.YellowKeys));
        }
    }
}
