using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Features.LevelsFeature.Models;
using Core.Features.MinStepsCalculationFeature;
using Core.Features.TilesFeature.Models;

namespace Core
{
    public static class MinStepCalculatorUtils
    {
        public static State GetState(LevelConfigModel level)
        {
            return GetState(level.Tiles.Select(a => a.Tile));
        }
        
        public static State GetState(IEnumerable<ITileModel> tiles)
        {
            var setup = new List<List<ColorsModel>>();

            foreach (var tile in tiles)
            {
                var list = new List<ColorsModel>();
                GetColor(tile, list);
                setup.Add(list);
            }

            var state = new State();
            state.Data.AddRange(setup);
            return state;
        }

        private static void GetColor(ITileModel tile, List<ColorsModel> list)
        {
            switch (tile)
            {
                case SimpleTileModel simpleTile:
                    list.Add(simpleTile.Colors);
                    break;
                case ComplexTileModel complexTile:
                    list.Add(new ColorsModel{ColorId1 = complexTile.ColorId});
                    GetColor(complexTile.SubTile, list);
                    break;
            }
        }

        static int CompareSequences(int[] a, int[] b)
        {
            int min = Math.Min(a.Length, b.Length);
            for (int i = 0; i < min; i++)
            {
                int cmp = a[i].CompareTo(b[i]);
                if (cmp != 0) return cmp;
            }

            return a.Length.CompareTo(b.Length);
        }

        public static string Encode(int[] state)
        {
            var sb = new StringBuilder(state.Length * 4);
            for (int i = 0; i < state.Length; i++)
            {
                sb.Append(state[i]);
                if (i < state.Length - 1) sb.Append(',');
            }

            return sb.ToString();
        }
    }
}