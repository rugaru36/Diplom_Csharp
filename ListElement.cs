using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplom_main {
    class ListElement {
        private double[] coordinates;
        public ListElement next;

        public ListElement(double[] data) {
            coordinates = data;
        }
        public void setNewValueInExistingElement(double[] data) {
            this.coordinates = data;
        }
        public double[] getValue() {
            return coordinates;
        }
    }
}
