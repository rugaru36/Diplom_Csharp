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

            for (int i = 0; i < iterationsQuantity; i++) {

                //для построения графиков
                if (i % 5 == 0 || i == iterationsQuantity - 1 || isEscaperInRadCurcle() || isEscaperCaught()) {
                    P_PlayerCoordinatesList.addElement(new double[] {
                        P_Player.getCoordinates()[0], P_Player.getCoordinates()[1] });

                    E_PlayerCoordinatesList.addElement(new double[] {
                        E_Player.getCoordinates()[0], E_Player.getCoordinates()[1] });
                    
                    if(min_x > Math.Min(P_PlayerCoordinatesList.min_x, E_PlayerCoordinatesList.min_x)) {
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

                E_Player.setPursuitersData(P_Player);

                P_Player.calculateNextWantedPoint(E_Player.getCoordinates());
                E_Player.calculateNextWantedPoint(P_Player.getCoordinates());

                P_Player.moveToWantedPoint(stepSize);
                E_Player.moveToWantedPoint(stepSize);


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

            if (!this.P_Player.getIsInerted()) return false;

            //вектора, ведущие от преследователя до центров окружностей радиусов
            double[] toPoint1 = Player.createVector(this.P_Player.getSpeedDirection() + 90, this.P_Player.getRadius());
            double[] toPoint2 = Player.createVector(this.P_Player.getSpeedDirection() - 90, this.P_Player.getRadius());

            double a = this.P_Player.getSpeedDirection();
            a = this.P_Player.getRadius();
            double[] b = E_Player.getCoordinates();

            double[] radCenterPoint1 = {
                this.P_Player.getCoordinates()[0] + toPoint1[0],
                this.P_Player.getCoordinates()[1] + toPoint1[1]
            };
            double[] radCenterPoint2 = {
                this.P_Player.getCoordinates()[0] + toPoint2[0],
                this.P_Player.getCoordinates()[1] + toPoint2[1]
            };

            double[] buffVector1 = { radCenterPoint1[0] - this.E_Player.getCoordinates()[0],
                                    radCenterPoint1[1] - this.E_Player.getCoordinates()[1]};

            double[] buffVector2 = { radCenterPoint1[0] - this.E_Player.getCoordinates()[0],
                                    radCenterPoint1[1] - this.E_Player.getCoordinates()[1]};

            bool isInFirstRad = Player.modOfVector(buffVector1) < this.P_Player.getRadius() * 0.8;
            bool isInSecondRad = Player.modOfVector(buffVector2) < this.P_Player.getRadius() * 0.8;

            return isInFirstRad || isInSecondRad;
        }
        private bool isEscaperCaught() {
            double LVector = Math.Sqrt(Math.Pow(this.E_Player.getCoordinates()[0] -
            this.P_Player.getCoordinates()[0], 2) + Math.Pow(this.E_Player.getCoordinates()[1] -
            this.P_Player.getCoordinates()[1], 2));

            return LVector < accuracy;
        }
    }
}
