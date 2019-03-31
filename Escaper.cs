using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplom_main {
    class Escaper : Player {

        private byte numOfRadCircle = 0;

        protected override void getNextWantedPoint(Player pursuiter) {

            double pRadius = pursuiter.getRadius();
            double pDir = pursuiter.getSpeedDirection();
            double pSpeed = pursuiter.getSpeedVectorLength();
            double[] pCoordinates = pursuiter.getCoordinates();

            Vector LVector = new Vector(pCoordinates, coordinates);

            //Если далеко - убегающий бежит прямо
            if (LVector.getLength() > pRadius * 2) {
                wantedPoint = new double[2] {
                    coordinates[0] + speedVector.getCoordinates()[0],
                    coordinates[1] + speedVector.getCoordinates()[1]
                };
            }

            //средняя дистанция
            else if (LVector.getLength() <= pRadius * 2 && LVector.getLength() > pRadius / 2) {
                wantedPoint = new double[2] {
                    coordinates[0] + LVector.getCoordinates()[0],
                    coordinates[1] + LVector.getCoordinates()[1]
                };
            }

            //близкая дистанция
            else if (LVector.getLength() <= pRadius / 2) {

                double[][] radPoints = pursuiter.getRadiusPoints();

                Vector toPoint1 = new Vector(coordinates, radPoints[0]);
                Vector toPoint2 = new Vector(coordinates, radPoints[1]);

                bool firstIsCloser = LVector.getLength() <= toPoint2.getLength();

                if (firstIsCloser || numOfRadCircle == 1) {
                    numOfRadCircle = 1;
                    wantedPoint = radPoints[0];
                }
                else {
                    numOfRadCircle = 2;
                    wantedPoint = radPoints[1];
                }
            }
        }
    }
}