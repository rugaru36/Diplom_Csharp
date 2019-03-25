using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplom_main {
    static class Functions {

        public const double PI = 3.14159265358979323846;

        //поворот вектора скорости
        public static double[] turnVector(double degAngle, ref double[] inputVector) {
            double radAngle = degToRad(degAngle);

            double buff1 = inputVector[0] * Math.Cos(radAngle) + inputVector[1] * Math.Sin(radAngle);
            double buff2 = inputVector[1] * Math.Cos(radAngle) - inputVector[0] * Math.Sin(radAngle);

            return new double[] { buff1, buff2 };
        }
        //создание вектора
        public static double[] createVector(double degAngle, double length) {
            double[] result = new double[2];

            if (length == 0) return result;

            //получим положительный угол, не больше 360
            while (Math.Abs(degAngle) >= 360 || degAngle < 0) {
                if (degAngle < 0) degAngle += 360;
                else degAngle -= 360;
            }
            //первая четверть
            if (degAngle >= 0 && degAngle <= 90)
                return new double[] { Math.Sin(degToRad(90 - degAngle)) * length,
                                Math.Sin(degToRad(degAngle)) * length };

            //вторая четверть
            else if (degAngle > 90 && degAngle < 180)
                return new double[] {
                    Math.Sin(degToRad(-90 - degAngle)) * length * (-1),
                    Math.Sin(degToRad((180 - degAngle))) * length};
            //третья четверть
            else if (degAngle >= 180 && degAngle <= 270)
                return new double[] {
                    Math.Sin(degToRad(-90 - degAngle)) * length * (-1),
                    Math.Sin(degToRad((degAngle - 180))) * length * (-1)
                };
            //четвертая четверть
            else if (degAngle > 270 && degAngle < 360)
                return new double[] {
                    Math.Sin(degToRad(90 - degAngle)) * length,
                    Math.Sin(degToRad((360 - degAngle))) * length * (-1)
                };

            else return new double[] { 0, 0 };
        }
        //статические вспомогательные функции
        public static double modOfVector(double[] vector) {
            return Math.Sqrt(Math.Pow(vector[0], 2) + Math.Pow(vector[1], 2));
        }
        public static double radToDeg(double rad) {
            return (rad * 180) / PI;
        }
        public static double degToRad(double deg) {
            return (PI * deg) / 180;
        }
        //возвращает положительный угол в градусах
        public static double getAngle(double[] vector) {

            if (vector[0] > 0 && vector[1] > 0) { //первая четверть
                return radToDeg(Math.Asin(Math.Abs(vector[1]) / modOfVector(vector)));
            }
            else if (vector[0] < 0 && vector[1] > 0) { //вторая четверть
                return radToDeg(PI - Math.Asin(Math.Abs(vector[1]) / modOfVector(vector)));
            }
            else if (vector[0] < 0 && vector[1] < 0) { //третья четверть
                return radToDeg(PI + Math.Asin(Math.Abs(vector[1]) / modOfVector(vector)));
            }
            else if (vector[0] > 0 && vector[1] < 0) { //четвёртая четверть
                return radToDeg(2 * PI - Math.Asin(Math.Abs(vector[1]) / modOfVector(vector)));
            }
            else if (vector[0] == 0 && vector[1] > 0) { // вверх
                return radToDeg(PI / 2);
            }
            else if (vector[0] == 0 && vector[1] < 0) { // вниз
                return radToDeg(PI * 3 / 2);
            }
            else if (vector[0] > 0 && vector[1] == 0) { // вправо
                return 0;
            }
            else if (vector[0] < 0 && vector[1] == 0) { // влево
                return radToDeg(PI);
            }
            else return 0;
        }
    }
}
