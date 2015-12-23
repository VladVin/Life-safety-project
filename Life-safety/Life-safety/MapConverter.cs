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
        private double plainWidth;
        private double plainHeight;
        private double mapWidth;
        private double mapHeight;

        // Plain width in meters
        // mapWidth and mapHeight in pixels
        public MapConverter(double plainWidth, double mapWidth, double mapHeight)
        {
            this.plainWidth = plainWidth;
            this.plainHeight = plainWidth / mapWidth * mapHeight;
            this.mapWidth = mapWidth;
            this.mapHeight = mapHeight;
        }

        public Point TranslatePointToMeters(Point point)
        {
            double xMeters = point.X / mapWidth * plainWidth;
            double yMeters = point.Y / mapHeight * plainHeight;
            return new Point(xMeters, yMeters);
        }

        public Point TranslatePointToPixels(Point point)
        {
            double xPixels = mapWidth / plainWidth * point.X;
            double yPixels = mapHeight / plainHeight * point.Y;
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
