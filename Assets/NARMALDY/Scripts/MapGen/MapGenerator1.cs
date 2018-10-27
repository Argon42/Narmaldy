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
        private uint MaxWidthRoom = 10; // Максимальная ширина комнаты 
        private uint MaxHightRoom = 10;
        private uint MinWidthRoom = 3; // минимальная высота комнаты
        private uint MinHightRoom = 3;
        private uint MaxRoomX = 10; // максимум комнат на оси X
        private uint MaxRoomY = 10; // максимум комнат на оси Y
        private int CountOfCorridors = 1; // воличество выходящих коридоров
        private int DegreeOfSmoothing = 1; // количество поворотов между комнатами
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
            if (Seed != 0) Random.InitState(Seed);
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
                new Vector2Int(0, (int)Random.Range(0, (MaxHightRoom * MaxRoomY) - MinHightRoom) + Random.Range(0, (int)(MaxHightRoom * MaxRoomY))),
                (uint)Random.Range(MinWidthRoom, MaxWidthRoom),
                (uint)Random.Range(MinHightRoom, MaxHightRoom))); // создание начальной комнаты

            for (int i = 1; i < MaxRoomX; i++) // цикл по всем осям
            {
                OnAxisX.Add(new List<Room>());
                //int CountRoonInAxis = (int)Random.Range(1, MaxRoomY);
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
                    PosOnY = PosOnY + LastHightRoom + OffsetY + Random.Range((int)MinHightRoom, (int)(MaxHightRoom / MaxRoomY));
                    OnAxisX[i].Add(new Room(new Vector2Int(PosOnX, PosOnY), (uint)WidthRoom, (uint)HightRoom));
                    //CountRoonInAxis--; ountRoonInAxis>0
                } while (PosOnY < CellLeft);
            }
            return OnAxisX;
        }

        void Swap(ref int a, ref int b)
        {
            int k = b;
            b = a;
            a = k;
        }

        void DrawCorridors(Room StartRoom, Room EndRoom)
        {
            Vector2Int StartPos = new Vector2Int(
                Random.Range(StartRoom.Position.x, StartRoom.Position.x + (int)StartRoom.Width),
                Random.Range(StartRoom.Position.y, StartRoom.Position.y + (int)StartRoom.Hight));

            Vector2Int EndPos = new Vector2Int(
                Random.Range(EndRoom.Position.x, EndRoom.Position.x + (int)EndRoom.Width),
                Random.Range(EndRoom.Position.y, EndRoom.Position.y + (int)EndRoom.Hight));

            //int DistansForX = (EndPos.x - StartPos.x) ;
            //int DistansForY = (EndPos.y - StartPos.y) ;


            //int x = StartPos.x,
            //    y = StartPos.y;


            int
                x1 = StartPos.x,
                y1 = StartPos.y,
                x2 = EndPos.x,
                y2 = EndPos.y,
                x3;

            if (x1 < x2)
            {
                x3 = Random.Range(x1, x2);
                for (int x = x1; x<= x3; x++)
                    _Floor.SetTile(new Vector3Int(x, y1,0),_FloorTile);
                for (int x = x3; x < x2; x++)
                    _Floor.SetTile(new Vector3Int(x, y2, 0), _FloorTile);
            }
            else
            {
                x3 = Random.Range(x2, x1);
                for (int x = x2; x<= x3; x++)
                    _Floor.SetTile(new Vector3Int(x, y2, 0), _FloorTile);
                for (int x = x3; x < x1; x++)
                    _Floor.SetTile(new Vector3Int(x, y1, 0), _FloorTile);
            }
            if (y1 > y2)
                Swap(ref y1,ref y2);
            for (int y = y1; y < y2; y++)
                _Floor.SetTile(new Vector3Int(x3, y,0), _FloorTile);

            
        }

        void CreateCorridors(List<Room> AllRooms)
        {
            List<List<float>> AllDistans = new List<List<float>>();
            for (int i = 0; i < AllRooms.Count; i++)
            {
                AllDistans.Add(new List<float>());
                for (int j = 0; j < AllRooms.Count; j++)
                {
                    AllDistans[i].Add(Vector2.Distance(AllRooms[i].GetCenterPosition(), AllRooms[j].GetCenterPosition()));
                }
            }

            for (int i = 0; i < AllDistans.Count - 1; i++)
            {
                int FirstRoom = i, SecondRoom = i + 1;
                float min = AllDistans[FirstRoom][SecondRoom];
                for (int j = i + 1; j < AllDistans.Count; j++)
                    if (AllDistans[i][j] < min)
                    {
                        min = AllDistans[i][j];
                        FirstRoom = i;
                        SecondRoom = j;
                    }
                DrawCorridors(AllRooms[FirstRoom], AllRooms[SecondRoom]);
            }

        }
        Tilemap _Floor;
        Tile _FloorTile;
        public List<Room> AllRooms;
        public void Gen(Tilemap tileMap, Tile tile)
        {
            _Floor = tileMap;
            _FloorTile = tile;

            tileMap.ClearAllTiles();
            RoomMap = CreateFloor();
            AllRooms = new List<Room>();


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
            CreateCorridors(AllRooms);

            
        }
    }


}