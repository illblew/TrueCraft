using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrueCraft.API.World;
using TrueCraft.Core.TerrainGen.Noise;
using TrueCraft.Core.World;
using TrueCraft.API;
using TrueCraft.Core.Logic.Blocks;

namespace TrueCraft.Core.TerrainGen.Decorators
{
    public class PlantDecorator : IChunkDecorator
    {
        public void Decorate(IWorld world, IChunk chunk, IBiomeRepository biomes)
        {
            var noise = new Perlin();
            noise.Seed = world.Seed;
            var chanceNoise = new ClampNoise(noise);
            chanceNoise.MaxValue = 2;
            for (int x = 0; x < 16; x++)
            {
                for (int z = 0; z < 16; z++)
                {
                    var biome = biomes.GetBiome(chunk.Biomes[x * Chunk.Width + z]);
                    var blockX = MathHelper.ChunkToBlockX(x, chunk.Coordinates.X);
                    var blockZ = MathHelper.ChunkToBlockZ(z, chunk.Coordinates.Z);
                    var height = chunk.HeightMap[x * Chunk.Width + z];
                    if (noise.Value2D(blockX, blockZ) > 0)
                    {
                        var blockLocation = new Coordinates3D(x, height, z);
                        var plantPosition = blockLocation + Coordinates3D.Up;
                        if (chunk.GetBlockID(blockLocation) == biome.SurfaceBlock && plantPosition.Y < Chunk.Height)
                        {
                            var chance = chanceNoise.Value2D(blockX, blockZ);
                            if (chance < 1)
                            {
                                var bushNoise = chanceNoise.Value2D(blockX * 0.7, blockZ * 0.7);
                                var grassNoise = chanceNoise.Value2D(blockX * 0.3, blockZ * 0.3);
                                var brownMushroomNoise = chanceNoise.Value2D (blockX * 0.3, blockZ * 0.3);
                                var redMushroomNoise = chanceNoise.Value2D (blockX * 0.3, blockZ * 0.3);
                                if (biome.Plants.Contains(PlantSpecies.Deadbush) && bushNoise > 1 && chunk.GetBlockID(blockLocation) == SandBlock.BlockID)
                                {
                                    GenerateDeadBush(chunk, plantPosition);
                                    continue;
                                }
                                
                                if (biome.Plants.Contains(PlantSpecies.TallGrass) && grassNoise > 0.3 && grassNoise < 0.95)
                                {
                                    byte meta = (grassNoise > 0.3 && grassNoise < 0.45 && biome.Plants.Contains(PlantSpecies.Fern)) ? (byte)0x2 : (byte)0x1;
                                    GenerateTallGrass(chunk, plantPosition, meta);
                                    continue;
                                }
                                //I think these have conditions rather than this type of placement in the biome, not sure.
                                if (biome.Plants.Contains(PlantSpecies.BrownMushroom) && brownMushroomNoise > 0.03 && brownMushroomNoise < 0.04)
                                {
                                    GenerateBrownMushroom(chunk, plantPosition);
                                }

                                if (biome.Plants.Contains(PlantSpecies.RedMushroom) && redMushroomNoise > 0.02 && redMushroomNoise < 0.03)
                                {
                                    GenerateRedMushroom(chunk, plantPosition);
                                }
                            }
                            else
                            {
                                var flowerTypeNoise = chanceNoise.Value2D(blockX * 1.2, blockZ * 1.2);
                                if (biome.Plants.Contains(PlantSpecies.Rose) && flowerTypeNoise > 0.8 && flowerTypeNoise < 1.5)
                                {
                                    GenerateRose(chunk, plantPosition);
                                }
                                else if (biome.Plants.Contains(PlantSpecies.Dandelion) && flowerTypeNoise <= 0.8)
                                {
                                    GenerateDandelion(chunk, plantPosition);
                                }
                            }
                        }
                    }
                }
            }
        }

        void GenerateRose(IChunk chunk, Coordinates3D location)
        {
            chunk.SetBlockID(location, RoseBlock.BlockID);
        }

        void GenerateDandelion(IChunk chunk, Coordinates3D location)
        {
            chunk.SetBlockID(location, DandelionBlock.BlockID);
        }

        void GenerateTallGrass(IChunk chunk, Coordinates3D location, byte meta)
        {
            chunk.SetBlockID(location, TallGrassBlock.BlockID);
            chunk.SetMetadata(location, meta);
        }

        void GenerateDeadBush(IChunk chunk, Coordinates3D location)
        {
            chunk.SetBlockID(location, DeadBushBlock.BlockID);
        }

        void GenerateBrownMushroom(IChunk chunk, Coordinates3D location) 
        {
            chunk.SetBlockID (location, BrownMushroomBlock.BlockID);
        }

        void GenerateRedMushroom(IChunk chunk, Coordinates3D location)
        {
            chunk.SetBlockID (location, RedMushroomBlock.BlockID);
        }
    }
}
