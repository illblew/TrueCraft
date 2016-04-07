using System;
using TrueCraft.API.Logic;
using TrueCraft.API;

namespace TrueCraft.Core.Logic.Blocks
{

    public abstract class MushroomBlock : BlockProvider
    {
        public override double BlastResistance { get { return 0; } }

        public override double Hardness { get { return 0; } }

        public override SoundEffectClass SoundEffect
        {
            get
            {
                return SoundEffectClass.Grass;
            }
        }
    }

    public class BrownMushroomBlock : MushroomBlock
    {
        public static readonly byte BlockID = 0x27;
        
        public override byte ID { get { return 0x27; } }

        public override byte Luminance { get { return 1; } }

        public override bool Opaque { get { return false; } }
        
        public override string DisplayName { get { return "Brown Mushroom"; } }

        public override BoundingBox? BoundingBox { get { return null; } }

        public override BoundingBox? InteractiveBoundingBox
        {
            get
            {
                //These are incorrect
                return new BoundingBox(new Vector3(2 / 16.0, 0, 2 / 16.0), new Vector3(14 / 16.0, 1.0, 14 / 16.0));
            }
        }

        public override Tuple<int, int> GetTextureMap(byte metadata)
        {
            return new Tuple<int, int>(13, 1);
        }
    }

    public class RedMushroomBlock : MushroomBlock
    {
        public static readonly byte BlockID = 0x28;

        public override byte ID { get { return 0x28; } }

        public override byte Luminance { get { return 1; } }

        public override bool Opaque { get { return false; } }

        public override string DisplayName { get { return "Red Mushroom"; } }

        public override BoundingBox? BoundingBox { get { return null; } }

        public override BoundingBox? InteractiveBoundingBox
        {
            get
            {
                //These are incorrect.
                return new BoundingBox(new Vector3(4 / 16.0, 0, 4 / 16.0), new Vector3(12 / 16.0, 8 / 16.0, 12 / 16.0));
            }
        }

        public override Tuple<int, int> GetTextureMap(byte metadata)
        {
            return new Tuple<int, int>(12, 1);
        }
    }
}