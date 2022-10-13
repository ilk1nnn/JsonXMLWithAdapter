using System;
using System.Collections.Generic;
using System.Windows;
using System.IO;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace JsonXMLWithAdapter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 


    public partial class MainWindow : Window
    {



        public MainWindow()
        {
            InitializeComponent();
        }




        public class User
        {
            

            public User()
            {

            }

            public string Name { get; set; }
            public string Surname { get; set; }
            public string Birthday { get; set; }
            public string Speciality { get; set; }

        }



        List<User> users = new List<User>();
        interface IAdapter
        {
            void CreateFile();
        }


        class JsonDb
        {

            public User User { get; set; }

            public JsonDb(User user)
            {
                User = user;
            }

            public void CreateFile()
            {
                var serializer = new JsonSerializer();

                using (var sw = new StreamWriter("FileJSON.json"))
                {
                    using (var jw = new JsonTextWriter(sw))
                    {
                        jw.Formatting = Formatting.Indented;
                        serializer.Serialize(jw, User);
                    }
                }
                MessageBox.Show("Created JsonFIle");
            }
        }

        class XMLDb
        {
            public User User { get; set; }

            public XMLDb(User user)
            {
                User = user;
            }

            public void CreateFile()
            {
                var xml = new XmlSerializer(typeof(User));
                using (var fs = new FileStream("FileXML.xml", FileMode.OpenOrCreate))
                {
                    xml.Serialize(fs,User);
                }
                MessageBox.Show("Created XMLFile");
            }
        }

        class JsonAdapter : IAdapter
        {
            private JsonDb _json_db;

            public JsonAdapter(JsonDb json_db)
            {
                _json_db = json_db;
            }

            public void CreateFile()
            {
                _json_db.CreateFile();
            }
        }


        class XMLAdapter : IAdapter
        {

            private XMLDb _xmldb;

            public XMLAdapter(XMLDb xmldb)
            {
                _xmldb = xmldb;
            }

            public void CreateFile()
            {
                _xmldb.CreateFile();
            }
        }

        class Converter
        {
            private readonly IAdapter _adapter;

            public Converter(IAdapter adapter)
            {
                _adapter = adapter;
            }

            public void CreateFile()
            {
                _adapter.CreateFile();
            }
        }

        class Application2
        {

            private readonly Converter _converter;
            public Application2(IAdapter adapter)
            {
                _converter = new Converter(adapter);
            }

            public void Start()
            {
                _converter.CreateFile();
            }

        }


        IAdapter adapter;

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            User user = new User();
            user.Name = nametxtb.Text;
            user.Surname = surnametxtb.Text;
            user.Birthday = birthtxtb.Text;
            user.Speciality = spectxtb.Text;
            JsonDb jsonDb = new JsonDb(user);
            adapter = new JsonAdapter(jsonDb);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Application2 application = new Application2(adapter);
                application.Start();
            }
            catch (Exception)
            {
                MessageBox.Show("You didn't choose");
            }
        }

        private void RadioButton_Click_1(object sender, RoutedEventArgs e)
        {
            User user = new User();
            user.Name = nametxtb.Text;
            user.Surname = surnametxtb.Text;
            user.Birthday = birthtxtb.Text;
            user.Speciality = spectxtb.Text;
            XMLDb xml = new XMLDb(user);
            adapter = new XMLAdapter(xml);
        }
    }
}
