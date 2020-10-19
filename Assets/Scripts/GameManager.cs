﻿using UnityEngine;


/// <summary>
/// Singleton class that exists as a GameObject in scene that sends out messages to all the game's systems.
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Singleton instance of the GameManager.
    /// </summary>
    public static GameManager Instance { get; private set; }

    #region Player Settings
    /// <summary>
    /// The current active player.
    /// </summary>
    public Player Player;
    /// <summary>
    /// The prefab to instantiate for the player character.
    /// </summary>
    public GameObject PlayerPrefab;
    /// <summary>
    /// The parent transform to put the player under.
    /// </summary>
    public Transform PlayerParent;
    /// <summary>
    /// The prefab for the block selector box.
    /// </summary>
    public GameObject BlockSelectorPrefab;
    /// <summary>
    /// The maximum distance that a block can be interacted with.
    /// </summary>
    public float BlockSelectionMaxDistance = 5f;
    /// <summary>
    /// The default player character height.
    /// </summary>
    public const float DefaultCharacterHeight = 1.8f;
    /// <summary>
    /// The player character height while crouching.
    /// </summary>
    public const float CrouchCharacterHeight = 0.9f;
    /// <summary>
    /// The rate to lerp from default height to crouching height for the player.
    /// </summary>
    public const float CrouchRate = 0.1f;
    /// <summary>
    /// The field of view of the camera while sprinting.
    /// </summary>
    public const float SprintFOV = 70f;
    /// <summary>
    /// The default field of view for the camera.
    /// </summary>
    public const float DefaultFOV = 60f;
    #endregion Player Settings

    #region Chunk Settings
    /// <summary>
    /// The layer that the game world geometry is on.
    /// </summary>
    public LayerMask LevelGeometryLayerMask;
    /// <summary>
    /// The prefab for the chunk objects.
    /// </summary>
    public GameObject ChunkPrefab;
    /// <summary>
    /// The transform to act as parent for all the chunks.
    /// </summary>
    public Transform ChunkParentTransform;
    /// <summary>
    /// The material used to render all the chunks.
    /// </summary>
    public Material ChunkMaterial;
    /// <summary>
    /// The size in blocks of each chunk. Chunks are this many blocks cubed.
    /// </summary>
    public int ChunkSize = 16;
    /// <summary>
    /// The number of chunks to generate per column.
    /// </summary>
    public int ChunksPerColumn = 16;
    /// <summary>
    /// The amount of chunks generated for the starting area is this value doubled minus 1 then cubed.
    /// </summary>
    public int StartingColumnRadius = 11;
    /// <summary>
    /// The radius of chunks to generate continously as the player explores the world.
    /// </summary>
    public int ActiveColumnRadius = 9;
    #endregion Chunk Settings

    #region Terrain Noise Settings
    /// <summary>
    /// The seed to use for all noise generation.
    /// </summary>
    public int Seed = 0;
    public enum NoiseCombinationEnum
    {
        Min,
        Max,
        Average,
        Just1,
        Just2
    }
    /// <summary>
    /// How to combine noise and noise2.
    /// </summary>
    public NoiseCombinationEnum NoiseCombination = GameManager.NoiseCombinationEnum.Average;
    /// <summary>
    /// The multiplier to add to the noise generation values.
    /// </summary>
    public float YMultiplier = 0.008f;
    /// <summary>
    /// The cutoff value to differentiate air from land.
    /// </summary>
    public float CutoffValue = 0.5f;

    /// <summary>
    /// The noise generator for the terrain.
    /// </summary>
    public FastNoiseLite NoiseGeneratorBase = new FastNoiseLite();
    /// <summary>
    /// The noise type to use for the noise generation.
    /// </summary>
    public FastNoiseLite.NoiseType NoiseTypeBase = FastNoiseLite.NoiseType.OpenSimplex2S;
    /// <summary>
    /// the fractal type to use for the noise generation.
    /// </summary>
    public FastNoiseLite.FractalType FractalTypeBase = FastNoiseLite.FractalType.FBm;
    /// <summary>
    /// The frequency of the noise generation.
    /// </summary>
    public float FrequencyBase = 0.0015f;
    /// <summary>
    /// The number of octaves to use if using fractalized noise generation.
    /// </summary>
    public int OctavesBase = 4;
    /// <summary>
    /// The lacunarity of the noise generation.
    /// </summary>
    public float LacunarityBase = 3f;
    /// <summary>
    /// The persistence of the noise generation.
    /// </summary>
    public float PersistenceBase = 0.8f;
    /// <summary>
    /// Should noise be inverted?
    /// </summary>
    public bool InvertBase = true;

    /// <summary>
    /// The noise generator for the terrain.
    /// </summary>
    public FastNoiseLite NoiseGeneratorRidged = new FastNoiseLite();
    /// <summary>
    /// The noise type to use for the noise generation.
    /// </summary>
    public FastNoiseLite.NoiseType NoiseTypeRidged = FastNoiseLite.NoiseType.OpenSimplex2S;
    /// <summary>
    /// the fractal type to use for the noise generation.
    /// </summary>
    public FastNoiseLite.FractalType FractalTypeRidged = FastNoiseLite.FractalType.Ridged;
    /// <summary>
    /// The frequency of the noise generation.
    /// </summary>
    public float FrequencyRidged = 0.002f;
    /// <summary>
    /// The number of octaves to use if using fractalized noise generation.
    /// </summary>
    public int OctavesRidged = 4;
    /// <summary>
    /// The lacunarity of the noise generation.
    /// </summary>
    public float LacunarityRidged = 2f;
    /// <summary>
    /// The persistence of the noise generation.
    /// </summary>
    public float PersistenceRidged = 0.5f;
    /// <summary>
    /// Should ridged noise be inverted?
    /// </summary>
    public bool InvertRidged = true;
    #endregion Terrain Noise Settings

    #region Cave Noise Settings
    /// <summary>
    /// Noise generator for cave worm position noise.
    /// </summary>
    public FastNoiseLite CaveWormPositionNoiseGenerator = new FastNoiseLite();
    /// <summary>
    /// Noise type for cave worm position noise generator.
    /// </summary>
    public FastNoiseLite.NoiseType CaveWormPositionNoiseType = FastNoiseLite.NoiseType.Value;
    /// <summary>
    /// The frequency of cave worm position noise generator.
    /// </summary>
    public float CaveWormPositionFrequency = 1f;
    /// <summary>
    /// Noise generator for cave worm direction noise.
    /// </summary>
    public FastNoiseLite CaveWormDirectionNoiseGenerator = new FastNoiseLite();
    /// <summary>
    /// Noise type for cave worm direction noise generator.
    /// </summary>
    public FastNoiseLite.NoiseType CaveWormDirectionNoiseType = FastNoiseLite.NoiseType.Perlin;
    /// <summary>
    /// The frequency of the cave worm direction noise generator.
    /// </summary>
    public float CaveWormDirectionFrequency = 0.1f;
    /// <summary>
    /// The minimum amount of cave worms to generate in each chunk.
    /// </summary>
    public int MinimumCaveWorms = 0;
    /// <summary>
    /// The maximum amount of cave worms to generate in each chunk.
    /// </summary>
    public int MaximumCaveWorms = 2;
    /// <summary>
    /// The maximum distance in chunks for each cave worm to span.
    /// </summary>
    public int MaxWormChunkDistance = 3;
    /// <summary>
    /// The maximum number of segments to create for each worm.
    /// </summary>
    public int MaxWormSegments = 50;
    /// <summary>
    /// The radius size of the cave generated by cave worms.
    /// </summary>
    public int CaveWormRadius = 3;
    #endregion Cave Noise Settings

    /// <summary>
    /// The minimum nuber of objects to keep in the object pooler.
    /// </summary>
    public int MinPooledObjects = 50;

    /// <summary>
    /// The file path where all opaque block textures are kept.
    /// </summary>
    public const string OpaqueBlockTexturePath = "Assets/Textures/Blocks/Opaque/";
    /// <summary>
    /// The path to save the texture atlas to.
    /// </summary>
    public const string TextureAtlasPath = "Assets/Textures/Blocks/Atlas/atlas.png";


    // Awake is called when the script instance is being loaded.
    private void Awake()
    {
        Instance = this;
        Logger.Start();
        this.ChunkParentTransform = new GameObject("Chunks").transform;
        this.UpdateNoiseGenerators();
        TextureAtlas.CreateAtlas();
    }

    // Start is called before the first frame update.
    private void Start()
    {
        World.WorldThreadStart();
    }

    // Update is called once per frame.
    private void Update()
    {
        World.MainThreadUpdate();
    }

    // Fixed update is called a fixed number of times per second.
    private void FixedUpdate()
    {
        
    }

    // OnApplicationQuit is called before the application is quit.
    private void OnApplicationQuit()
    {
        World.Quit();
        Logger.Quit();
    }

    /// <summary>
    /// Updates the noise generators by passing them the current noise generator values set in the inspector window.
    /// </summary>
    private void UpdateNoiseGenerators()
    {
        this.NoiseGeneratorBase.SetSeed(this.Seed);
        this.NoiseGeneratorBase.SetNoiseType(this.NoiseTypeBase);
        this.NoiseGeneratorBase.SetFractalType(this.FractalTypeBase);
        this.NoiseGeneratorBase.SetFrequency(this.FrequencyBase);
        this.NoiseGeneratorBase.SetFractalOctaves(this.OctavesBase);
        this.NoiseGeneratorBase.SetFractalLacunarity(this.LacunarityBase);
        this.NoiseGeneratorBase.SetFractalGain(this.PersistenceBase);

        this.NoiseGeneratorRidged.SetSeed(this.Seed);
        this.NoiseGeneratorRidged.SetNoiseType(this.NoiseTypeRidged);
        this.NoiseGeneratorRidged.SetFractalType(this.FractalTypeRidged);
        this.NoiseGeneratorRidged.SetFrequency(this.FrequencyRidged);
        this.NoiseGeneratorRidged.SetFractalOctaves(this.OctavesRidged);
        this.NoiseGeneratorRidged.SetFractalLacunarity(this.LacunarityRidged);
        this.NoiseGeneratorRidged.SetFractalGain(this.PersistenceRidged);

        this.CaveWormPositionNoiseGenerator.SetSeed(this.Seed);
        this.CaveWormPositionNoiseGenerator.SetNoiseType(this.CaveWormPositionNoiseType);
        this.CaveWormPositionNoiseGenerator.SetFractalType(FastNoiseLite.FractalType.None);
        this.CaveWormPositionNoiseGenerator.SetFrequency(this.CaveWormPositionFrequency);

        this.CaveWormDirectionNoiseGenerator.SetSeed(this.Seed);
        this.CaveWormDirectionNoiseGenerator.SetNoiseType(this.CaveWormDirectionNoiseType);
        this.CaveWormDirectionNoiseGenerator.SetFractalType(FastNoiseLite.FractalType.None);
        this.CaveWormDirectionNoiseGenerator.SetFrequency(this.CaveWormDirectionFrequency);
    }
}
