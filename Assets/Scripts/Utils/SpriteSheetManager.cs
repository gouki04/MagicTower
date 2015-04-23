using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public class SpriteSheetManager : Singleton<SpriteSheetManager>
    {
        public SpriteSheetManager()
        {

        }

        private Dictionary<string, SpriteSheet> mSpriteSheets = new Dictionary<string, SpriteSheet>();

        public SpriteSheet GetSpriteSheet(string filename)
        {
            SpriteSheet ret;
            if (mSpriteSheets.TryGetValue(filename, out ret))
            {
                return ret;
            }

            return null;
        }

        public bool IsSpriteSheetLoaded(string filename)
        {
            return GetSpriteSheet(filename) != null;
        }

        public bool Load(string filename)
        {
            if (IsSpriteSheetLoaded(filename))
                return true;

            var sprites = Resources.LoadAll<Sprite>(filename);
            if (sprites == null)
                return false;

            mSpriteSheets[filename] = new SpriteSheet(sprites);
            return true;
        }

        public Sprite GetSprite(string spriteName)
        {
            foreach (var sheet in mSpriteSheets)
            {
                Sprite ret;
                if (sheet.Value.TryGetValue(spriteName, out ret))
                    return ret;
            }

            return null;
        }

        public Sprite this[string spriteName]
        {
            get { return GetSprite(spriteName); }
        }
    }
}
