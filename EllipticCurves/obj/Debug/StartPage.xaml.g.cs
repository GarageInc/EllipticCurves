//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EllipticCurves {
    using System;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;
    
    
    public partial class StartPage : ContentPage {
        
        private Button calculatorButton;
        
        private Button ellipticButton;
        
        private Button cryptoButton;
        
        private Button moreButton;
        
        private void InitializeComponent() {
            this.LoadFromXaml(typeof(StartPage));
            calculatorButton = this.FindByName<Button>("calculatorButton");
            ellipticButton = this.FindByName<Button>("ellipticButton");
            cryptoButton = this.FindByName<Button>("cryptoButton");
            moreButton = this.FindByName<Button>("moreButton");
        }
    }
}
