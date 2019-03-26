using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplom_main {
    class Vector {

        public const double PI = 3.14159265358979323846;

        private double length, direction;
        private double[] coordinates;

        public Vector(double inputLength, double inputDirection) {
            length = inputLength;
            direction = inputDirection;
            calculateCoordinates();
        }
        public Vector(double[] beginPoint, double[] endPoint) {
            coordinates = new double[] { endPoint[0] - beginPoint[0], endPoint[1] - beginPoint[1] };
            calculateModOfVector();
            calculateVectorDirection();
        }
        public Vector() {
            coordinates = new double[] { 0, 0 };
            setDirection(0);
            setLength(0);
        }

        public double getLength() {
            return length;
        }
        public double getDirection() {
            return direction;
        }
        public double[] getCoordinates() {
            return coordinates;
        }

        public void setDirection(double value) {
            direction = value;
            calculateCoordinates();
        }
        public void setLength(double value) {
            length = value;
            calculateCoordinates();
        }

        private void calculateCoordinates() {

            while (Math.Abs(direction) >= 360 || direction < 0) {
                if (direction < 0) direction += 360;
                else direction -= 360;
            }

            //первая четверть
            if (direction >= 0 && direction <= 90)
                coordinates = new double[] {
                                Math.Sin(degToRad(90 - direction)) * length,
                                Math.Sin(degToRad(direction)) * length };
            //вторая четверть
            else if (direction > 90 && direction < 180)
                coordinates = new double[] {
                                Math.Sin(degToRad(-90 - direction)) * length * (-1),
                                Math.Sin(degToRad((180 - direction))) * length};
            //третья четверть
            else if (direction >= 180 && direction <= 270)
                coordinates = new double[] {
                                Math.Sin(degToRad(-90 - direction)) * length * (-1),
                                Math.Sin(degToRad((direction - 180))) * length * (-1)};
            //четвертая четверть
            else if (direction > 270 && direction < 360)
                coordinates = new double[] {
                                Math.Sin(degToRad(90 - direction)) * length,
                                Math.Sin(degToRad((360 - direction))) * length * (-1)
                };

            else coordinates = new double[] { 0, 0 };
        }
        private void calculateModOfVector() {
            length = Math.Sqrt(Math.Pow(coordinates[0], 2) + Math.Pow(coordinates[1], 2));
        }
        private void calculateVectorDirection() {
            if (coordinates[0] > 0 && coordinates[1] > 0) { //первая четверть
                direction = radToDeg(Math.Asin(Math.Abs(coordinates[1]) / length));
            }
            else if (coordinates[0] < 0 && coordinates[1] > 0) { //вторая четверть
                direction = radToDeg(PI - Math.Asin(Math.Abs(coordinates[1]) / length));
            }
            else if (coordinates[0] < 0 && coordinates[1] < 0) { //третья четверть
                direction = radToDeg(PI + Math.Asin(Math.Abs(coordinates[1]) / length));
            }
            else if (coordinates[0] > 0 && coordinates[1] < 0) { //четвёртая четверть
                direction = radToDeg(2 * PI - Math.Asin(Math.Abs(coordinates[1]) / length));
            }
            else if (coordinates[0] == 0 && coordinates[1] > 0) { // вверх
                direction = radToDeg(PI / 2);
            }
            else if (coordinates[0] == 0 && coordinates[1] < 0) { // вниз
                direction = radToDeg(PI * 3 / 2);
            }
            else if (coordinates[0] > 0 && coordinates[1] == 0) { // вправо
                direction = 0;
            }
            else if (coordinates[0] < 0 && coordinates[1] == 0) { // влево
                direction = radToDeg(PI);
            }
            else direction = 0;
        }
        private static double radToDeg(double rad) {
            return (rad * 180) / PI;
        }
        private static double degToRad(double deg) {
            return (PI * deg) / 180;
        }
    }
}
