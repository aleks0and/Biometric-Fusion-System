using Gui.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using System.Reflection;

namespace Gui.Model
{
    public class PersonData : BindableBase
    {
        private BitmapImage _image;
        public BitmapImage Image
        {
            get { return _image; }
            set { _image = value; Notify(); }
        } 

        public void LoadImage(string path)
        {
            path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\" + path;
            Image = new BitmapImage(new Uri(path));
        }
    }
}
