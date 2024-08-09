using UnityEngine;

public static class TileUtility
{
    /// <summary>
    /// Utility function for finding the neighboring coordinates of a given tile
    /// </summary>
    /// <returns>Array of coordinates: NorthWest, North, NorthEast, East, SouthEast, South, SouthWest, West, value will be null if neighbor is out of bounds</returns>
    public static void GetNeighbors(ref Vector3Int[] neighbors, Vector3Int tileCoord, int minCoord, int maxCoord, ref Vector3Int badCoord)
    {
        int neighborMask = 0b111_1_111_1; //Weird spacing is for the top three neighbors, right, bottom three, left

        // We don't have western neighbors!
        if (tileCoord.x == minCoord)
            neighborMask &= 0b011_1_110_0;

        // We don't have northern neighbors!
        if (tileCoord.y == maxCoord)
            neighborMask &= 0b000_1_111_1;

        // No eastern neighbors
        if (tileCoord.x == maxCoord)
            neighborMask &= 0b110_0_011_1;

        // No southern neighbors
        if (tileCoord.y == minCoord)
            neighborMask &= 0b111_1_000_1;

        // Vast majority of tiles will have 8 neighbors so we'll spend a quick check to skip this baloney
        if (neighborMask != 0b111_1_111_1)
        {
            int x = tileCoord.x;
            int y = tileCoord.y;
            
            // This is gross af but looked too unreadable when I was doing it in a loop and we only do this for (literal) edge cases
            // West
            neighbors[7] = (neighborMask % 2 != 0) ? new Vector3Int(x - 1, y, 0) : badCoord;
            neighborMask >>= 1;
            // Southern edge
            neighbors[6] = (neighborMask % 2 != 0) ? new Vector3Int(x - 1, y - 1, 0) : badCoord;
            neighborMask >>= 1;
            neighbors[5] = (neighborMask % 2 != 0) ? new Vector3Int(x, y - 1, 0) : badCoord;
            neighborMask >>= 1;
            neighbors[4] = (neighborMask % 2 != 0) ? new Vector3Int(x + 1, y - 1, 0) : badCoord;
            neighborMask >>= 1;
            // East
            neighbors[3] = (neighborMask % 2 != 0) ? new Vector3Int(x + 1, y, 0) : badCoord;
            neighborMask >>= 1;
            // Northern edge
            neighbors[2] = (neighborMask % 2 != 0) ? new Vector3Int(x + 1, y + 1, 0) : badCoord;
            neighborMask >>= 1;
            neighbors[1] = (neighborMask % 2 != 0) ? new Vector3Int(x, y + 1, 0) : badCoord;
            neighborMask >>= 1;
            neighbors[0] = (neighborMask % 2 != 0) ? new Vector3Int(x - 1, y + 1, 0) : badCoord;

            return;
        }

        GetAllNeighbors(tileCoord, ref neighbors);
    }

    // TODO: *Slightly* better than the unrolled version above bc lack of bit shifts and conditionals,
    //  BUT this is still ugly. Could maybe optimize by more cleverly re-using as many neighbors as we can
    //  depending on last coord to current coord?
    private static void GetAllNeighbors(Vector3Int tileCoord, ref Vector3Int[] neighbors)
    {
        int x = tileCoord.x;
        int y = tileCoord.y;
        // Northern, x coords go from -1 to +1
        for (int i = -1; i < 2; i++)
        {
            neighbors[i+1] = new Vector3Int(x + i, y + 1, 0);
        }
        // Eastern
        neighbors[3] = new Vector3Int(x + 1, y, 0);
        // Southern, x coords go from +1 to -1
        for (int i = 1; i > -2; i--)
        {
            neighbors[5 - i] = new Vector3Int(x + i, y - 1, 0);
        }
        // Western
        neighbors[7] = new Vector3Int(x - 1, y, 0);
    }
}
