using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Life_safety
{
    class MapConverter
    {
        private int plainWidth;
        private int plainHeight;
        private int mapWidth;
        private int mapHeight;

        // Plain width in meters
        // mapWidth and mapHeight in pixels
        public MapConverter(int plainWidth, int mapWidth, int mapHeight)
        {
            this.plainWidth = plainWidth;
            this.plainHeight = (int)((float)plainWidth / mapWidth * mapHeight);
            this.mapWidth = mapWidth;
            this.mapHeight = mapHeight;
        }

        public Point TranslatePointToMeters(Point point)
        {
            int xMeters = (int)((float)point.X / mapWidth * plainWidth);
            int yMeters = (int)((float)point.Y / mapHeight * plainHeight);
            return new Point(xMeters, yMeters);
        }

        public Point TranslatePointToPixels(Point point)
        {
            float xPixels = mapWidth / plainWidth * (float)point.X;
            float yPixels = mapHeight / plainHeight * (float)point.Y;
            return new Point(xPixels, yPixels);
        }
    }
}
