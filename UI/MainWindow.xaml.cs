using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using Core.Network;
using UI.Annotations;
using UI.Keyboard;
using UI.Theme;

namespace UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Port _port = (Port) 6672;

        private readonly Firewall _firewall = new Firewall();

        private readonly ITheme _theme = new DarkTheme();

        private HotKeyRegister _hotKeys;

        private bool _set = false;
        
        public ImageSource LockImageSource { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Init();
        }

        void Init()
        {
            Background = _theme.Background.ToBrush();

            // rules are default disabled
            FirewallButton.Background = _theme.RulesInactive.ToBrush();
        }

        public void RulesButtonClickHandler(object sender, RoutedEventArgs e)
        {
            SetRules();
        }

        void SetRules()
        {
            try
            {
                if (!_set)
                {
                    _firewall.Block(_port);

                    _set = true;
                    UpdateActive();
                }
                else
                {
                    _firewall.Unblock(_port);

                    _set = false;
                    UpdateNotActive();
                }
            }
            catch (Exception e)
            {
                AdminLabel.Visibility = Visibility.Visible;
                Console.Write(e);
            }
        }

        void UpdateNotActive()
        {
            FirewallButton.Background = _theme.RulesInactive.ToBrush();
            LockImage.Source = new BitmapImage(new Uri("/Assets/unlocked.png", UriKind.Relative));

            LockLabel.Content = "Rules not active." + Environment.NewLine + "Click to activate!";
        }

        void UpdateActive()
        {
            FirewallButton.Background = _theme.RulesActive.ToBrush();
            LockImage.Source = new BitmapImage(new Uri("/Assets/locked.png", UriKind.Relative));
            LockLabel.Content = "Rules active." + Environment.NewLine + "Click to deactivate!";
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            _hotKeys = HotKeys.Init(this);
            _hotKeys.Register(Key.F10, () => { SetRules(); }, ctrl: true);

            // delete the rules at startup to have a clean slate
            _firewall.DeleteRules();
        }

        protected override void OnClosed(EventArgs e)
        {
            _hotKeys.UnregisterAll();

            _firewall.DeleteRules();
            base.OnClosed(e);
        }
    }
}