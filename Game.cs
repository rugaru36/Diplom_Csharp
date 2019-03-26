using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplom_main {
    class Game {
        /*ПОЛЯ*/
        private int iterationsQuantity;
        private double stepSize = 0.01, accuracy = 0.1;
        public Pursuiter P_Player;
        public Escaper E_Player;

        //списки для построения графиков
        public CoordinatesList E_PlayerCoordinatesList;
        public CoordinatesList P_PlayerCoordinatesList;
        public double min_x, max_x, min_y, max_y;

        /*МЕТОДЫ*/
        public Game() {
            E_Player = new Escaper();
            P_Player = new Pursuiter();
        }

        //итерационный процесс
        public string iterationsProcess() {
            E_PlayerCoordinatesList = new CoordinatesList();
            P_PlayerCoordinatesList = new CoordinatesList();

            min_x = P_Player.getCoordinates()[0];
            max_x = min_x;
            min_y = P_Player.getCoordinates()[1];
            max_y = min_y;

            for (int i = 0; i < iterationsQuantity; i++) {

                //для построения графиков
                if (i % 5 == 0 || i == iterationsQuantity - 1 || isEscaperInRadCurcle() || isEscaperCaught()) {
                    P_PlayerCoordinatesList.addElement(new double[] {
                        P_Player.getCoordinates()[0], P_Player.getCoordinates()[1] });

                    E_PlayerCoordinatesList.addElement(new double[] {
                        E_Player.getCoordinates()[0], E_Player.getCoordinates()[1] });

                    if (min_x > Math.Min(P_PlayerCoordinatesList.min_x, E_PlayerCoordinatesList.min_x)) {
                        min_x = Math.Min(P_PlayerCoordinatesList.min_x, E_PlayerCoordinatesList.min_x);
                    }
                    if (min_y > Math.Min(P_PlayerCoordinatesList.min_y, E_PlayerCoordinatesList.min_y)) {
                        min_y = Math.Min(P_PlayerCoordinatesList.min_y, E_PlayerCoordinatesList.min_y);
                    }

                    if (max_x < Math.Max(P_PlayerCoordinatesList.max_x, E_PlayerCoordinatesList.max_x)) {
                        max_x = Math.Max(P_PlayerCoordinatesList.max_x, E_PlayerCoordinatesList.max_x);
                    }
                    if (max_y < Math.Max(P_PlayerCoordinatesList.max_y, E_PlayerCoordinatesList.max_y)) {
                        max_y = Math.Max(P_PlayerCoordinatesList.max_y, E_PlayerCoordinatesList.max_y);
                    }
                }

                P_Player.makeNextMove(stepSize, E_Player);
                E_Player.makeNextMove(stepSize, P_Player);

                if (isEscaperCaught()) {
                    return "Убегающий был пойман \nКоличество итераций: " + i.ToString();
                }

                else if (isEscaperInRadCurcle()) {
                    return "Убегающий забежал в радиус! \nКоличество итераций: " + i.ToString();
                }
            }
            return "Время погони вышло";
        }

        //сеттеры
        public void setStepSize(double newSize) {
            this.stepSize = newSize;
        }
        public void setAccuracy(double newAccuracy) {
            this.accuracy = newAccuracy;
        }
        public void setIterations(int newIter) {
            this.iterationsQuantity = newIter;
        }
        private bool isEscaperInRadCurcle() {

            if (!P_Player.getIsInerted()) return false;

            double[,] radCenterPoints = P_Player.getRadiusPoints();

            double[] point1 = new double[] { radCenterPoints[0, 0], radCenterPoints[0, 1] };
            double[] point2 = new double[] { radCenterPoints[1, 0], radCenterPoints[1, 1] };

            Vector buffVector1 = new Vector(point1, E_Player.getCoordinates());
            Vector buffVector2 = new Vector(point2, E_Player.getCoordinates());

            bool isInFirstRad = buffVector1.getLength() < P_Player.getRadius() * 0.8;
            bool isInSecondRad = buffVector2.getLength() < P_Player.getRadius() * 0.8;

            return isInFirstRad || isInSecondRad;
        }
        private bool isEscaperCaught() {
            Vector LVector = new Vector(this.E_Player.getCoordinates(), this.P_Player.getCoordinates());

            return LVector.getLength() < accuracy;
        }
    }
}
