using System.Collections.Generic;
using SelfishFramework.Src.Core;
using UnityEngine;

namespace Core.Features.GameScreenFeature.Mono
{
    public class GridMonoComponent : MonoBehaviour
    {
        [SerializeField] private Vector2 _offset;
        [SerializeField] private Vector2 _size;
        [SerializeField] private Vector2Int _gridSize;

        public readonly Dictionary<(int, int), Entity> Tiles = new();
        
        public int MaxTiles => _gridSize.x * _gridSize.y;
        
        public bool TryGetFreeCell(out int x, out int y, out Vector2 pos)
        {
            var startX = Random.Range(0, _gridSize.x);
            var startY = Random.Range(0, _gridSize.y);
            
            for (var i = 0; i < _gridSize.y; i++)
            {
                for (var j = 0; j < _gridSize.x; j++)
                {
                    x = (startX + j) % _gridSize.x;
                    y = (startY + i) % _gridSize.y;
                    if (Tiles.ContainsKey((x,y)))
                        continue;
                    
                    pos = GetPointPosition(x, y);
                    pos = transform.TransformPoint(pos);
                    return true;
                }
            }

            x = -1;
            y = -1;
            pos = Vector2.zero;
            return false;
        }

        private Vector3 GetPointPosition(int i, int j)
        {
            var cellW = _size.x / _gridSize.x;
            var cellH = _size.y / _gridSize.y;
            return Origin() + new Vector3((i + 0.5f) * cellW, (j + 0.5f) * cellH, 0);
        }

        private Vector3 Origin()
        {
            return new Vector3(-_size.x * 0.5f, -_size.y * 0.5f, 0) + (Vector3)_offset;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;

            var cellW = _size.x / _gridSize.x;
            var cellH = _size.y / _gridSize.y;

            for (var y = 0; y < _gridSize.y; y++)
            {
                for (var x = 0; x < _gridSize.x; x++)
                {
                    var center = GetPointPosition(x,y);
                    var cellMin = center - new Vector3(cellW * 0.5f, cellH * 0.5f, 0);
                    var cellMax = center + new Vector3(cellW * 0.5f, cellH * 0.5f, 0);

                    var p1 = transform.TransformPoint(cellMin);
                    var p2 = transform.TransformPoint(new Vector3(cellMax.x, cellMin.y, 0));
                    var p3 = transform.TransformPoint(cellMax);
                    var p4 = transform.TransformPoint(new Vector3(cellMin.x, cellMax.y, 0));

                    Gizmos.DrawLine(p1, p2);
                    Gizmos.DrawLine(p2, p3);
                    Gizmos.DrawLine(p3, p4);
                    Gizmos.DrawLine(p4, p1);
                }
            }
        }
    }
}