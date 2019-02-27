using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplom_main {
    class Escaper : Player {
        private double pRadius;
        private double pSpeed;
        private double pDir;
        private byte numOfRadCircle = 0;

        //на будущее - получить радиус преследователя для более сложного алгоритма
        public void setPursuitersData(Player Pursuiter) {
            pRadius = Pursuiter.getRadius();
            pDir = Pursuiter.getSpeedDirection();
            pSpeed = Pursuiter.getSpeedLength();
        }

        public override void calculateNextWantedPoint(double[] pCoordinates) {

            double[] LVector = new double[2] {
                coordinates[0] - pCoordinates[0],
                coordinates[1] - pCoordinates[1]
            };

            //Если далеко - убегающий бежит прямо
            if (modOfVector(LVector) > 10) {
                wantedPoint = new double[2] {
                    coordinates[0] + speedVector[0],
                    coordinates[1] + speedVector[1]
                };

                return;
            }

            //средняя дистанция
            else if (modOfVector(LVector) <= 10 && modOfVector(LVector) > pRadius / 2) {
                wantedPoint = new double[2] {
                    coordinates[0] + LVector[0],
                    coordinates[1] + LVector[1]
                };
                return;
            }

            //близкая дистанция
            else if (modOfVector(LVector) <= this.pRadius / 2) {
                //расстояние от координат преследователя до искомой точки
                double lengthToPoint = pRadius * 1.8;

                double[] radPoint1 = new double[2] {
                                    createVector(pDir + 90, lengthToPoint)[0] + pCoordinates[0],
                                    createVector(pDir + 90, lengthToPoint)[1] + pCoordinates[1] };

                double[] radPoint2 = new double[2] {
                                    createVector(pDir - 90, lengthToPoint)[0] + pCoordinates[0],
                                    createVector(pDir - 90, lengthToPoint)[1] + pCoordinates[1] };

                double[] vectorToPoint1 = { radPoint1[0] - coordinates[0], radPoint1[1] - coordinates[1] };
                double[] vectorToPoint2 = { radPoint2[0] - coordinates[0], radPoint2[0] - coordinates[0] };


                if (modOfVector(vectorToPoint1) <= modOfVector(vectorToPoint2) || numOfRadCircle == 1) {
                    //двигаемся к radPoint1
                    numOfRadCircle = 1;
                    wantedPoint = radPoint1;
                }
                //двигаемся к radPoint2
                else {
                    numOfRadCircle = 2;
                    wantedPoint = radPoint2;
                }
            }
        }
    }
}