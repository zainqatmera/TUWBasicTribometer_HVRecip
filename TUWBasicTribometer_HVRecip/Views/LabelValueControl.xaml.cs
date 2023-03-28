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

namespace TUWBasicTribometer_HVRecip.Views
{
    /// <summary>
    /// Interaction logic for LabelValueControl.xaml
    /// </summary>
    public partial class LabelValueControl : UserControl
    {


        public object ValueContent
        {
            get { return (object)GetValue(ValueContentProperty); }
            set { SetValue(ValueContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ValueContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueContentProperty =
            DependencyProperty.Register("ValueContent", typeof(object), typeof(LabelValueControl), new PropertyMetadata(0));



        public object LabelContent
        {
            get { return (object)GetValue(LabelContentProperty); }
            set { SetValue(LabelContentProperty, value); }
        }

        public static readonly DependencyProperty LabelContentProperty = DependencyProperty.Register("LabelContent", typeof(object), typeof(LabelValueControl), new PropertyMetadata(0));



        public LabelValueControl()
        {
            InitializeComponent();
        }
    }
}
