using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;


namespace Diplom_main {
    abstract class Player {

        //поля
        
        protected double[] coordinates = new double[2];
        protected double[] speedVector = new double[2];
        protected double[] wantedPoint = new double[2];
        protected double radius = 0, maxAngle = 0;
        protected bool isInerted = false;

        //public методы игры
        public void makeNextMove(double stepSize, Player opponent) {
            moveToWantedPoint(stepSize, getNextWantedPoint(opponent));
        }
        public abstract double[] getNextWantedPoint(Player opponent);
        public double[,] getRadiusPoints(double lengthToPointCoeff = 1) {

            double[,] result = new double[2, 2];
            double[] radPoint1 = new double[2] {
                    Functions.createVector(getSpeedDirection() + 90, radius*lengthToPointCoeff)[0] + coordinates[0],
                    Functions.createVector(getSpeedDirection() + 90, radius*lengthToPointCoeff)[1] + coordinates[1] };

            double[] radPoint2 = new double[2] {
                    Functions.createVector(getSpeedDirection() + 90, radius*lengthToPointCoeff)[0] + coordinates[0],
                    Functions.createVector(getSpeedDirection() + 90, radius*lengthToPointCoeff)[1] + coordinates[1] };

            result[0, 0] = radPoint1[0];
            result[0, 1] = radPoint1[1];
            result[1, 0] = radPoint1[0];
            result[1, 1] = radPoint1[1];

            return result;
        }
        private void moveToWantedPoint(double stepSize, double[] wantedPoint) {

            double[] JVector = {wantedPoint[0] - coordinates[0],
                                wantedPoint[1] - coordinates[1]};

            double wantedDirection = Functions.getAngle(JVector);

            //безынерционный объект
            if (!getIsInerted()) setSpeedVectorDirection(wantedDirection);

            //инерция
            else if (getIsInerted()) {
                double currentDirection = Functions.getAngle(speedVector);

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

        //геттеры
        public double getRadius() {
            return radius;
        }
        public double getSpeedVectorLength() {
            return Functions.modOfVector(speedVector);
        }
        public double getSpeedDirection() {
            return Functions.getAngle(speedVector);
        }
        public double[] getCoordinates() {
            return coordinates;
        }
        public double[] getSpeedVector() {
            return speedVector;
        }
        public bool getIsInerted() {
            return radius > 0.1;
        }

        //сеттеры
        public void setRadius(double newRadius) {
            radius = newRadius;

            if (radius > 0) {
                maxAngle = Functions.modOfVector(speedVector) / radius;
            }

        }
        public void setSpeedVectorLength(double newSpeed) {
            speedVector = Functions.createVector(getSpeedDirection(), newSpeed);
            if (radius > 0) {
                maxAngle = newSpeed / radius;
                return;
            }
        }
        public void setSpeedVectorDirection(double newDirection) {
            speedVector = Functions.createVector(newDirection, Functions.modOfVector(speedVector));
        }
        public void setXCoordinate(double newXCoordinate) {
            this.coordinates[0] = newXCoordinate;
        }
        public void setYCoordinate(double newYCoordinate) {
            this.coordinates[1] = newYCoordinate;
        }
        public void createSpeedVector(double direction, double speed) {
            speedVector = Functions.createVector(direction, speed);

            if (radius == 0) {
                maxAngle = 0;
                return;
            }
            maxAngle = speed / radius;
        }


        /*вспомогательные функции*/


    }
}