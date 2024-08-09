using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Alrighty, this is the component you probably care most about, Arthrex reviewer, possibly Lewis :).
/// 
/// Here is my strategy. I believe there are algorithms that are ideal for ConwaysGame but I definitely
///  do not know them off the top of my head and the instructions asked me not to look anything up so
///  this is what I came up with.
///  
/// This relies on a HashSet, _onTiles, to track the tiles that are currently "on". Look up and removal from
///  this collection is O(1)
/// 
/// Each step:
///     1. We iterate over this _onTiles hashSet
///         a. We grab the neighbors of each on-tile using a utility class (TODO: there could be more optimization here, I'm thinking re-use as many neighbors as possible, but this would also require we check tiles in some ordered way and know which direction we've moved)
///         b. We use the _onTiles HashSet to count the number of "on" neighbors
///             i. According to rules and the number of on-neighbors, 
///                 1. the current on-tile either survives (we do nothing), or 
///                 2. it dies (we add it do the _tilesToChange hashSet to be dealt with later) [we can't change it now or we'll change the state for upcoming tiles]
///             ii. Off-tile neighbors are added to a dictionary (_adjacentOffTiles) or their entry is incremented if it already exists
///     2. We iterate through the _adjacentOffTiles dictionary
///         a. Iff an off-tile key in here has a value of exactly 3, we add it to the _tilesToChange hashSet to be dealt with later
///             i. NOTE: This is slightly clever because we do not need to get neighbors again for any off-tile; by using a Dict we effectively count
///                        the number of on-tile neighbors for these guys as we iterate over the _onTiles
///     3. We iterate over the _tilesToChange hashSet
///         a. For any tile in here, we call ToggleTile which adds/removes it from the _onTiles hashSet as needed (and updates visual Tile)
///             i. NOTE: this was originally a dict where I used a bool to track the pending state. But since lookup in _onTiles is O(1) I opted
///                         to check whether it is Contain()ed in the collection to save some memory since I have three potentially large collections going
///     4. We clear out the temporary collections (_adjacentOffTiles and _tilesToChange)
/// </summary>
public class TileManager : MonoBehaviour
{
    [Tooltip("Reference to the Tilemap used for this game")]
    public Tilemap Tilemap;
    [Tooltip("Reference to the GridSize variable controlling size of the simulation")]
    public IntValue GridSize;
    [Space]
    [Tooltip("Tile to display when a tile is ON")]
    public Tile OnTile;
    [Tooltip("Tile to display when a tile is OFF")]
    public Tile OffTile;
    

    // Okay, here's where things get fun

    // First, we're going to keep track of the tiles that are currently on. This will let us minimize the work we
    //  do each pass at the expense of some memory.
    private HashSet<Vector3Int> _onTiles = new HashSet<Vector3Int>();
    // This will track the neighbors of on-tiles. Since each on-tile will be checked once and log its neighbors once, we can use
    //  this dict to determine the next-step status of each off-tile as we check on-tiles with no additional work
    private Dictionary<Vector3Int, int> _adjacentOffTiles = new Dictionary<Vector3Int, int>();
    // We can't change tiles as we go because then we'd get weird updates for tiles we check later. So we remember all of them
    //  here to be changed at the end then cleared for the next step
    private HashSet<Vector3Int> _tilesToChange = new HashSet<Vector3Int>();

    // Slightly more efficient to cache these for neighbor checking later since we have to do that so much
    private int _minCoord = 1;
    private int _maxCoord = -1;

    // Vector3Int's are a non-nullable type and the HashSet wasn't playing nicely with a Vector3Int? type,
    //  so instead we'll create a single "_badCoord" Vector3Int and use a reference to this same struct in lieu of null
    private static Vector3Int _badCoord = new Vector3Int(0, 0, -1);

    /// <summary>
    /// Takes one step in the simulation according to game rules
    /// </summary>
    public void Step()
    {
        // We'll keep using the same array for neighbors and pass by ref, save some cleanup
        Vector3Int[] neighbors = new Vector3Int[8];

        // Determine what's up with all living tiles
        foreach (Vector3Int coord in _onTiles)
        {
            int livingNeighbors = 0;
            TileUtility.GetNeighbors(ref neighbors, coord, _minCoord, _maxCoord, ref _badCoord);

            foreach (Vector3Int neighbor in neighbors)
            {
                if (neighbor == _badCoord)
                    continue;

                // By iterating through on-tiles and having them log their adjacent off-tiles here,
                //  we can solve the problem of turning off-tiles ON by simply iterating through this wihout
                //  having to get neighbors again for them etc.
                if (!_onTiles.Contains(neighbor))
                {
                    if (_adjacentOffTiles.ContainsKey(neighbor))
                        _adjacentOffTiles[neighbor] += 1;
                    else
                        _adjacentOffTiles[neighbor] = 1;
                }
                // Living neighbor, just count it for this tile
                else
                    livingNeighbors++;
            }

            // This living tile needs to die
            if (livingNeighbors < 2 || livingNeighbors > 3)
                _tilesToChange.Add(coord);
            // else { //do nothing, this tile survives }
        }

        // Determine any off tiles that need to come on!
        foreach (Vector3Int coord in _adjacentOffTiles.Keys)
        {
            if (_adjacentOffTiles[coord] == 3)
                _tilesToChange.Add(coord);
            // else { // do nothing, this dead tile is still dead }
        }

        // Make changes
        foreach (Vector3Int coord in _tilesToChange)
        {
            ToggleTile(coord);
        }

        // Clear the temporary collections
        _adjacentOffTiles.Clear();
        _tilesToChange.Clear();
    }

