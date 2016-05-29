using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_Course_Work
{
    class Room
    {
        private Package[][] Packages;
        private int floorNumber;
        private int roomWidth;
        private int roomLength;
        private int roomHeight;
        public Room()
        {
            floorNumber = 0;
            roomWidth = 100;
            roomHeight = 100;
            roomLength = 100;
        }
        public Room(int f, int w, int l, int h)
        {
            floorNumber = f;
            roomWidth = w;
            roomLength = l;
            roomHeight = h;
            Packages = new Package[w][];
            for (int i=0; i<w; i++)
            {
                Packages[i] = new Package[l];
            }
        }
        public int FloorNumber { get { return floorNumber; } }
        public int RoomWidth { get { return roomWidth; } }
        public int RoomLength { get { return roomLength; } }
        public int RoomHeight { get { return roomHeight; } }

    }
}
