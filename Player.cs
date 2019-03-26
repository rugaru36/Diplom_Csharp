using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;


namespace Diplom_main {
    abstract class Player {

        //поля

        protected Vector speedVector;
        protected double[] coordinates = new double[2];
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

            Vector toPoint1 = new Vector(getSpeedDirection() + 90, radius * lengthToPointCoeff);
            Vector toPoint2 = new Vector(getSpeedDirection() - 90, radius * lengthToPointCoeff);

            double[] radPoint1 = new double[2] {
                    toPoint1.getCoordinates()[0] + coordinates[0],
                    toPoint1.getCoordinates()[1] + coordinates[1] };

            double[] radPoint2 = new double[2] {
                    toPoint2.getCoordinates()[0] + coordinates[0],
                    toPoint2.getCoordinates()[1] + coordinates[1] };

            result[0, 0] = radPoint1[0];
            result[0, 1] = radPoint1[1];
            result[1, 0] = radPoint1[0];
            result[1, 1] = radPoint1[1];

            return result;
        }
        private void moveToWantedPoint(double stepSize, double[] wantedPoint) {

            Vector jVector = new Vector(coordinates, wantedPoint);

            double wantedDirection = jVector.getDirection();

            //безынерционный объект
            if (!getIsInerted()) setSpeedVectorDirection(wantedDirection);

            //инерция
            else if (getIsInerted()) {
                double currentDirection = speedVector.getDirection();

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
            coordinates[0] += speedVector.getCoordinates()[0] * stepSize;
            coordinates[1] += speedVector.getCoordinates()[1] * stepSize;
        }

        //геттеры
        public double getRadius() {
            return radius;
        }
        public double getSpeedVectorLength() {
            return speedVector.getLength();
        }
        public double getSpeedDirection() {
            return speedVector.getDirection();
        }
        public double[] getCoordinates() {
            return coordinates;
        }
        public double[] getSpeedVector() {
            return speedVector.getCoordinates();
        }
        public bool getIsInerted() {
            return radius > 0.1;
        }

        //сеттеры
        public void setRadius(double newRadius) {
            radius = newRadius;

            if (radius > 0) {
                maxAngle = speedVector.getLength() / radius;
            }
        }
        public void setSpeedVectorLength(double newSpeed) {
            speedVector.setLength(newSpeed);
            if (radius > 0) {
                maxAngle = newSpeed / radius;
                return;
            }
        }
        public void setSpeedVectorDirection(double newDirection) {
            speedVector.setDirection(newDirection);
        }
        public void setXCoordinate(double newXCoordinate) {
            this.coordinates[0] = newXCoordinate;
        }
        public void setYCoordinate(double newYCoordinate) {
            this.coordinates[1] = newYCoordinate;
        }
        public void createSpeedVector(double direction, double speed) {
            speedVector = new Vector(speed, direction);

            if (radius == 0) {
                maxAngle = 0;
                return;
            }
            maxAngle = speed / radius;
        }
    }
}