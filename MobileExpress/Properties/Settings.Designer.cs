﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil.
//     Version du runtime :4.0.30319.42000
//
//     Les modifications apportées à ce fichier peuvent provoquer un comportement incorrect et seront perdues si
//     le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MobileExpress.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "17.10.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("COM7")]
        public string PortScanner {
            get {
                return ((string)(this["PortScanner"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\Users\\merto\\Source\\MobileExpressApp\\Configuration\\Template facture.docx")]
        public string TemplateFacturePath {
            get {
                return ((string)(this["TemplateFacturePath"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\Users\\merto\\Source\\MobileExpressApp\\Configuration\\Template prise en charge.doc" +
            "x")]
        public string TemplateRecuPath {
            get {
                return ((string)(this["TemplateRecuPath"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\Users\\merto\\Source\\MobileExpressApp\\Configuration\\Logoeducdombleu.png")]
        public string LogoPath {
            get {
                return ((string)(this["LogoPath"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\Users\\merto\\Source\\MobileExpressApp\\Configuration\\Marques.csv")]
        public string MarquesDSPath {
            get {
                return ((string)(this["MarquesDSPath"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\Users\\merto\\Source\\MobileExpressApp\\Configuration\\Modèles.csv")]
        public string ModelesDSPath {
            get {
                return ((string)(this["ModelesDSPath"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\Users\\merto\\Source\\MobileExpressApp\\Configuration\\Types de déblocage.csv")]
        public string UnlockTypesDSPath {
            get {
                return ((string)(this["UnlockTypesDSPath"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\Users\\merto\\Source\\MobileExpressApp\\Configuration\\Types de réparation.csv")]
        public string RepairTypesDSPath {
            get {
                return ((string)(this["RepairTypesDSPath"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\Users\\merto\\Source\\MobileExpressApp\\Configuration\\Modes de paiement.csv")]
        public string PaymentModesDSPath {
            get {
                return ((string)(this["PaymentModesDSPath"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\Users\\merto\\Source\\MobileExpressApp\\Ticket de caisse.csv")]
        public string ReceiptDSPath {
            get {
                return ((string)(this["ReceiptDSPath"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\Users\\merto\\Source\\MobileExpressApp\\Client.csv")]
        public string CustomersDSPath {
            get {
                return ((string)(this["CustomersDSPath"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\Users\\merto\\Source\\MobileExpressApp\\Article.csv")]
        public string StockDSPath {
            get {
                return ((string)(this["StockDSPath"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\Users\\merto\\Source\\MobileExpressApp\\Prises en charges\\")]
        public string PriseEnChargeDirectory {
            get {
                return ((string)(this["PriseEnChargeDirectory"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\Users\\merto\\Source\\MobileExpressApp\\Factures\\")]
        public string FactureDirectory {
            get {
                return ((string)(this["FactureDirectory"]));
            }
        }
    }
}
