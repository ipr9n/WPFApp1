using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

using System.Windows.Media;

namespace WpfApp1
{
    public class UserModel : INotifyPropertyChanged
    {
        private int _rank;
        private string _user;
        private string _status;
        private int _steps;
        private int _maxStep;
        private int _minStep;
        private int _averageStep;
        private string _color;

        private PointCollection diagramPointCollection = new PointCollection();

        private List<Days> _dayses = new List<Days>();

        public List<Days> Dayses
        {
            get { return _dayses;}
            set { OnPropertyChanged("Dayses"); }
        }

        public PointCollection DiagramPointCollection
        {
            get { return diagramPointCollection; }
            set { diagramPointCollection = value; OnPropertyChanged("DiagramPointCollection"); }
        }

        public int MaxStep
        {
            get { return _maxStep; }
            set { _maxStep = value;
                CheckColor(); OnPropertyChanged("MaxStep"); }
        }

        public int AverageStep
        {
            get { return _averageStep; }
            set { _averageStep = value;
                CheckColor(); OnPropertyChanged("AverageStep"); }
        }

        public string Color
        {
            get { return _color; }
            set { _color = value; }
        }

        void CheckColor()
        {
            if (AverageStep - (AverageStep * 0.2) > MinStep)
                Color = "Red";
            else if (AverageStep + (AverageStep * 0.2) < MaxStep)
                Color = "Red";
            else Color = "Black";
        }

        public int MinStep
        {
            get { return _minStep; }
            set { _minStep = value; OnPropertyChanged("MinStep"); }
        }


        public string User
        {
            get { return _user; }
            set { _user = value; OnPropertyChanged("User"); }
        }

        public int Rank
        {
            get { return _rank;}
            set { _rank = value; OnPropertyChanged("Rank"); }
        }

        public string Status
        {
            get { return _status; }
            set { _status = value; OnPropertyChanged("Status"); }
        }

        public int Steps
        {
            get { return _steps; }
            set { _steps = value;
                OnPropertyChanged("Steps");
            }
        }

        public void SetDiagramPoints(PointCollection points) => DiagramPointCollection = points;

        public void SetAverage()
        {
            if (Dayses == null) throw new ArgumentNullException(nameof(Dayses));
            AverageStep = (int)Dayses.Average(day => day.StepCount);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
