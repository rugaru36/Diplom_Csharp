using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplom_main {
    class CoordinatesList {
        public ListElement headElement; //начало списка
        public ListElement tailElement; //конец списка
        int elementsQuantity = 0;
        public double min_x, min_y, max_x, max_y;

        public void addElement(double[] data) {
            elementsQuantity++;
            ListElement newElement = new ListElement(data);

            //если список пуст
            if (headElement == null) {
                headElement = newElement;

                min_x = data[0];
                max_x = data[0];
                min_y = data[1];
                max_y = data[1];
            }
            else {
                tailElement.next = newElement;

                if (data[0] > max_x) max_x = data[0];
                if (data[1] > max_y) max_y = data[1];
                if (data[0] < min_x) min_x = data[0];
                if (data[1] < min_y) min_y = data[1];
            }
            tailElement = newElement;
        }
        public void normalizeCoordinates(int width, int height,
                                         double commonMin_x, double commonMin_y,
                                         double commonMax_x, double commonMax_y) {

            double whatToAdd_x = 0, whatToAdd_y = 0, whatToMultiply = 1;
            ListElement currentElement = headElement;

            //чтобы не было отрицательных значений
            if (commonMin_x < 0) {
                whatToAdd_x = Math.Abs(commonMin_x);
                min_x = 0;
            }
            if (commonMin_y < 0) {
                whatToAdd_y = Math.Abs(commonMin_y);
                min_y = 0;
            }

            double verticalCoeff = height / (commonMax_y + whatToAdd_y);
            double horizontalCoeff = width / (commonMax_x + whatToAdd_x);

            if (verticalCoeff < horizontalCoeff)
                whatToMultiply = verticalCoeff;

            else
                whatToMultiply = horizontalCoeff;

            while (currentElement.next != null) {
                currentElement.setNewValue(new double[] {
                    (currentElement.getValue()[0] + whatToAdd_x) * whatToMultiply,
                    (currentElement.getValue()[1] + whatToAdd_y) * whatToMultiply * (-1)
                });
                currentElement = currentElement.next;
            }
        }
    }
}
