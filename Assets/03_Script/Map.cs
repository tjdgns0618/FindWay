using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField] GameObject _tile;
    [SerializeField] Color _color0;
    [SerializeField] Color _color1;

    char[,] _map;
    bool[,] _pMap;
    Tile[,] _tMap;

    // Start is called before the first frame update
    void Start()
    {
        string path = Application.dataPath+"/04_Map/Map01.txt";
        string data = System.IO.File.ReadAllText(path);
        
        BuildMap(data);
        SetMapPosition();
    }

    void SetMapPosition()
    {
        var cam = Camera.main;
        float halfH = cam.orthographicSize;                 // orthographic = 2d카메라기법, 사이즈는 높이의 절반사이즈
        float screenHeight = halfH * 2F;                      // 사이즈의 두배
        float screenWidth = screenHeight * cam.aspect;  // aspect = 종횡비 16:9
        float tileSize = _tile.GetComponent<SpriteRenderer>().bounds.size.x;

        int row = _map.GetLength(0);
        int col = _map.GetLength(1);
        float mapWidth = col * tileSize;
        float mapHeight = row * tileSize;

        Vector3 pos = Vector3.zero;
        for(int r = 0; r < row; ++r)
        {
            pos.x = 0f;
            for(int c = 0; c < col; ++c)
            {
                _tMap[r, c].transform.position = pos;
                pos.x += tileSize;
            }

            pos.y -= tileSize;
        }

        float scaleX = screenWidth / mapWidth;
        float scaleY = screenHeight / mapHeight;
        bool fitToWidth = scaleX < scaleY;
        float scale = fitToWidth ? scaleX : scaleY;
        this.transform.localScale = Vector3.one * scale;

        float x = (mapWidth - tileSize) * -0.5f * scale;
        float y = (mapHeight- tileSize) * -0.5f * scale;

        transform.position = new Vector3(x, -y, 0f);

    }

    void BuildMap(string map)
    {
        string[] lines = map.Split('\n');
        string[] words = lines[0].Trim().Split();

        int R = lines.Length;
        int C= words.Length;

        _map = new char[R, C];
        _pMap = new bool[R, C];
        _tMap = new Tile[R, C];

        Color color = _color0;
        bool _0 = false;
        Transform parent = this.transform;
        
        for (int r = 0; r < R; ++r)
        {
            string line = lines[r].Trim();
            words = line.Split();

            _0 = (r & 0x1) == 0;

            for(int c = 0; c < C; ++c)
            {
                color = _0 ? _color0 : _color1;
                _0 = !_0;

                char ch = words[c][0];
                bool isBlock = ch == '1';

                _map[r, c] = ch;
                _pMap[r, c] = isBlock;

                Tile tile = Instantiate(_tile, parent).GetComponent<Tile>();
                tile.gameObject.name = string.Format($"{r} {c}");

                _tMap[r, c] = tile;
                tile.Init(color, isBlock);
            }
        }
    }
}
