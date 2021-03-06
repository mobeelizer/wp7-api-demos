﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Com.Mobeelizer.Mobile.Wp7.Api;

namespace wp7_api_demos.View.Controls
{
    public partial class PhotoControl : UserControl
    {
        private IMobeelizerFile photo;

        public PhotoControl(IMobeelizerFile photo)
        {
            InitializeComponent();
            this.photo = photo;
            this.DataContext = this;
        }

        public IMobeelizerFile Photo
        {
            get
            {
                return photo;
            }
        }
    }
}
