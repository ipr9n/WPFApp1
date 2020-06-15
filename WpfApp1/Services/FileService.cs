using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using Newtonsoft.Json;
using ServiceStack.Text;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;
using XmlSerializer = System.Xml.Serialization.XmlSerializer;

namespace WpfApp1.Services
{
    class FileService
    {
        public void SaveUser(string filename, UserModel user, DialogService.TypeIndex index)
        {
            var User = new { user.User, user.AverageStep, user.MaxStep, user.MinStep, user.Dayses };

            switch (index)
            {
                case DialogService.TypeIndex.Json:
                    JsonSerializer serializer = new JsonSerializer();

                    using (StreamWriter sw = new StreamWriter(filename, false, System.Text.Encoding.Default))
                    using (JsonWriter writer = new JsonTextWriter(sw))
                        serializer.Serialize(writer, User);
                    break;

                case DialogService.TypeIndex.XML:
                    XmlSerializer formatter = new XmlSerializer(typeof(UserModel));

                    using (StreamWriter sw = new StreamWriter(filename, false, System.Text.Encoding.Default))
                        formatter.Serialize(sw, User);
                    break;

                case DialogService.TypeIndex.CSV:
                    var serialize = CsvSerializer.SerializeToString(User);

                    using (StreamWriter sw = new StreamWriter(filename, false, System.Text.Encoding.Default))
                        sw.Write(serialize);
                    break;
            }
        }

        public ObservableCollection<UserModel> LoadUsersFromFile()
        {
            ObservableCollection<UserModel> Users = new ObservableCollection<UserModel>();

            for (int i = 1; i <= 30; i++)
            {
                try
                {
                    string file = File.ReadAllText("days\\day" + i + ".json");
                    var result = JsonConvert.DeserializeObject<ObservableCollection<UserModel>>(file);

                    foreach (var userModel in result)
                    {
                        UserModel user = Users.FirstOrDefault(t => t.User == userModel.User);

                        if (user != null)
                        {
                            user.Dayses.Add(new Days
                            { DayNumber = i, Rank = userModel.Rank, Status = userModel.Status, StepCount = userModel.Steps });

                            if (user.MinStep > userModel.Steps)
                                user.MinStep = userModel.Steps;
                            if (user.MaxStep < userModel.Steps)
                                user.MaxStep = userModel.Steps;

                            user.SetAverage();
                        }
                        else
                        {
                            userModel.Dayses.Add(new Days
                            { DayNumber = i, Rank = userModel.Rank, Status = userModel.Status, StepCount = userModel.Steps });
                            userModel.MaxStep = userModel.Steps;
                            userModel.MinStep = userModel.Steps;
                            userModel.SetAverage();
                            Users.Add(userModel);
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Тестовые файлы не найдены. Поместите папку days в папку с исполняемым файлом\n {e.Message} ");
                    throw;
                }
            }

            return Users;
        }
    }
}
