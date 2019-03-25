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

            double[] LVector = new double[2] {
                coordinates[0] - pCoordinates[0],
                coordinates[1] - pCoordinates[1]
            };

            //Если далеко - убегающий бежит прямо
            if (Calculations.modOfVector(LVector) > 10) {
                return new double[2] {
                    coordinates[0] + speedVector[0],
                    coordinates[1] + speedVector[1]
                };
            }

            //средняя дистанция
            else if (Calculations.modOfVector(LVector) <= 10 && Calculations.modOfVector(LVector) > pRadius / 2) {
                return new double[2] {
                    coordinates[0] + LVector[0],
                    coordinates[1] + LVector[1]
                };
            }

            //близкая дистанция
            else if (Calculations.modOfVector(LVector) <= pRadius / 2) {
                //расстояние от координат преследователя до искомой точки
                double lengthToPoint = pRadius * 1.8;

                double[,] radPoints = pursuiter.getRadiusPoints();

                double[] toPoint1 = { radPoints[0, 0] - coordinates[0], radPoints[0, 1] - coordinates[1] };
                double[] toPoint2 = { radPoints[1, 0] - coordinates[0], radPoints[1, 1] - coordinates[0] };

                bool firstIsCloser = Calculations.modOfVector(toPoint1) <= Calculations.modOfVector(toPoint2);

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