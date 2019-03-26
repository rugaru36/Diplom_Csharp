using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplom_main {
    class Escaper : Player {

        private byte numOfRadCircle = 0;

        public override double[] getNextWantedPoint(Player pursuiter) {

            double pRadius = pursuiter.getRadius();
            double pDir = pursuiter.getSpeedDirection();
            double pSpeed = pursuiter.getSpeedVectorLength();
            double[] pCoordinates = pursuiter.getCoordinates();

            Vector LVector = new Vector(pCoordinates, coordinates);

            //Если далеко - убегающий бежит прямо
            if (LVector.getLength() > 10) {
                return new double[2] {
                    coordinates[0] + speedVector.getCoordinates()[0],
                    coordinates[1] + speedVector.getCoordinates()[1]
                };
            }

            //средняя дистанция
            else if (LVector.getLength() <= 10 && LVector.getLength() > pRadius / 2) {
                return new double[2] {
                    coordinates[0] + LVector.getCoordinates()[0],
                    coordinates[1] + LVector.getCoordinates()[1]
                };
            }

            //близкая дистанция
            else if (LVector.getLength() <= pRadius / 2) {
                //расстояние от координат преследователя до искомой точки
                double lengthToPoint = pRadius * 1.8;

                double[,] radPoints = pursuiter.getRadiusPoints();

                double[] point1 = { radPoints[0, 0], radPoints[0, 1] };
                double[] point2 = { radPoints[1, 0], radPoints[1, 1] };


                Vector toPoint1 = new Vector(coordinates, point1);
                Vector toPoint2 = new Vector(coordinates, point2);


                bool firstIsCloser = LVector.getLength() <= toPoint2.getLength();

                if (firstIsCloser || numOfRadCircle == 1) {
                    //двигаемся к radPoint1
                    numOfRadCircle = 1;
                    return new double[2] { radPoints[0, 0], radPoints[0, 1] };
                }
                //двигаемся к radPoint2
                else {
                    numOfRadCircle = 2;
                    return new double[2] { radPoints[1, 0], radPoints[1, 1] };
                }
            }

            return new double[] { 0, 0 };
        }
    }
}