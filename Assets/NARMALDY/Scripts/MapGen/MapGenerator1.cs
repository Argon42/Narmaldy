using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Map
{
    public class Room
    {
        private uint _Width;
        private uint _Hight;
        public Vector2Int Position { get; set; }

        public Room(Vector2Int Position, uint Width, uint Hight)
        {

            this.Position = Position;
            this.Width = Width;
            this.Hight = Hight;

        }

        public uint Width
        {
            get
            {
                return _Width;
            }

            set
            {
                if (value == 0)
                    _Width = 1;
                else
                    _Width = value;
            }
        }

        public uint Hight
        {
            get
            {
                return _Hight;
            }

            set
            {
                if (value == 0)
                    _Hight = 1;
                else
                    _Hight = value;
            }
        }

        public Vector2 GetCenterPosition()
        {
            return new Vector2(Position.x + _Width / 2, Position.y + _Hight / 2);
        }
    }
    
    public class MapGenerator1
    {
        #region Настройки генератора
        private uint MaxWidthRoom = 10;
        private uint MaxHightRoom = 10;
        private uint MinWidthRoom = 3;
        private uint MinHightRoom = 3;
        private uint MaxRoomX = 10;
        private uint MaxRoomY = 10;
        #endregion

        private List<List<Room>> RoomMap;

        /// <summary>
        /// Установка настроек генератора карты
        /// </summary>
        /// <param name="LevelLength">Количество комнат в ширину</param>
        /// <param name="LevelHeight">Максимальное количество возможных комнат в высоту</param>
        /// <param name="MaxWidthRoomInLevel">Максимальная ширина комнат</param>
        /// <param name="MaxHightRoomInLevel">Максимальная высота комнат</param>
        /// <param name="Seed">Зерно для генерации. 0 - случайное</param>
        public void SetGeneratorSettings(uint LevelLength, uint LevelHeight, uint MaxWidthRoomInLevel, uint MaxHightRoomInLevel, int Seed = 0)
        {
            MaxRoomX = LevelLength;
            MaxRoomY = LevelHeight;
            MaxWidthRoom = MaxWidthRoomInLevel;
            MaxHightRoom = MaxHightRoomInLevel;
            if(Seed != 0) Random.InitState(Seed);
        }

        /// <summary>
        /// Создает оси на оси Х и на каждой из них создает N комнат
        /// </summary>
        /// <returns> Список в котором находятся списки с комнатами на оси</returns>
        public List<List<Room>> CreateFloor()
        {
            List<List<Room>> OnAxisX = new List<List<Room>>();
            OnAxisX.Add(new List<Room>());
            OnAxisX[0].Add(new Room(
                new Vector2Int(0, (int)Random.Range(0,(MaxHightRoom * MaxRoomY)-MinHightRoom)), 
                (uint)Random.Range(MinWidthRoom, MaxWidthRoom), 
                (uint)Random.Range(MinHightRoom, MaxHightRoom)));

            for (int i = 1; i < MaxRoomX; i++)
            {
                OnAxisX.Add(new List<Room>());
                int CountRoonInAxis = (int)Random.Range(1, MaxRoomY);
                int CellLeft = (int)(MaxHightRoom * MaxRoomY);

                int
                    WidthRoom,
                    PosOnX,
                    HightRoom,
                    LastHightRoom,
                    PosOnY,
                    OffsetY;
                HightRoom = 0;
                PosOnY = 0;

                do
                {
                    WidthRoom = Random.Range((int)MinWidthRoom, (int)MaxWidthRoom);
                    PosOnX = (int)MaxWidthRoom * i + i + Random.Range(0, (int)MaxWidthRoom - WidthRoom);
                    LastHightRoom = HightRoom;
                    HightRoom = Random.Range((int)MinHightRoom, (int)MaxHightRoom);
                    OffsetY = Random.Range(0, (int)MaxHightRoom - HightRoom) + Random.Range(0, (int)(MaxHightRoom * MaxRoomY));
                    PosOnY = PosOnY  + LastHightRoom + OffsetY + Random.Range((int)MinHightRoom, (int)(MaxHightRoom/MaxRoomY));
                    OnAxisX[i].Add(new Room(new Vector2Int(PosOnX, PosOnY), (uint)WidthRoom, (uint)HightRoom));

                } while (PosOnY < CellLeft);
            }
            return OnAxisX;
        }

        void CreateCorridors()
        {

        }

        public void Gen(Tilemap tileMap, Tile tile)
        {
            tileMap.ClearAllTiles();
            RoomMap = CreateFloor();
            List<Room> AllRooms = new List<Room>();
            for (int iteratorOnAxisX = 0; iteratorOnAxisX < RoomMap.Count; iteratorOnAxisX++)
            {
                for (int iteratorOnAxisY = 0; iteratorOnAxisY < RoomMap[iteratorOnAxisX].Count; iteratorOnAxisY++)
                {
                    Room room = RoomMap[iteratorOnAxisX][iteratorOnAxisY];
                    AllRooms.Add(room);
                    for (int x = room.Position.x; x <= room.Position.x + room.Width; x++)
                    {
                        for (int y = room.Position.y; y <= room.Position.y + room.Hight; y++)
                        {
                            tileMap.SetTile(new Vector3Int(x, y, 0), tile);
                        }
                    }
                }
            }

            
        }
    }


}