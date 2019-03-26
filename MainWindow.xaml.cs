using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace Diplom_main {

    public partial class MainWindow : Window {

        //ПОЛЯ
        private string oldValue = "";
        private bool isDataCorrect = true;
        private string errorMessage = "";
        private Game game;
        private int graphLeft, graphRight, graphBottom, graphTop, /*graphMiddle_y,*/ graphHeight, graphWidth;
        private Polyline p_line, e_line;
        //МЕТОДЫ

        public MainWindow() {
            InitializeComponent();
            game = new Game();

            graphLeft = Convert.ToInt32(graphGroup.Margin.Left) + 5;
            graphRight = graphLeft + Convert.ToInt32(graphGroup.Width) - 5;

            //graphMiddle_y = Convert.ToInt32(graphGroup.Margin.Top + (graphGroup.Height / 2));
            graphWidth = Convert.ToInt32(graphGroup.Width) - 10;
            graphHeight = Convert.ToInt32(graphGroup.Height) - 17;

            graphTop = Convert.ToInt32(graphGroup.Margin.Top) + 7;
            graphBottom = graphTop + Convert.ToInt32(graphGroup.Height) - 10;
        }
        private void setPrevValue(object sender, RoutedEventArgs e) {
            TextBox currentTb = (TextBox)sender;
            this.oldValue = currentTb.Text;
        }
        private void drawUsualLines() {

            if (p_line != null) {
                mainGrid.Children.Remove(p_line);
                mainGrid.Children.Remove(e_line);
            }

            game.E_PlayerCoordinatesList.normalizeCoordinates(graphWidth, graphHeight, 
                                                              game.min_x, game.min_y, game.max_x, game.max_y);

            game.P_PlayerCoordinatesList.normalizeCoordinates(graphWidth, graphHeight,
                                                              game.min_x, game.min_y, game.max_x, game.max_y);

            p_line = new Polyline();
            e_line = new Polyline();

            e_line.Stroke = Brushes.Red;
            p_line.Stroke = Brushes.Blue;

            mainGrid.Children.Add(p_line);
            mainGrid.Children.Add(e_line);

            ListElement current_pPoint = game.P_PlayerCoordinatesList.headElement;
            ListElement current_ePoint = game.E_PlayerCoordinatesList.headElement;

            while (current_pPoint.next != null && current_ePoint.next != null) {
                p_line.Points.Add(new Point(
                    current_pPoint.getValue()[0] + graphLeft,
                    current_pPoint.getValue()[1] + graphBottom));

                e_line.Points.Add(new Point(
                    current_ePoint.getValue()[0] + graphLeft,
                    current_ePoint.getValue()[1] + graphBottom));

                current_pPoint = current_pPoint.next;
                current_ePoint = current_ePoint.next;
            }

        }
        private void checkInputData() {
            this.isDataCorrect = true;

            int iterationsNum = Convert.ToInt16(Game_InputIterations.Text);
            double accuracy, step, pSpeed, eSpeed, eRadius, pRadius;
            accuracy = Convert.ToDouble(fixSeparator(Game_InputAccuracy.Text));
            step = Convert.ToDouble(fixSeparator(Game_InputStep.Text));

            pSpeed = Convert.ToDouble(fixSeparator(P_InputSpeedVectorLength.Text));
            eSpeed = Convert.ToDouble(fixSeparator(E_InputSpeedVectorLength.Text));

            pRadius = Convert.ToDouble(fixSeparator(P_inputRadius.Text));
            eRadius = Convert.ToDouble(fixSeparator(E_inputRadius.Text));


            if (accuracy < 0.01) {
                this.isDataCorrect = false;
                this.errorMessage = "Точность слишком мала!";
                return;
            }

            else if (iterationsNum > 10000) {
                this.isDataCorrect = false;
                this.errorMessage = "Количество итераций слишком велико!\n(Максимум - 10000)";
                return;
            }

            else if (iterationsNum < 500) {
                this.isDataCorrect = false;
                this.errorMessage = "Количество итераций слишком мало!\n(Минимум - 500)";
                return;
            }

            else if (step < 0.01) {
                this.isDataCorrect = false;
                this.errorMessage = "Величина шага слишком мала!";
                return;
            }

            else if (eSpeed < 0 && pSpeed < 0) {
                this.isDataCorrect = false;
                this.errorMessage = "Скорость одного или двух игроков меньше нуля!";
                return;
            }

            else if (eRadius < 0 && pRadius < 0) {
                this.isDataCorrect = false;
                this.errorMessage = "Радиус поворота одного или двух игроков меньше нуля!";
                return;
            }

            else if (accuracy < step) {
                this.isDataCorrect = false;
                this.errorMessage = "Точность должна быть больше шага!";
                return;
            }

            else if (pSpeed < eSpeed) {
                this.isDataCorrect = false;
                this.errorMessage = "Убегающий не должен быть быстрее преследователя!";
                return;
            }

            else if (pRadius < eRadius) {
                this.isDataCorrect = false;
                this.errorMessage = "Радиус убегающего не должен быть больше радиуса преследователя!";
                return;
            }
        }
        private static string fixSeparator(string inputData) {
            bool toFix = false;
            string result = "";

            for (int i = 0; i < inputData.Length; i++) {
                if (inputData[i] == '.') {
                    toFix = true;
                    for (int j = 0; j < inputData.Length; j++) {
                        if (i == j) {
                            result += ",";
                            continue;
                        }
                        result += inputData[j];
                    }
                    break;
                }
            }
            if (!toFix) result = inputData;

            return result;
        }
        private void button_Click(object sender, RoutedEventArgs e) {
            checkInputData();

            if (!isDataCorrect) {
                MessageBox.Show(errorMessage);
                return;
            }

            setDataBeforeStart();
            Label_outputResult.Content = "";

            if (isDataCorrect) {
                Label_outputResult.Content = game.iterationsProcess();
            }

            drawUsualLines();
        }
        private void setDataBeforeStart() {

            double a = Convert.ToDouble(fixSeparator(E_InputXCoordinate.Text));

            game.E_Player.setXCoordinate(Convert.ToDouble(fixSeparator(E_InputXCoordinate.Text)));
            game.E_Player.setYCoordinate(Convert.ToDouble(fixSeparator(E_InputYCoordinate.Text)));
            game.E_Player.createSpeedVector(Convert.ToDouble(fixSeparator(E_InputSpeedVectorDirection.Text)),
                Convert.ToDouble(fixSeparator(E_InputSpeedVectorLength.Text)));

            

            game.E_Player.setRadius(Convert.ToDouble(fixSeparator(E_inputRadius.Text)));

            game.P_Player.setXCoordinate(Convert.ToDouble(P_InputXCoordinate.Text));
            game.P_Player.setYCoordinate(Convert.ToDouble(P_InputYCoordinate.Text));
            game.P_Player.createSpeedVector(Convert.ToDouble(fixSeparator(P_InputSpeedVectorDirection.Text)),
                Convert.ToDouble(fixSeparator(P_InputSpeedVectorLength.Text)));

            game.P_Player.setRadius(Convert.ToDouble(fixSeparator(P_inputRadius.Text)));

            game.setIterations(Convert.ToInt32(Game_InputIterations.Text));
            game.setAccuracy(Convert.ToDouble(fixSeparator(Game_InputAccuracy.Text)));
            game.setStepSize(Convert.ToDouble(fixSeparator(Game_InputStep.Text)));
        }
        private void checkInputDataTypes(object sender, RoutedEventArgs e) {

            TextBox currentTb = (TextBox)sender;

            if (currentTb.Name == "Game_InputIterations") {
                int buff;
                if (int.TryParse(currentTb.Text, out buff)) return;

                else {
                    MessageBox.Show("Ошибка! Будет возвращено предыдущее значение");
                    currentTb.Text = this.oldValue;
                }
            }
            else {
                double buff;
                if (double.TryParse(fixSeparator(currentTb.Text), out buff)
                    && currentTb.Text.Length == buff.ToString().Length) return;

                else {
                    MessageBox.Show("Ошибка! Будет возвращено предыдущее значение");
                    currentTb.Text = this.oldValue;
                }
            }
        }
    }
}
