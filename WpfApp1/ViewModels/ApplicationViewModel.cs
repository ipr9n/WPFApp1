using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;
using Newtonsoft.Json;
using WpfApp1.Services;

namespace WpfApp1
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        FileService fileService = new FileService();

        private UserModel _selectedUser;
        public ObservableCollection<UserModel> Users { get; set; }

        public ApplicationViewModel() => Users = fileService.LoadUsersFromFile();

        public UserModel SelectedUser
        {
            get
            {
                SetDiagramPoint(_selectedUser);
                return _selectedUser;
            }
            set
            {
                _selectedUser = value;
                OnPropertyChanged("SelectedUser");
            }
        }

        private void SetDiagramPoint(UserModel user)
        {
            try
            {
                PointCollection pointToDiagram = new PointCollection();
                var allSteps = _selectedUser.Dayses.Select(x => x.StepCount).ToArray();
                int currentXPos = 10;
                pointToDiagram.Add(new Point(currentXPos, 450));

                foreach (var step in allSteps)
                {
                    currentXPos += 13;
                    pointToDiagram.Add(new Point(currentXPos, Math.Abs(450 - (step / 450))));
                }

                _selectedUser.SetDiagramPoints(pointToDiagram);
            }
            catch (Exception e)
            {

            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        private MyCommand saveCommand;
        public MyCommand SaveCommand
        {
            get
            {
                return saveCommand ??
                       (saveCommand = new MyCommand(obj =>
                           {
                               try
                               {
                                   DialogService saveDialog = new DialogService();

                                   if (saveDialog.SaveFileDialog(out DialogService.TypeIndex ind) == true)
                                       fileService.SaveUser(saveDialog.FilePath, obj as UserModel,ind);
                               }
                               catch (Exception ex)
                               {
                                   MessageBox.Show($"Выберите пользователя \n"+ex.Message);
                               }
                           }, (obj) => Users.Count > 0));
            }
        }
    }
}
