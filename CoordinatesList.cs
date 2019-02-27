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
    }
}
