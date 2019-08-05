using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project2
{
    class TileSettings
    {

        public struct TileSettngsStruct
        {
            public TileType drop_type;
            public bool CanBreak;
            public int TimeBreak;
            public bool CanWallk;
            public bool CanBuild;
            public int CountInStack;
            public bool CanOpen;
        }

        public static TileSettngsStruct[] tilesettings = new TileSettngsStruct[64];

       public static void load()
        {
            /*
             1) добавить парамерты 
             2) добавить case в Tile.cs
             3) добавить case в Inventory.cs
             4) изменить размер массива tilesettings
             */
            tilesettings[(int)TileType.AIR].drop_type = TileType.AIR;
            tilesettings[(int)TileType.AIR].CanBreak =false;
            tilesettings[(int)TileType.AIR].TimeBreak =0 ;
            tilesettings[(int)TileType.AIR].CanWallk = true;
            tilesettings[(int)TileType.AIR].CanBuild = true;
            tilesettings[(int)TileType.AIR].CountInStack = 1;

            tilesettings[(int)TileType.DIRT].drop_type = TileType.DIRT;
            tilesettings[(int)TileType.DIRT].CanBreak = true;
            tilesettings[(int)TileType.DIRT].TimeBreak = 10;
            tilesettings[(int)TileType.DIRT].CanWallk = false;
            tilesettings[(int)TileType.DIRT].CanBuild = false;
            tilesettings[(int)TileType.DIRT].CountInStack = 64;

            tilesettings[(int)TileType.GRASS].drop_type = TileType.DIRT;
            tilesettings[(int)TileType.GRASS].CanBreak =true;
            tilesettings[(int)TileType.GRASS].TimeBreak = 10;
            tilesettings[(int)TileType.GRASS].CanWallk = false;
            tilesettings[(int)TileType.GRASS].CanBuild = false;
            tilesettings[(int)TileType.GRASS].CountInStack = 64;

            tilesettings[(int)TileType.STOUN].drop_type = TileType.STOUN;
            tilesettings[(int)TileType.STOUN].CanBreak =true;
            tilesettings[(int)TileType.STOUN].TimeBreak = 10;
            tilesettings[(int)TileType.STOUN].CanWallk = false;
            tilesettings[(int)TileType.STOUN].CanBuild = false;
            tilesettings[(int)TileType.STOUN].CountInStack = 64;

            tilesettings[(int)TileType.PLATE].drop_type = TileType.PLATE;
            tilesettings[(int)TileType.PLATE].CanBreak =true;
            tilesettings[(int)TileType.PLATE].TimeBreak = 10;
            tilesettings[(int)TileType.PLATE].CanWallk = false;
            tilesettings[(int)TileType.PLATE].CanBuild = false;
            tilesettings[(int)TileType.PLATE].CountInStack =64 ;

            tilesettings[(int)TileType.WOOD].drop_type = TileType.WOOD;
            tilesettings[(int)TileType.WOOD].CanBreak =true;
            tilesettings[(int)TileType.WOOD].TimeBreak = 10;
            tilesettings[(int)TileType.WOOD].CanWallk = true;
            tilesettings[(int)TileType.WOOD].CanBuild = false;
            tilesettings[(int)TileType.WOOD].CountInStack = 64;

            tilesettings[(int)TileType.LEAF].drop_type = TileType.AIR;
            tilesettings[(int)TileType.LEAF].CanBreak =true;
            tilesettings[(int)TileType.LEAF].TimeBreak = 10;
            tilesettings[(int)TileType.LEAF].CanWallk = true;
            tilesettings[(int)TileType.LEAF].CanBuild = false;
            tilesettings[(int)TileType.LEAF].CountInStack = 64;

            tilesettings[(int)TileType.COAL].drop_type = TileType.COAL;
            tilesettings[(int)TileType.COAL].CanBreak =true;
            tilesettings[(int)TileType.COAL].TimeBreak = 10;
            tilesettings[(int)TileType.COAL].CanWallk = false;
            tilesettings[(int)TileType.COAL].CanBuild = false;
            tilesettings[(int)TileType.COAL].CountInStack = 64;

            tilesettings[(int)TileType.GOLD].drop_type = TileType.GOLD;
            tilesettings[(int)TileType.GOLD].CanBreak = true;
            tilesettings[(int)TileType.GOLD].TimeBreak = 10;
            tilesettings[(int)TileType.GOLD].CanWallk = false;
            tilesettings[(int)TileType.GOLD].CanBuild = false;
            tilesettings[(int)TileType.GOLD].CountInStack = 64;

            tilesettings[(int)TileType.IRON].drop_type = TileType.IRON;
            tilesettings[(int)TileType.IRON].CanBreak = true;
            tilesettings[(int)TileType.IRON].TimeBreak = 10;
            tilesettings[(int)TileType.IRON].CanWallk = false;
            tilesettings[(int)TileType.IRON].CanBuild = false;
            tilesettings[(int)TileType.IRON].CountInStack = 64;

            tilesettings[(int)TileType.DIAMONT].drop_type = TileType.DIAMONT;
            tilesettings[(int)TileType.DIAMONT].CanBreak = true;
            tilesettings[(int)TileType.DIAMONT].TimeBreak = 10;
            tilesettings[(int)TileType.DIAMONT].CanWallk = false;
            tilesettings[(int)TileType.DIAMONT].CanBuild = false;
            tilesettings[(int)TileType.DIAMONT].CountInStack = 64;

            /*
            tilesettings[(int)TileType.].drop_type = TileType.;
            tilesettings[(int)TileType.].CanBreak =;
            tilesettings[(int)TileType.].TimeBreak = ;
            tilesettings[(int)TileType.].CanWallk = ;
            tilesettings[(int)TileType.].CanBuild = ;
            tilesettings[(int)TileType.].CountInStack = ;*/


            ///////////////////////////////////////////////////////
            ///smart tiles///
            tilesettings[(int)TileType.CRAFTTABEL].drop_type = TileType.CRAFTTABEL;
            tilesettings[(int)TileType.CRAFTTABEL].CanBreak = true;
            tilesettings[(int)TileType.CRAFTTABEL].TimeBreak = 10;
            tilesettings[(int)TileType.CRAFTTABEL].CanWallk = false;
            tilesettings[(int)TileType.CRAFTTABEL].CanBuild = false;
            tilesettings[(int)TileType.CRAFTTABEL].CountInStack = 64;
            tilesettings[(int)TileType.CRAFTTABEL].CanOpen = true;

            tilesettings[(int)TileType.FURNACE].drop_type = TileType.FURNACE;
            tilesettings[(int)TileType.FURNACE].CanBreak = true;
            tilesettings[(int)TileType.FURNACE].TimeBreak = 10;
            tilesettings[(int)TileType.FURNACE].CanWallk = false;
            tilesettings[(int)TileType.FURNACE].CanBuild = false;
            tilesettings[(int)TileType.FURNACE].CountInStack = 64;
            tilesettings[(int)TileType.FURNACE].CanOpen = true;

            tilesettings[(int)TileType.CHEAST].drop_type = TileType.CHEAST;
            tilesettings[(int)TileType.CHEAST].CanBreak = true;
            tilesettings[(int)TileType.CHEAST].TimeBreak = 10;
            tilesettings[(int)TileType.CHEAST].CanWallk = false;
            tilesettings[(int)TileType.CHEAST].CanBuild = false;
            tilesettings[(int)TileType.CHEAST].CountInStack = 64;
            tilesettings[(int)TileType.CHEAST].CanOpen = true;


        }

    }                                                                                                                     
}