    /// <summary>
    /// Given a tile coord, swaps the state of this tile
    /// O(1) performance thanks to HashSets
    /// </summary>
    public void ToggleTile(Vector3Int tileCoord)
    {
        // Tile is currently on
        if (_onTiles.Contains(tileCoord))
        {
            _onTiles.Remove(tileCoord);
            Tilemap.SetTile(tileCoord, OffTile);
            return;
        }

        // Tile is currently off
        _onTiles.Add(tileCoord);
        Tilemap.SetTile(tileCoord, OnTile);
    }

    /// <summary>
    /// Clears our tilemap and collections. Then, according to GridSize, paints a corresponding number of OFF-tiles
    /// </summary>
    public void ResetGrid()
    {
        _onTiles.Clear();
        _adjacentOffTiles.Clear();
        _tilesToChange.Clear();

        Tilemap.ClearAllTiles();

        // Slightly quirky coordinate system by default grid, these values work for both odd and even grid sizes
        _minCoord = -(GridSize.Value / 2);
        _maxCoord = ((GridSize.Value + 1) / 2) - 1;
        Vector3Int coord = new Vector3Int(0, 0, 0);

        for (int x = _minCoord; x <= _maxCoord; x++)
        {
            coord.x = x;
            for (int y = _minCoord; y <= _maxCoord; y++)
            {
                coord.y = y;
                Tilemap.SetTile(coord, OffTile);
            }
        }
    }

    /// <summary>
    /// This is used to initialize the _onTiles collection according to the current state of the tilemap.
    ///  This is expensive and just used at startup to be able to have a default "seed"/starting state
    ///  that I can easily paint
    /// </summary>
    private void InitializeFromTilemap()
    {
        _onTiles.Clear();
        _adjacentOffTiles.Clear();
        _tilesToChange.Clear();

        // Slightly quirky coordinate system by default grid, these values work for both odd and even grid sizes
        _minCoord = -(GridSize.Value / 2);
        _maxCoord = ((GridSize.Value + 1) / 2) - 1;
        Vector3Int coord = new Vector3Int(0, 0, 0);

        for (int x = _minCoord; x <= _maxCoord; x++)
        {
            coord.x = x;
            for (int y = _minCoord; y <= _maxCoord; y++)
            {
                coord.y = y;
                if (Tilemap.GetTile(coord) == OnTile)
                    _onTiles.Add(coord);
            }
        }
    }

    #region ADAPTIVE_RESIZING

    private void OnEnable()
    {
        GridSize.OnValueChanged += OnGridSizeChanged;
    }

    private void OnDisable()
    {
        GridSize.OnValueChanged -= OnGridSizeChanged;
    }

    /// <summary>
    /// Handles adding/removing tiles at runtime when the user changes the size of the grid
    /// </summary>
    private void OnGridSizeChanged(dynamic value)
    {
        if (value is int) //it is
        {
            int newMin = -(value / 2);
            int newMax = ((value + 1) / 2) - 1;

            // Are we adding or subtracting tiles?
            bool downSizing = newMax < _maxCoord;

            // The actual least and most indexes depend on if we're shrinking or not
            int min = downSizing ? _minCoord : newMin;
            int minBound = downSizing ? newMin-1 : _minCoord;
            int max = downSizing ? _maxCoord : newMax;
            int maxBound = downSizing ? newMax+1 : _maxCoord;

            Vector3Int coord = new Vector3Int(0, 0, 0);
            // Deal with top strip, including corners
            for (int x = min; x <= max; x++)
            {
                coord.x = x;
                for (int y = maxBound; y <= max; y++)
                {
                    coord.y = y;
                    Tilemap.SetTile(coord, downSizing ? null : OffTile);
                    // only have to consider this on downsizing, not up
                    if (downSizing)
                        if (_onTiles.Contains(coord))
                            _onTiles.Remove(coord);
                }
            }
            // Deal with right edge, excluding corners
            for (int x = maxBound; x <= max; x++)
            {
                coord.x = x;
                for (int y = minBound; y <= maxBound; y++)
                {
                    coord.y = y;
                    Tilemap.SetTile(coord, downSizing ? null : OffTile);
                    if (downSizing)
                        if (_onTiles.Contains(coord))
                            _onTiles.Remove(coord);
                }
            }
            // Deal with bottom strip, including corners
            for (int x = min; x <= max; x++)
            {
                coord.x = x;
                for (int y = min; y <= minBound; y++)
                {
                    coord.y = y;
                    Tilemap.SetTile(coord, downSizing ? null : OffTile);
                    if (downSizing)
                        if (_onTiles.Contains(coord))
                            _onTiles.Remove(coord);
                }
            }
            // Deal with left edge, excluding corners
            for (int x = min; x <= minBound; x++)
            {
                coord.x = x;
                for (int y = minBound; y <= maxBound; y++)
                {
                    coord.y = y;
                    Tilemap.SetTile(coord, downSizing ? null : OffTile);
                    if (downSizing)
                        if (_onTiles.Contains(coord))
                            _onTiles.Remove(coord);
                }
            }

            _minCoord = newMin;
            _maxCoord = newMax;
        }
    }

    #endregion

    private void Awake()
    {
        // Reset with notify
        GridSize.Value = GridSize.DefaultValue;
        InitializeFromTilemap();
    }
}
