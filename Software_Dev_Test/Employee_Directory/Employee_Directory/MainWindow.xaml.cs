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
using System.Collections.ObjectModel;
using System.IO;

namespace Employee_Directory
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<Employee> Employees = new ObservableCollection<Employee>();
        private string SaveFile;

        /// <summary>
        /// This will set the ListBox's ItemsSource to the Employees ObservableCollection.
        /// </summary>
        public MainWindow()
        {

            InitializeComponent();

            Employee_List.ItemsSource = Employees;
        }


        /// <summary>
        /// This method is called when the Insert button is pressed.
        /// It will pull up the Insert Input menu and also clear out the 
        /// TextBoxes from when the Insert Input menu was last opened
        /// </summary>
        private void InsertButton_Click(object sender, RoutedEventArgs e)
        {

            InsertInput.Visibility = System.Windows.Visibility.Visible;
            UpdateInput.Visibility = System.Windows.Visibility.Collapsed;
            PathLoading.Visibility = System.Windows.Visibility.Collapsed;
            Notify.Visibility = System.Windows.Visibility.Collapsed;

            InsertNameBox.Text = "";
            InsertTitleBox.Text = "";
        }

        /// <summary>
        /// This method is called when the Enter button is pressed in the Insert Input menu
        /// It will take the information from the two TextBox fields, then using them to create
        /// a new Employee Object, then adding it to the bottom of the Employees collection.
        /// 
        /// This will also close the Insert Input menu and open the Notify menu,
        /// notifying the user what was just added to the Directory.
        /// </summary>
        private void InsertInput_Click(object sender, RoutedEventArgs e)
        {

            InsertInput.Visibility = System.Windows.Visibility.Collapsed;

            String name = InsertNameBox.Text;
            String title = InsertTitleBox.Text;

            Employee newGuy  = new Employee()  { Name = name, Title = title };

            Employees.Add(newGuy);

            NotifyingMessage.Text = (name + "\n" + title + "\nAdded to the Employee Directory"  );

            Notify.Visibility = System.Windows.Visibility.Visible;

        }



        /// <summary>
        /// This method is called when the cancel button is pressed in the Insert Input menu
        /// It will just close the Insert Input menu without inserting anything.
        /// </summary>
        private void InsertCancel_Click(object sender, RoutedEventArgs e)
        {
            InsertInput.Visibility = System.Windows.Visibility.Collapsed;
        }

        /// <summary>
        /// This method is called when the Update button is pressed.
        /// It will pull up the Update Input menu if there is an employee selected in the ListBox.
        /// This will also clear out the TextBoxes from when the Update Input menu was last opened.
        /// </summary>
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (Employee_List.SelectedIndex != -1)
            {
                InsertInput.Visibility = System.Windows.Visibility.Collapsed;
                UpdateInput.Visibility = System.Windows.Visibility.Visible;
                PathLoading.Visibility = System.Windows.Visibility.Collapsed;
                Notify.Visibility = System.Windows.Visibility.Collapsed;

                UpdateNameBox.Text = Employees[Employee_List.SelectedIndex].Name;
                UpdateTitleBox.Text = Employees[Employee_List.SelectedIndex].Title;
            }
        }

        /// <summary>
        /// This method is called when the Enter button is pressed in the Update Input menu
        /// It will take the information from the two TextBox fields, then using them
        /// to update the selected Employee's information in the the Employee Directory.
        /// 
        /// This will also close the Update Input menu and open the Notify menu,
        /// notifying the user what was the previous was and what it was changed to.
        /// </summary>
        private void UpdateInput_Click(object sender, RoutedEventArgs e)
        {

                UpdateInput.Visibility = System.Windows.Visibility.Collapsed;

                String name = UpdateNameBox.Text;
                String title = UpdateTitleBox.Text;

                Employee newGuy = new Employee() { Name = name, Title = title };

                NotifyingMessage.Text = (Employees[Employee_List.SelectedIndex].Name + " Updatd to: " + name +
                    "\n" + Employees[Employee_List.SelectedIndex].Title + " Updated to: " + title);
               
                Employees[Employee_List.SelectedIndex] = newGuy;

                Notify.Visibility = System.Windows.Visibility.Visible;

        }

        /// <summary>
        /// This method is called when the cancel button is pressed in the Update input menu
        /// It will just close the update input menu without changing anything.
        /// </summary>
        private void UpdateCancel_Click(object sender, RoutedEventArgs e)
        {

            UpdateInput.Visibility = System.Windows.Visibility.Collapsed;

        }

        /// <summary>
        /// This method is called when the Delete button is pressed.
        /// It will remove the selected employee in the ListBox
        /// 
        /// This also opens the Notify menu, for notifying the user 
        /// of who was just removed from the Employee Directory.
        /// </summary>
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (Employee_List.SelectedIndex != -1)
            {
                NotifyingMessage.Text = (Employees[Employee_List.SelectedIndex].Name + ": " + 
                    Employees[Employee_List.SelectedIndex].Title + " was removed from the Employee Directory.");

                Employees.RemoveAt(Employee_List.SelectedIndex);

                Notify.Visibility = System.Windows.Visibility.Visible;

            }

        }

        /// <summary>
        /// This method is called when the Load button is pressed.
        /// This will pull up the PathLoading Menu to get the path the SaveFile 
        /// </summary>
        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            PathLoading.Visibility = System.Windows.Visibility.Visible;
            FileButton.Visibility = System.Windows.Visibility.Visible;

            InsertInput.Visibility = System.Windows.Visibility.Collapsed;
            UpdateInput.Visibility = System.Windows.Visibility.Collapsed;
            Notify.Visibility = System.Windows.Visibility.Collapsed;
        }

        /// <summary>
        /// This method is called when the Browse button is pressed in the PathLoading menu.
        /// It will get a path from file explorer and set the PathBox text to the path returned.
        /// </summary>
        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            PathBox.Text = GetFilePath();
        }

        /// <summary>
        /// This method is called when the Enter button is pressed in the PathLoading menu
        /// This will set the SaveFile to the text entered in the PathBox.
        /// then it will load the employees into the directory from that file.
        /// </summary>
        private void LoadEnter_Click(object sender, RoutedEventArgs e)
        {
            SaveFile = PathBox.Text;
            ReadSaveFile();

            PathLoading.Visibility = System.Windows.Visibility.Collapsed;

            Console.WriteLine("DeBug: SaveFile = " + SaveFile);

        }

        /// <summary>
        /// This function will open the file explorer to select a .txt file
        /// </summary>
        /// <returns> The full path of a selected .txt file or returns a message if an error occured </returns>
        private string GetFilePath()
        {
            Microsoft.Win32.OpenFileDialog file = new Microsoft.Win32.OpenFileDialog();

            file.DefaultExt = ".txt";
            file.Filter = "Text|*.txt|All|*.*";

            Nullable<bool> result = file.ShowDialog();
            if(result == true)
            {
                return file.FileName;
            }
            else
            {
                return "Error: type in file path";
            }

        }

        /// <summary>
        /// This method is called when the save button is pressed.
        /// It will overwrite the save file you have selected.
        /// It will loop through the Employees collection writing the 
        /// name and title on separate line for each employee.
        /// This is also writing a message to the console for debuging purposes. 
        /// </summary>
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFile = PathBox.Text;
            PathLoading.Visibility = System.Windows.Visibility.Collapsed;

            if (String.IsNullOrEmpty(SaveFile) || !File.Exists(SaveFile) )
            {

                PathLoading.Visibility = System.Windows.Visibility.Visible;
                FileButton.Visibility = System.Windows.Visibility.Collapsed;

                PathBox.Text = "Set an Existing Save file, Then click Save";
            }
            else
            {
                using (StreamWriter writer = new StreamWriter(SaveFile, false))
                {
                    foreach (Employee em in Employees)
                    {
                        writer.WriteLine(em.Name);
                        writer.WriteLine(em.Title);

                        Console.WriteLine("Debug: Employee " + em.Name + " was saved to Directory file");
                    }

                    Console.WriteLine("Debug: All employees were saved to the Directory file");

                }

                NotifyingMessage.Text = ("The Current Directory was saved into the Save File:\n" 
                    + SaveFile);
                Notify.Visibility = System.Windows.Visibility.Visible;
            }
        }

        /// <summary>
        /// This Method will clear out the Employees collection then
        /// reads the save file the ueser give; reading two lines at a time
        ///     First line read will contain the Name of the employee. 
        ///     Second line read will contain the job Title of the employee.
        /// Both the name and title are then added to the put into an Employee object
        /// and added to the Employees Colletion.
        /// This is also writing a message to the console for debuging purposes.
        /// </summary>
        private void ReadSaveFile()
        {
            Employees.Clear();

            if (File.Exists(SaveFile))
            {
                using (var fileStream = File.OpenRead(SaveFile))
                {
                    using (StreamReader reader = new StreamReader(fileStream, Encoding.UTF8, true, 128))
                    {
                        String name;
                        String title;

                        while (((name = reader.ReadLine()) != null) && ((title = reader.ReadLine()) != null))
                        {
                            Employees.Add(new Employee() { Name = name, Title = title });

                            Console.WriteLine("debug:   Loading Employee " + name + ": " + title);
                        }
                    }
                }
            }
            else
            {
                PathBox.Text = "File not found: choose another";
                PathLoading.Visibility = System.Windows.Visibility.Visible;
            }
        }

        /// <summary>
        /// This method is called when the OK button is pressed in the Notify menu.
        /// It will close the notify menu.
        /// </summary>
        private void Notify_Click(object sender, RoutedEventArgs e)
        {

            Notify.Visibility = System.Windows.Visibility.Collapsed;

        }


    }
}

/// <summary>
/// An employee object 
/// </summary>
public class Employee
{ 
    public string Name { get; set; }
    public string Title { get; set; }

}

