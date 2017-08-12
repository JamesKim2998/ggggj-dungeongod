using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder
{
    List<Coord> queue;
    Dir[][] from;

    DungeonFloor currMap;
    
	public void init () { 
        queue = new List<Coord>();
        from = new Dir[100][];
        for( int i = 0; i < 100; i ++ )
        {
            from[i] = new Dir[100];
        }
	}

    private void ResetCheck()
    {
        for( int i = 0; i < 100; i ++ )
        {
            for (int j = 0; j < 100; j ++ )
            {
                from[i][j] = Dir.Stay;
            }
        }
    }

    public void UpdateMapInfo(DungeonFloor currMap)
    {
        this.currMap = currMap;
    }

    private bool IsArrived(Coord curr, Coord dest)
    {
        return Coord.distance(curr, dest) == 0;
    }
	
    public Dir FindPath(Coord start, Coord dest)
    {
        int seed;
        Dir dir_Iter = Dir.Up;
        Coord front, temp;
        bool succeed = false;

        ResetCheck();
        queue.Clear();
        queue.Add(start);
        from[start.x][start.y] = Dir.Up;
 
        while(!succeed && queue.Count != 0)
        {
            seed = Random.Range(1, 5);
            front = queue[0];
            
            queue.RemoveAt(0);
            for (int i = 0; i < seed; i++)
            {
                dir_Iter = dir_Iter.Clockwise();
            }
            for (int i = 0; i < 4; i ++)
            {
                
                dir_Iter = dir_Iter.Clockwise();
                temp = front + dir_Iter.ToCoord();
                if (temp.x != Mathf.Clamp(temp.x, 0, 100)) continue;
                if (temp.y != Mathf.Clamp(temp.y, 0, 100)) continue;
                if (from[temp.x][temp.y] != Dir.Stay) continue;
                if (currMap.CheckWallExists(temp))
                {
                    from[temp.x][temp.y] = (Dir)10;
                    continue;
                }
               
                queue.Add(temp);
                from[temp.x][temp.y] = dir_Iter;
                if (IsArrived(temp, dest))
                {
                    succeed = true;
                    break;
                }
            }
        }
        
        temp = dest;
        dir_Iter = Dir.Stay;

        if (succeed)
        {
            while(!IsArrived(temp, start) )
            {
                dir_Iter = from[temp.x][temp.y];
                temp = temp + dir_Iter.Reverse().ToCoord();
            }
        }
        
        return dir_Iter;
        
    }
}
