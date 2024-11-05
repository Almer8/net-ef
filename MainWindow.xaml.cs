using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query.Internal;
using NetPract2.Model;
using NetPract2.Repository;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NetPract2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Regex numericRegex = new Regex(@"^[0-9]*(?:\.[0-9]*)?$");
        public Regex positiveIntRegex = new Regex(@"^[1-9]\d*$");
        DepositTypeRepository depositTypeRepository = new DepositTypeRepository();
        DepositRepository depositRepository = new DepositRepository();
        ClientRepository clientRepository = new ClientRepository();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitLoad();
        }

        private void InitLoad()
        {
            depTypesList.ItemsSource = depositTypeRepository.getAllDepositTypes();
            clientList.ItemsSource = clientRepository.getAllClients();
            depList.ItemsSource = depositRepository.getAllDeposits();
            fillClientComboBox();
            fillDepComboBox();
            depStartDate.Text = DateTime.Now.Date.ToShortDateString();
        }
        private void fillClientComboBox()
        {
            depClient.SelectedValuePath = "Key";
            depClient.DisplayMemberPath = "Value";
            depClient.ItemsSource = clientRepository.getAllClients()
                .Select(client => new KeyValuePair<int,string>(client.Id,
                String.Format("{0} | ID: {1}",client.Name,client.ClientID))).ToList();
        }
        private void fillDepComboBox()
        {
            depositType.SelectedValuePath = "Key";
            depositType.DisplayMemberPath = "Value";
            depositType.ItemsSource = depositTypeRepository.getAllDepositTypes()
                .Select(depType => new KeyValuePair<int, string>(depType.Id,
                String.Format("{0} | Percent: {1}% | Min Term: {2} months", depType.Name, depType.Percent,depType.MinTerm))).ToList();
        }

        private void clearDepTypesFields(object sender, RoutedEventArgs e)
        {
            depTypeName.Clear();
            depTypePercent.Clear();
            depTypeCompound.IsChecked = false;
            depTypeRegularity.Clear();
            depTypeMinTerm.Clear();
            depTypesList.SelectedIndex = -1;
        }
        private void depTypeSave_Click(object sender, RoutedEventArgs e)
        {
            if (depTypeName.Text.Length == 0 ||
                depTypePercent.Text.Length == 0 ||
                depTypeRegularity.Text.Length == 0 ||
                depTypeMinTerm.Text.Length == 0)
            {
                MessageBox.Show("Please fill all the values");
                return;
            }

            if(int.Parse(depTypeMinTerm.Text) % int.Parse(depTypeRegularity.Text) != 0){
                MessageBox.Show("Regularity must fit minimal term!");
                return;
            }
            try
            {
                DepositType depositType = new DepositType
                {
                    Name = depTypeName.Text,
                    Percent = float.Parse(depTypePercent.Text, CultureInfo.InvariantCulture),
                    CompoundInt = depTypeCompound.IsChecked == true,
                    Regularity = int.Parse(depTypeRegularity.Text),
                    MinTerm = int.Parse(depTypeMinTerm.Text)
                };

                if (depTypesList.SelectedIndex != -1)
                {
                    if (depTypesList.SelectedItem is Model.DepositType depositTypeDG)
                    {
                        depositType.Id = depositTypeDG.Id;
                    }
                    depositTypeRepository.updateDepositType(depositType);
                    fillDepComboBox();
                    clearDepTypesFields(null,null);
                    depTypesList.ItemsSource = depositTypeRepository.getAllDepositTypes();
                    return;
                }
                if (depositTypeRepository.getDepositTypeByName(depositType.Name) != null)
                {
                    MessageBox.Show("Deposit Type with this name already exists!");
                    return;
                }
                depositTypeRepository.createDepositType(depositType);
                clearDepTypesFields(null, null);
                fillDepComboBox();
                depTypesList.ItemsSource = depositTypeRepository.getAllDepositTypes();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
       }
        private void fillDepositTypesFields(object sender, SelectionChangedEventArgs e)
        {
            if (depTypesList.SelectedIndex != -1)
            {
                depTypeDel.IsEnabled = true;
                if (depTypesList.SelectedItem is Model.DepositType depositType)
                {
                    depTypeName.Text = depositType.Name;
                    depTypePercent.Text = depositType.Percent.ToString(CultureInfo.InvariantCulture);
                    depTypeCompound.IsChecked = depositType.CompoundInt == true;
                    depTypeRegularity.Text = depositType.Regularity.ToString(CultureInfo.InvariantCulture);
                    depTypeMinTerm.Text = depositType.MinTerm.ToString(CultureInfo.InvariantCulture);

                }
            }
            else
            {
                depTypeDel.IsEnabled = false;
            }
        }



        private void onlyNumeric(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            string newText = getNewText(textBox, e.Text);
            e.Handled = !numericRegex.IsMatch(newText);
        }

        private void positiveInt(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            string newText = getNewText(textBox, e.Text);
            e.Handled = !positiveIntRegex.IsMatch(newText);
        }

        private string getNewText(TextBox textBox, string inputText)
        {
            int caretIndex = textBox.SelectionStart;
            string textBefore = textBox.Text.Substring(0, caretIndex);
            string textAfter = textBox.Text.Substring(textBox.SelectionStart + textBox.SelectionLength);
            return textBefore + inputText + textAfter;
        }

        private void depTypeDel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (depTypesList.SelectedItem is Model.DepositType depositType)
                {
                    if (depositRepository.getAllDepositsWithDepositTypeId(depositType.Id).Count != 0)
                    {
                        MessageBox.Show("Deposits with this deposit types still existing!");
                        return;
                    }
                    depositTypeRepository.deleteDepositTypeById(depositType.Id);
                    clearDepTypesFields(null, null);
                    fillDepComboBox();
                    depTypesList.ItemsSource = depositTypeRepository.getAllDepositTypes();

                }
            } catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());    
            }
        }

        private void clearClientFields(object sender, RoutedEventArgs e)
        {
            clientName.Clear();
            clientID.Clear();
            clientDepsNum.Clear();
            clientList.SelectedIndex = -1;
        }




        private void clientSave_Click(object sender, RoutedEventArgs e)
        {
            if (clientName.Text.Length == 0 ||
                clientID.Text.Length == 0)
            {
                MessageBox.Show("Please fill all the values");
                return;
            }

            try
            {
                Client client = new Client
                {
                    Name = clientName.Text,
                    ClientID = clientID.Text
                };

                if (clientList.SelectedIndex != -1)
                {
                    if (clientList.SelectedItem is Model.Client clientDG)
                    {
                        client.Id = clientDG.Id;
                    }
                    clientRepository.updateClient(client);
                    clearClientFields(null,null);
                    fillClientComboBox();
                    clientList.ItemsSource = clientRepository.getAllClients();
                    return;
                }
                if (clientRepository.getClientByPassportID(client.ClientID) != null)
                {
                    MessageBox.Show("Client with this passport already exists");
                    return;
                }
                clientRepository.createClient(client);
                clearClientFields(null, null);
                fillClientComboBox();
                clientList.SelectedIndex = -1;
                clientList.ItemsSource = clientRepository.getAllClients();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void fillClientFields(object sender, SelectionChangedEventArgs e)
        {
            if (clientList.SelectedIndex != -1)
            {
                clientDel.IsEnabled = true;
                if (clientList.SelectedItem is Model.Client client)
                {

                   clientName.Text = client.Name;
                   clientID.Text = client.ClientID;
                   clientDepsNum.Text = depositRepository.getAllDepositsWithClientId(client.Id).Count.ToString();
                }
            }
            else
            {
                clientDel.IsEnabled = false;
            }
        }

        private void clientDel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (clientList.SelectedItem is Model.Client client)
                {
                    if(depositRepository.getAllDepositsWithClientId(client.Id).Count != 0)
                    {
                        MessageBox.Show("This client has unclosed deposits!");
                        return;
                    }
                    clientRepository.deleteClientById(client.Id);
                    clearClientFields(null, null);
                    fillClientComboBox();
                    clientList.ItemsSource = clientRepository.getAllClients();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void clearDepFields(object sender, RoutedEventArgs e)
        {
            depList.SelectedIndex = -1;
            depClient.SelectedIndex = -1;
            depositType.SelectedIndex = -1;
            depStartDate.Text = DateTime.Now.Date.ToShortDateString();
            depEndDate.SelectedDate = null;
            depMoney.Clear();
            profitMoney.Clear();
            depSave.IsEnabled = true;
        }

        private void depSave_Click(object sender, RoutedEventArgs e)
        {
            if (depClient.SelectedIndex == -1 ||
                depositType.SelectedIndex == -1 ||
                depEndDate.SelectedDate == null ||
                depMoney.Text == "" ||
                profitMoney.Text == "")
            {
                MessageBox.Show("Please fill all the values");
                return;
            }
            if(depEndDate.SelectedDate.GetValueOrDefault()
                .Subtract(DateTime.Now.Date).Days / (365.12/12)
                < depositTypeRepository.getDepositTypeById((int)depositType.SelectedValue).MinTerm)
            {
                MessageBox.Show("This deposit term is shorter than least allowed for this deposit type!");
                return;
            }

            double initDep;
            double total;

            bool successDep = double.TryParse(depMoney.Text,
                NumberStyles.Any, CultureInfo.InvariantCulture,
                out initDep);

            bool successTotal = double.TryParse(profitMoney.Text,
                NumberStyles.Any, CultureInfo.InvariantCulture,
                out total);

            if (successDep && successTotal)
            {
                Deposit deposit = new Deposit
                {
                    ClientId = (int)depClient.SelectedValue,
                    DepositTypeId = (int)depositType.SelectedValue,
                    DepositedMoney = initDep,
                    ProfitMoney = total,
                    StartDate = DateOnly.Parse(depStartDate.Text),
                    EndDate = DateOnly.Parse(depEndDate.Text),

                };
                try
                {
                    depositRepository.createDeposit(deposit);
                    depList.SelectedIndex = -1;
                    clearDepFields(null,null);
                    depList.ItemsSource = depositRepository.getAllDeposits();
                }
                catch (Exception ex) 
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            else
            {
                MessageBox.Show("Error while parsing data");
            }
            return;

        }

        private void calculateProfit(object sender, TextChangedEventArgs e)
        {
            if(depositType.SelectedIndex != -1 && depMoney.Text != "" && depEndDate.SelectedDate != null)
            {
                calculateProfit();
            }
        }

        private void calculateProfit(object sender, SelectionChangedEventArgs e)
        {
            if (depositType.SelectedIndex != -1 && depMoney.Text != "" && depEndDate.SelectedDate != null)
            {
                calculateProfit();
            }
        }
        private void calculateProfit()
        {
            DepositType type = depositTypeRepository.getDepositTypeById((int) depositType.SelectedValue);

            int periods = (int) Math.Floor(depEndDate.SelectedDate.GetValueOrDefault()
                .Subtract(DateTime.Now.Date).Days / (365.12 / 12));

            double initialDep;
            bool success = double.TryParse(depMoney.Text, 
                NumberStyles.Any, CultureInfo.InvariantCulture, 
                out initialDep);
            if (success)
            {
                if (type.CompoundInt)
                {
                    profitMoney.Text = (initialDep * (double)Math.Pow((1 + (double)type.Percent / 100), periods)).ToString("F2",
                        CultureInfo.InvariantCulture);
                }
                else
                {
                    profitMoney.Text = (initialDep + (initialDep * (type.Percent / 100) * periods)).ToString("F2",
                        CultureInfo.InvariantCulture);
                }
            }

        }

        private void fillDepositFields(object sender, SelectionChangedEventArgs e)
        {
            depSave.IsEnabled = false;
            if (depList.SelectedIndex != -1)
            {
                deleteDep.IsEnabled = true;
                if (depList.SelectedItem is Model.Deposit deposit)
                {
                    depClient.SelectedValue = deposit.ClientId;
                    depositType.SelectedValue = deposit.DepositTypeId;
                    depStartDate.Text = deposit.StartDate.ToString();
                    depEndDate.SelectedDate = deposit.EndDate.ToDateTime(TimeOnly.MinValue);
                    depMoney.Text = deposit.DepositedMoney.ToString();
                    profitMoney.Text = deposit.ProfitMoney.ToString();
                }
            }
            else
            {
                deleteDep.IsEnabled = false;
            }
        }

        private void deleteDep_Click(object sender, RoutedEventArgs e)
        {
            if (depList.SelectedItem is Deposit deposit) 
            {
                if (DateTime.Now.Date < deposit.EndDate.ToDateTime(TimeOnly.MinValue))
                {
                    var result = MessageBox.Show("You trying to close deposit before term. If you do it now, you'll lose all percents.", "Do you want to proceed?",
                        MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result != MessageBoxResult.Yes) 
                    {
                        return;
                    }
                    try
                    {
                        depositRepository.deleteDepositById(deposit.Id);
                        depList.ItemsSource = depositRepository.getAllDeposits();
                    }
                    catch (Exception ex) 
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
            }
        }

        private void filterByName(object sender, TextChangedEventArgs e)
        {
            if(nameFilter.Text == "")
            {
                depList.ItemsSource = depositRepository.getAllDeposits();
            }
            else
            {
                depList.ItemsSource = depositRepository.getAllDepositWhereClientNameContains(nameFilter.Text);
            }
        }   
    }
}