using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;


namespace Diplom_main {
    abstract class Player {

        //поля
        protected const double PI = 3.14159265358979323846;
        protected double[] coordinates = new double[2];
        protected double[] speedVector = new double[2];
        protected double[] wantedPoint = new double[2];
        protected double radius = 0, maxAngle = 0;
        protected bool isInerted = false;

        //public методы игры
        public void moveToWantedPoint(double stepSize) {

            double[] JVector = {wantedPoint[0] - coordinates[0],
                                wantedPoint[1] - coordinates[1]};

            double wantedDirection = getAngle(JVector);

            //безынерционный объект
            if (!isInerted) setSpeedVectorDirection(wantedDirection);

            //инерция
            else if (isInerted) {
                double currentDirection = getAngle(speedVector);

                if (currentDirection > 180) currentDirection -= 360;

                double diffAngle = currentDirection - wantedDirection;

                while (Math.Abs(diffAngle) >= 360) {
                    if (diffAngle < 0) diffAngle += 360;
                    else if (diffAngle > 0) diffAngle -= 360;
                }

                if (Math.Abs(diffAngle) <= maxAngle) {
                    setSpeedVectorDirection(wantedDirection);
                }

                else if (Math.Abs(diffAngle) > maxAngle) {
                    setSpeedVectorDirection(currentDirection - (Math.Sign(diffAngle) * maxAngle));
                }
            }
            coordinates[0] += speedVector[0] * stepSize;
            coordinates[1] += speedVector[1] * stepSize;
        }

        public abstract void calculateNextWantedPoint(Player opponent);
        public double[,] getRadiusPoints(double lengthToPointCoeff = 1) {

            double[,] result = new double[2, 2];
            double[] radPoint1 = new double[2] {
                    createVector(getSpeedDirection() + 90, radius*lengthToPointCoeff)[0] + coordinates[0],
                    createVector(getSpeedDirection() + 90, radius*lengthToPointCoeff)[1] + coordinates[1] };

            double[] radPoint2 = new double[2] {
                    createVector(getSpeedDirection() + 90, radius*lengthToPointCoeff)[0] + coordinates[0],
                    createVector(getSpeedDirection() + 90, radius*lengthToPointCoeff)[1] + coordinates[1] };

            result[0, 0] = radPoint1[0];
            result[0, 1] = radPoint1[1];
            result[1, 0] = radPoint1[0];
            result[1, 1] = radPoint1[1];

            return result;
        }

        //геттеры
        public double getRadius() {
            return radius;
        }
        public double getSpeedLength() {
            return modOfVector(speedVector);
        }
        public double getSpeedDirection() {
            return getAngle(speedVector);
        }
        public double[] getCoordinates() {
            return coordinates;
        }
        public bool getIsInerted() {
            return isInerted;
        }

        //сеттеры
        public void setRadius(double newRadius) {
            radius = newRadius;

            if (radius == 0) {
                isInerted = false;
                maxAngle = 0;
                return;
            }

            isInerted = true;
            maxAngle = modOfVector(speedVector) / radius;
        }
        public void setSpeedVectorLength(double newSpeed) {
            speedVector = createVector(getSpeedDirection(), newSpeed);
            if (radius == 0) {
                maxAngle = 0;
                return;
            }
            maxAngle = newSpeed / radius;
        }
        public void setSpeedVectorDirection(double newDirection) {
            speedVector = createVector(newDirection, modOfVector(speedVector));
        }
        public void setXCoordinate(double newXCoordinate) {
            this.coordinates[0] = newXCoordinate;
        }
        public void setYCoordinate(double newYCoordinate) {
            this.coordinates[1] = newYCoordinate;
        }
        public void createSpeedVector(double direction, double speed) {
            speedVector = createVector(direction, speed);

            if (radius == 0) {
                maxAngle = 0;
                return;
            }
            maxAngle = speed / radius;
        }


        /*вспомогательные функции*/

        //поворот вектора скорости
        public static void turnVector(double degAngle, ref double[] inputVector) {
            double radAngle = degToRad(degAngle);

            double buff1 = inputVector[0] * Math.Cos(radAngle) + inputVector[1] * Math.Sin(radAngle);
            double buff2 = inputVector[1] * Math.Cos(radAngle) - inputVector[0] * Math.Sin(radAngle);

            inputVector[0] = buff1;
            inputVector[1] = buff2;
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