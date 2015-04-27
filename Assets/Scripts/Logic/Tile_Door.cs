using System.Collections;

namespace MagicTower.Logic
{
    public enum EDoorType
    {
        Yellow = 0,
        Blue = 1,
        Red = 2,
        Trigger = 3,
        Fences = 4,
    }

    public class Tile_Door : Tile
    {
        public Tile_Door(EDoorType type)
            : base(EType.Door)
        {
            mDoorType = type;
        }

        private EDoorType mDoorType;
        public EDoorType DoorType
        {
            get { return mDoorType; }
            set { mDoorType = value; }
        }

        public override bool ValidateMove(Tile target)
        {
            switch (mDoorType)
            {
                case EDoorType.Fences:
                    {
                        return true;
                    }
                case EDoorType.Trigger:
                    {
                        return false;
                    }
                case EDoorType.Red:
                    {
                        return PlayerData.Instance.RedKeys > 0;
                    }
                case EDoorType.Blue:
                    {
                        return PlayerData.Instance.BlueKeys > 0;
                    }
                case EDoorType.Yellow:
                    {
                        return PlayerData.Instance.YellowKeys > 0;
                    }
                default:
                    {
                        return false;
                    }
            }
        }

        public override IEnumerator BeginTrigger(Tile target)
        {
            // use the key
            switch (mDoorType)
            {
                case EDoorType.Fences:
                    {
                        break;
                    }
                case EDoorType.Trigger:
                    {
                        break;
                    }
                case EDoorType.Red:
                    {
                        PlayerData.Instance.UseKey(EDoorKeyType.RedKey);
                        break;
                    }
                case EDoorType.Blue:
                    {
                        PlayerData.Instance.UseKey(EDoorKeyType.BlueKey);
                        break;
                    }
                case EDoorType.Yellow:
                    {
                        PlayerData.Instance.UseKey(EDoorKeyType.YellowKey);
                        break;
                    }
            }

            // TODO show the door open anim
            yield return true;
        }
    }
}
