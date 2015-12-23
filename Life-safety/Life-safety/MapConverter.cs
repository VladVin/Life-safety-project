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
            this.plainHeight = (int)((double)plainWidth / mapWidth * mapHeight);
            this.mapWidth = mapWidth;
            this.mapHeight = mapHeight;
        }

        public Point TranslatePointToMeters(Point point)
        {
            int xMeters = (int)((double)point.X / mapWidth * plainWidth);
            int yMeters = (int)((double)point.Y / mapHeight * plainHeight);
            return new Point(xMeters, yMeters);
        }

        public Point TranslatePointToPixels(Point point)
        {
            double xPixels = mapWidth / plainWidth * (double)point.X;
            double yPixels = mapHeight / plainHeight * (double)point.Y;
            return new Point(xPixels, yPixels);
        }

        public double ConvertWidthToPixels(double width)
        {
            double widthPixels = mapWidth / plainWidth * (double)width;
            return widthPixels;
        }

        public double ConvertHeightToPixels(double height)
        {
            double heightPixels = mapHeight / plainHeight * (double)height;
            return heightPixels;
        }
    }
}
